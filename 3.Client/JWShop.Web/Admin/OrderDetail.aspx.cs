using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using Microsoft.AspNet.SignalR;

namespace JWShop.Web.Admin
{
    public partial class OrderDetail : JWShop.Page.AdminBasePage
    {
        protected int sendPoint = 0;
        protected OrderInfo order = new OrderInfo();
        protected List<OrderActionInfo> orderActionList = new List<OrderActionInfo>();
        protected bool canEdit = false;
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();

        protected string shippingLink = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { //检查待付款订单是否超时失效，超时则更新为失效状态
                OrderBLL.CheckOrderPayTime();
                int orderId = RequestHelper.GetQueryString<int>("Id");
                sendPoint = OrderBLL.ReadOrderSendPoint(orderId);
                if (orderId != int.MinValue)
                {
                    CheckAdminPower("ReadOrder", PowerCheckType.Single);
                    order = OrderBLL.Read(orderId);
                    order.UserName = System.Web.HttpUtility.UrlDecode(order.UserName, System.Text.Encoding.UTF8);
                    int isCod = PayPlugins.ReadPayPlugins(order.PayKey).IsCod;
                    if ((order.OrderStatus == (int)OrderStatus.WaitPay || order.OrderStatus == (int)OrderStatus.WaitCheck && isCod == (int)BoolType.True) && order.IsActivity == (int)BoolType.False)
                    {
                        canEdit = true;
                    }
                    orderActionList = OrderActionBLL.ReadList(orderId);
                    ShowButton(order);

                    #region 获取快递信息
                    if (order.OrderStatus == (int)OrderStatus.HasShipping)
                    {
                        ShippingInfo tempShipping = ShippingBLL.Read(order.ShippingId);

                        string show = RequestHelper.GetQueryString<string>("show");

                        //string apiurl = "http://api.kuaidi100.com/api?id=2815b2d431fdfd26&com=" + typeCom + "&nu=" + nu + "&show=" + show + "&muti=1&order=asc";
                        string apiurl = "http://www.kuaidi100.com/applyurl?key=2815b2d431fdfd26&com=" + tempShipping.ShippingCode + "&nu=" + order.ShippingNumber + "&show=" + show + "&muti=1&order=desc";
                        //Response.Write (apiurl);
                        WebRequest request = WebRequest.Create(@apiurl);
                        WebResponse response = request.GetResponse();
                        Stream stream = response.GetResponseStream();
                        Encoding encode = Encoding.UTF8;
                        StreamReader reader = new StreamReader(stream, encode);

                        shippingLink = reader.ReadToEnd();
                    }
                    #endregion

                    //正在处理中的退款订单或商品
                    orderRefundList = OrderRefundBLL.ReadListValid(orderId);
                    //有正在处理中的退款订单或商品，禁用功能按钮
                    if (orderRefundList.Count(k => !OrderRefundBLL.HasReturn(k.Status)) > 0)
                    {
                        divOperate.Visible = false;
                        divNotice.Visible = true;
                        lblNotice.Text = "该订单有正在处理中的退款订单或商品...";
                    }
                }
                //如果付款操作，发送signalr消息
                if (RequestHelper.GetQueryString<int>("paysuccess")==1)
                {
                    IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<PushHub>();
                    chat.Clients.All.sendMessage(1);
                }
            }

            //不支持退货和退款操作，如果要退货退款，请线下操作
            ReturnButton.Visible = false;
        }

        /// <summary>
        /// 判断按纽是否显示
        /// </summary>
        protected void ShowButton(OrderInfo order)
        {
            //如果订单已删除则只显示恢复按钮
            if (order.IsDelete == (int)BoolType.True) {
                RecoverButton.Visible = true;
            }
            else
            {
                //付款
                //if (order.OrderStatus == (int)OrderStatus.WaitPay)
                //{
                //    PayButton.Visible = true;
                //}
                //审核/取消
                if (order.OrderStatus == (int)OrderStatus.WaitPay || order.OrderStatus == (int)OrderStatus.WaitCheck)
                {
                    CheckButton.Visible = true;
                    CancelButton.Visible = true;
                }
                //安装
                if (order.OrderStatus == (int)OrderStatus.Shipping && order.Shuigong_stat==0)
                {
                    AnzhuangButton.Visible = true;

                }
                //试压
                if (order.OrderStatus == (int)OrderStatus.HasShipping && order.Shuigong_stat == 0)
                {
                    SendButton.Visible = true;
                }
                //完成确认
                if (order.OrderStatus == (int)OrderStatus.HasShipping && order.Shuigong_stat == 1)
                {
                    ReceivedButton.Visible = true;
                }
                //换货确认
                //if (order.OrderStatus == (int)OrderStatus.HasShipping)
                //{
                //    ChangeButton.Visible = true;
                //}
                //退货确认
                //if (order.OrderStatus == (int)OrderStatus.HasShipping)
                //{
                //    ReturnButton.Visible = true;
                //}
                //撤销
                if (orderActionList.Count > 0 && order.IsRefund != (int)BoolType.True && order.OrderStatus != (int)OrderStatus.WaitPay)
                {
                    BackButton.Visible = true;
                }
            }
        }

        /// <summary>
        /// 恢复按钮点击方法
        /// </summary>
        protected void RecoverButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            if (order.Id > 0 && order.IsDelete == (int)BoolType.True)
            {
                order.IsDelete = (int)BoolType.False;
                OrderBLL.Update(order);
            }
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("OperateOK"), RequestHelper.RawUrl);
        }

        /// <summary>
        /// 付款按钮点击方法
        /// </summary>
        protected void PayButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.PayDate = RequestHelper.DateNow;
            order.OrderStatus = (int)OrderStatus.WaitCheck;
            ButtonEnd(order, Note.Text, OrderOperate.Pay, startOrderStatus);          
        }

        /// <summary>
        /// 审核按钮点击方法
        /// </summary>
        protected void CheckButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            #region  拼团单，未拼满或者拼团失败不能通过审核
            if (order.IsActivity == (int)OrderKind.GroupBuy)
            {
                var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                if(!(groupBuy.StartTime<=DateTime.Now && groupBuy.EndTime>=DateTime.Now && groupBuy.SignCount >= groupBuy.Quantity))
                {
                    ScriptHelper.Alert("拼团未成功，暂不能通过审核");
                }
            }
            #endregion
            int startOrderStatus = order.OrderStatus;

            //确认付款,收款时间
            order.PayDate = RequestHelper.DateNow;

            order.ShippingNumber = ShippingNumber.Text;
            order.ShippingDate = Convert.ToDateTime(ShippingDate.Text);
            //更新商品库存数量
            ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);

            //自提：现场审核提货，完成
            if (order.SelfPick == 1)
            {
                order.OrderStatus = (int)OrderStatus.ReceiveShipping;
                #region 提货码状态置为1（无效）           
                PickUpCodeBLL.UsePickCodeByOrder(order.Id);
                #endregion
                #region 完成订单给分销商返佣
                //订单实际支付金额
                decimal paid_money = OrderBLL.ReadNoPayMoney(order);
                //购买人
                var user = UserBLL.Read(order.UserId);
                //购买者有推荐人 且 实际支付金额大于0才进行返佣
                if (user.Recommend_UserId > 0 && paid_money > 0)
                {
                    RebateBLL.RebateToDistributor(user, paid_money,order.Id);
                }
                #endregion
            }
            else
            {//配送：进入配货状态
                order.OrderStatus = (int)OrderStatus.Shipping;
            }
            ButtonEnd(order, Note.Text, OrderOperate.Check, startOrderStatus);
        }

        /// <summary>
        /// 取消按钮点击方法
        /// </summary>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)OrderStatus.NoEffect;
            #region 待付款状态，退还用户下单时抵现的积分
            if (startOrderStatus ==(int)OrderStatus.WaitPay && order.Point > 0)
            {
                var accountRecord = new UserAccountRecordInfo
                {
                    RecordType = (int)AccountRecordType.Point,
                    Money = 0,
                    Point = order.Point,
                    Date = DateTime.Now,
                    IP = ClientHelper.IP,
                    Note = "取消订单：" + order.OrderNumber + "，退回用户积分",
                    UserId =order.UserId,
                    UserName = order.UserName,
                };
                UserAccountRecordBLL.Add(accountRecord);
            }
            #endregion
            //更新商品库存数量
            ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
            ButtonEnd(order, Note.Text, OrderOperate.Cancle, startOrderStatus);
        }

        /// <summary>
        /// 安装按钮点击方法
        /// </summary>
        protected void AnzhuangButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)OrderStatus.HasShipping;
            order.Shuigong_stat = 1;

            ButtonEnd(order, Note.Text, OrderOperate.NoSend, startOrderStatus);
        }

        /// <summary>
        /// 试压按钮点击方法
        /// </summary>
        protected void SendButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)OrderStatus.HasShipping;
            order.Shiya_stat = 1;
            
            ButtonEnd(order, Note.Text, OrderOperate.Send, startOrderStatus);
        }

        /// <summary>
        /// 完成确认按钮点击方法
        /// </summary>
        protected void ReceivedButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            #region 确认收货赠送优惠券
            int count = 0;
            var couponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.ReceiveShippingGet, CanUse = 1 }, ref count);
            if (couponlist.Count > 0)
            {
                UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponlist[0].Id);
                int startNumber = 0;
                if (userCoupon.Id > 0)
                {
                    string tempNumber = userCoupon.Number.Substring(3, 5);
                    while (tempNumber.Substring(0, 1) == "0")
                    {
                        tempNumber = tempNumber.Substring(1);
                    }
                    startNumber = Convert.ToInt32(tempNumber);
                }
                startNumber++;
                UserCouponBLL.Add(new UserCouponInfo
                {
                    UserId = order.UserId,
                    UserName = order.UserName,
                    CouponId = couponlist[0].Id,
                    GetType = (int)CouponType.ReceiveShippingGet,
                    Number = ShopCommon.CreateCouponNo(couponlist[0].Id, startNumber),
                    Password = ShopCommon.CreateCouponPassword(startNumber),
                    IsUse = (int)BoolType.False,
                    OrderId = 0

                });
            }
            #endregion
            #region 赠送积分
            //赠送积分--根据商品信息填写的赠送积分额度
            //int sendPoint = OrderBLL.ReadOrderSendPoint(order.Id);
            //根据订单付款金额全额返还积分
            int sendPoint = (int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
            if (sendPoint > 0 && order.IsActivity == (int)BoolType.False)
            {
                var accountRecord = new UserAccountRecordInfo
                {
                    RecordType = (int)AccountRecordType.Point,
                    Money = 0,
                    Point = sendPoint,
                    Date = DateTime.Now,
                    IP = ClientHelper.IP,
                    Note = ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber),
                    UserId = order.UserId,
                    UserName = order.UserName
                };
                UserAccountRecordBLL.Add(accountRecord);
            }
            #endregion
            #region 确认收货给分销商返佣
            //订单实际支付金额
            decimal paid_money =OrderBLL.ReadNoPayMoney(order);
            //购买人
            var user = UserBLL.Read(order.UserId);
            //购买者有推荐人 且 实际支付金额大于0才进行返佣
            if (user.Recommend_UserId > 0 && paid_money > 0)
            {
                RebateBLL.RebateToDistributor(user, paid_money,order.Id);
            }
            #endregion
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)OrderStatus.ReceiveShipping;
            ButtonEnd(order, Note.Text, OrderOperate.Received, startOrderStatus);
        }

        /// <summary>
        /// 换货确认按钮点击方法
        /// </summary>
        protected void ChangeButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)OrderStatus.Shipping;
            //更新商品库存数量
            ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Minus);
            ButtonEnd(order, Note.Text, OrderOperate.Change, startOrderStatus);
        }

        /// <summary>
        /// 退货确认按钮点击方法【不支持】
        /// </summary>
        protected void ReturnButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)OrderStatus.HasReturn;
            //更新商品库存数量
            ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
            ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Minus);
            ButtonEnd(order, Note.Text, OrderOperate.Return, startOrderStatus);
        }

        /// <summary>
        /// 撤销按钮点击方法
        /// </summary>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = OrderActionBLL.ReadLast(order.Id, order.OrderStatus).StartOrderStatus;
            //减去用户积分,积分为负数
            int sendPoint = 0;
            if (startOrderStatus == (int)OrderStatus.ReceiveShipping && order.IsActivity == (int)BoolType.False)
            {
                //sendPoint = -OrderBLL.ReadOrderSendPoint(order.Id);
                //根据订单付款金额全额返还积分
                 sendPoint = -(int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
            }
            if (startOrderStatus == (int)OrderStatus.HasShipping && order.OrderStatus == (int)OrderStatus.ReceiveShipping && order.IsActivity == (int)BoolType.False)//相当于收货确认
            {
                //sendPoint = OrderBLL.ReadOrderSendPoint(order.Id);
                //根据订单付款金额全额返还积分
                 sendPoint = -(int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
            }
            if (sendPoint != 0)
            {
                var accountRecord = new UserAccountRecordInfo
                {
                    RecordType = (int)AccountRecordType.Point,
                    Money = 0,
                    Point = sendPoint,
                    Date = DateTime.Now,
                    IP = ClientHelper.IP,
                    Note = ShopLanguage.ReadLanguage("OrderBack").Replace("$OrderNumber", order.OrderNumber),
                    UserId = order.UserId,
                    UserName = order.UserName
                };
                UserAccountRecordBLL.Add(accountRecord);
            }
          
            //更新商品库存数量
            switch (startOrderStatus)
            {
                case (int)OrderStatus.WaitPay:
                case (int)OrderStatus.WaitCheck:
                    if (order.OrderStatus == (int)OrderStatus.NoEffect)//相当于取消操作
                    {
                        ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
                    }
                    break;
                case (int)OrderStatus.NoEffect:
                    ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Plus);
                    break;
                case (int)OrderStatus.HasReturn:
                    ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Plus);
                    ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);
                    break;
                case (int)OrderStatus.Shipping:
                    if (order.OrderStatus == (int)OrderStatus.HasShipping)
                    {
                        ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);
                    }
                    break;
                case (int)OrderStatus.HasShipping:
                    if (order.OrderStatus == (int)OrderStatus.Shipping)
                    {
                        ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Minus);
                    }
                    if (order.OrderStatus == (int)OrderStatus.HasReturn)//相当于退货确认操作
                    {
                        ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
                        ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Minus);
                    }
                    break;
                default:
                    break;
            }
            //更新订单
            ButtonEnd(order, Note.Text, OrderOperate.Back, startOrderStatus);
        }

        /// <summary>
        /// 退款按钮点击方法【不支持】
        /// </summary>
        protected void RefundButton_Click(object sender, EventArgs e)
        {
            OrderInfo order = ButtoStart();
            //余额处理
            decimal money = order.Balance;
            int isCod = PayPlugins.ReadPayPlugins(order.PayKey).IsCod;
            if (order.OrderStatus == (int)OrderStatus.HasReturn && isCod == (int)BoolType.False)
            {
                money += OrderBLL.ReadNoPayMoney(order);
            }
            else if (order.OrderStatus == (int)OrderStatus.NoEffect && OrderActionBLL.ReadLast(order.Id, order.OrderStatus).StartOrderStatus == (int)OrderStatus.WaitCheck && isCod == (int)BoolType.False)
            {
                money += OrderBLL.ReadNoPayMoney(order);
            }
            if (money > 0)
            {
                var accountRecord = new UserAccountRecordInfo
                {
                    RecordType = (int)AccountRecordType.Money,
                    Money = money,
                    Point = 0,
                    Date = DateTime.Now,
                    IP = ClientHelper.IP,
                    Note = "退还订单：" + order.OrderNumber + "的金额",
                    UserId = order.UserId,
                    UserName = order.UserName
                };
                UserAccountRecordBLL.Add(accountRecord);
            }
            //更新订单
            int startOrderStatus = order.OrderStatus;
            order.OrderStatus = (int)order.OrderStatus;
            order.Balance = 0;
            order.CouponMoney = 0;
            order.IsRefund = (int)BoolType.True;
            ButtonEnd(order, Note.Text, OrderOperate.Refund, startOrderStatus);
        }
        /// <summary>
        /// 按纽提交开始
        /// </summary>
        protected OrderInfo ButtoStart()
        {
            CheckAdminPower("OperateOrder", PowerCheckType.Single);
            int orderId = RequestHelper.GetQueryString<int>("Id");
            OrderInfo order = OrderBLL.Read(orderId);
            return order;
        }

        /// <summary>
        /// 按纽提交结束
        /// </summary>
        protected void ButtonEnd(OrderInfo order, string note, OrderOperate orderOperate, int startOrderStatus)
        {
            OrderBLL.AdminUpdateOrderAddAction(order, note, (int)orderOperate, startOrderStatus);
          
            string result= OrderOperateSendEmail(order, orderOperate);
            if (orderOperate == OrderOperate.Pay)
            {              
                #region 拼团订单付款：团长付款--开团+增加参团记录；成员付款--增加参团记录
                if (order.IsActivity == (int)OrderKind.GroupBuy)
                {
                    var orderDetail = OrderDetailBLL.ReadList(order.Id).FirstOrDefault() ?? new OrderDetailInfo();
                    if (orderDetail.Id > 0)
                    {
                        var product = ProductBLL.Read(orderDetail.ProductId);
                        //团长付款
                        if (order.FavorableActivityId <= 0)
                        {
                            //开团
                            int groupId = GroupBuyBLL.Add(new GroupBuyInfo
                            {
                                Leader = order.UserId,
                                ProductId = product.Id,
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now.AddDays(ShopConfig.ReadConfigInfo().GroupBuyDays),
                                Quantity = product.GroupQuantity,
                                //团购订单支付成功之后计数加1
                                SignCount = 1
                            });
                            //订单绑定团购Id
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("[FavorableActivityId]", groupId);
                            OrderBLL.UpdatePart("[Order]", dict, order.Id);
                            //增加参团记录
                            GroupSignBLL.Add(new GroupSignInfo
                            {
                                GroupId = groupId,
                                UserId = order.UserId,
                                OrderId = order.Id,
                                SignTime = DateTime.Now
                            });
                        }
                        else//参团者付款
                        {
                            //增加参团记录
                            GroupSignBLL.Add(new GroupSignInfo
                            {
                                GroupId = order.FavorableActivityId,
                                UserId = order.UserId,
                                OrderId = order.Id,
                                SignTime = DateTime.Now
                            });
                            //开团表signcount加1
                            GroupBuyBLL.PlusSignCount(order.FavorableActivityId);
                        }
                    }
                }
                #endregion
                #region 自提订单 生成提货码
                //避免重复数据，一个订单对应一个提货码，提货码没有有效期，使用过后失效
                if (order.SelfPick==1 && PickUpCodeBLL.ReadByOrderId(order.Id).Id <= 0)
                {
                    PickUpCodeInfo pkCode = new PickUpCodeInfo();
                    pkCode.OrderId = order.Id;
                    pkCode.Status = 0;
                    pkCode.PickCode = PickUpCodeBLL.CreatePickUpCode();
                    int pickCodeId = PickUpCodeBLL.Add(pkCode);
                    //if (pickCodeId <= 0)
                    //{
                    //    return Json(new { flag = false, msg = "生成提货码失败" });
                    //}
                }
                #endregion
                //付款操作时触发sianalr
                ScriptHelper.Alert("订单" + ShopLanguage.ReadLanguage("OperateOK") + "。" + result, RequestHelper.RawUrl.IndexOf("?") >= 0 ? RequestHelper.RawUrl + "&paysuccess=1" : RequestHelper.RawUrl + "?paysuccess=1");
            }
            else
            {
                ScriptHelper.Alert("订单" + ShopLanguage.ReadLanguage("OperateOK") + "。" + result, RequestHelper.RawUrl);
            }
        }

        /// <summary>
        ///  读取订单的上一个，下一个
        /// </summary>
        protected string ReadPreNextOrderString(int orderId)
        {
            string result = string.Empty;
            int[] idArray = new int[2] { 0, 0 };
            foreach (OrderInfo order in OrderBLL.ReadPreNextOrder(orderId))
            {
                if (order.Id < orderId)
                {
                    idArray[0] = order.Id;
                }
                else
                {
                    idArray[1] = order.Id;
                }
            }
            if (idArray[0] > 0)
            {
                result += "<input type=\"button\" class=\"btn-button\" value=\"上一个\" onclick=\"window.location.href='OrderDetail.aspx?Id=" + idArray[0] + "'\"/>";
            }
            if (idArray[1] > 0)
            {
                result += " <input type=\"button\" class=\"btn-button\" value=\"下一个\" onclick=\"window.location.href='OrderDetail.aspx?Id=" + idArray[1] + "'\"/>";
            }
            return result;
        }

        /// <summary>
        /// 订单操作时候，发送通知(Email,Message)的操作
        /// </summary>
        protected string OrderOperateSendEmail(OrderInfo order, OrderOperate orderOperate)
        {
            string result = string.Empty;
            string key = string.Empty;
            int isSendNotice = (int)BoolType.False;
            int isSendEmail = (int)BoolType.False;
            int isSendMessage = (int)BoolType.False;
            switch (orderOperate)
            {
                case OrderOperate.Pay:
                    isSendNotice = ShopConfig.ReadConfigInfo().PayOrder;
                    isSendEmail = ShopConfig.ReadConfigInfo().PayOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().PayOrderMsg;
                    key = "PayOrder";
                    break;
                case OrderOperate.Cancle:
                    isSendNotice = ShopConfig.ReadConfigInfo().CancleOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().CancleOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().CancleOrderMsg;
                    key = "CancleOrder";
                    break;
                case OrderOperate.Check:
                    isSendNotice = ShopConfig.ReadConfigInfo().CheckOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().CheckOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().CheckOrderMsg;
                    key = "CheckOrder";
                    break;
                case OrderOperate.Send:
                    isSendNotice = ShopConfig.ReadConfigInfo().SendOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().SendOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().SendOrderMsg;
                    key = "SendOrder";
                    break;
                case OrderOperate.Received:
                    isSendNotice = ShopConfig.ReadConfigInfo().ReceivedOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().ReceivedOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().ReceivedOrderMsg;
                    key = "ReceivedOrder";
                    break;
                case OrderOperate.Change:
                    isSendNotice = ShopConfig.ReadConfigInfo().ChangeOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().ChangeOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().ChangeOrderMsg;
                    key = "ChangeOrder";
                    break;
                case OrderOperate.Return:
                    isSendNotice = ShopConfig.ReadConfigInfo().ReturnOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().ReturnOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().ReturnOrderMsg;
                    key = "ReturnOrder";
                    break;
                case OrderOperate.Back:
                    isSendNotice = ShopConfig.ReadConfigInfo().BackOrder;
                     isSendEmail = ShopConfig.ReadConfigInfo().BackOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().BackOrderMsg;
                    key = "BackOrder";
                    break;
                case OrderOperate.Refund:
                    isSendNotice = ShopConfig.ReadConfigInfo().RefundOrder;
                    isSendEmail = ShopConfig.ReadConfigInfo().RefundOrderEmail;
                    isSendMessage = ShopConfig.ReadConfigInfo().RefundOrderMsg;
                    key = "RefundOrder";
                    break;
                default:
                    break;
            }
            if (isSendNotice == (int)BoolType.True && key != string.Empty)
            {
                if (isSendEmail == (int)BoolType.True)
                {//发邮件
                    try
                    {
                        EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent(key);
                        EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                        emailSendRecord.Title = emailContent.EmailTitle;
                        emailSendRecord.Content = emailContent.EmailContent.Replace("$UserName$", order.UserName).Replace("$OrderNumber$", order.OrderNumber).Replace("$PayName$", order.PayName).Replace("$ShippingName$", ShippingBLL.Read(order.ShippingId).Name).Replace("$ShippingNumber$", order.ShippingNumber).Replace("$ShippingDate$", order.ShippingDate.ToString("yyyy-MM-dd"));
                        emailSendRecord.IsSystem = (int)BoolType.True;
                        emailSendRecord.EmailList = order.Email;
                        emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                        emailSendRecord.SendStatus = (int)SendStatus.No;
                        emailSendRecord.AddDate = RequestHelper.DateNow;
                        emailSendRecord.SendDate = RequestHelper.DateNow;
                        emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                        EmailSendRecordBLL.SendEmail(emailSendRecord);
                        result = "通知邮件已发送。";
                    }
                    catch {
                        result = "未发送通知邮件。";
                    }
                }
                if (isSendMessage == (int)BoolType.True)
                {//发短信
                    result += "未发送通知短信。";
               }
            }
            return result;
        }
        /// <summary>
        /// 获取会员用户名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected string getusername(int uid)
        {
            string username = "";
            if (uid <= 0)
            {
                return username;
            }
            UserInfo model = UserBLL.Read(uid);
            if (model.Id > 0)
                username = model.UserName;
            else
                username = "用户已删除";
            return username;
        }

    }
}