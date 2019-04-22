using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Pay.WxPay
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify : Notify
    {
        //退款成功标识
        public bool notifyResult = false;
        public ResultNotify(System.Web.UI.Page page)
            : base(page)
        {
        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();


            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                //page.Response.Write(res.ToXml());
                //page.Response.End();
            }
            else
            {
                string transaction_id = notifyData.GetValue("transaction_id").ToString();

                //查询订单，判断订单真实性
                if (!QueryOrder(transaction_id))
                ////沙箱测试只能用out_trade_no
                //if (notifyData.IsSet("out_trade_no") && !QueryOrderByout_trade_no(notifyData.GetValue("out_trade_no").ToString()))
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                    //page.Response.Write(res.ToXml());
                    //page.Response.End();
                }
                //查询订单成功
                else
                {
                    Log.Debug(this.GetType().ToString(), "订单查询成功 ");
                    /************在这里加入商户自己的逻辑***********************************************************/

                    //attach：以逗号分开的订单Id字符串，与支付时提交的数据一致
                    if (notifyData.IsSet("attach"))
                    {
                        string attach = notifyData.GetValue("attach").ToString();

                        string[] orderIds = attach.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string orderId in orderIds)
                        {
                            var order = OrderBLL.Read(int.Parse(orderId));
                            if (order.Id > 0)
                            {
                                #region 正常流程 待付款--》待审核
                                if (order.OrderStatus == (int)OrderStatus.WaitPay)
                                {
                                    order.PayKey = "WxPay";
                                    order.PayName = "微信支付";

                                    order.WxPayTradeNo = transaction_id;
                                    //沙箱测试用out_trade_no
                                    //order.WxPayTradeNo = notifyData.GetValue("out_trade_no").ToString();

                                    order.OrderStatus = (int)OrderStatus.WaitCheck;
                                    order.PayDate = RequestHelper.DateNow;
                                    OrderBLL.Update(order);                                 
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
                                            else//参团者付款--放到参团者checkout
                                            {
                                                ////增加参团记录
                                                //GroupSignBLL.Add(new GroupSignInfo
                                                //{
                                                //    GroupId = order.FavorableActivityId,
                                                //    UserId = order.UserId,
                                                //    OrderId = order.Id,
                                                //    SignTime = DateTime.Now
                                                //});
                                                ////开团表signcount加1
                                                //GroupBuyBLL.PlusSignCount(order.FavorableActivityId);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 自提订单 生成提货码
                                    //避免重复数据，一个订单对应一个提货码，提货码没有有效期，使用过后失效
                                    if (order.SelfPick == 1 && PickUpCodeBLL.ReadByOrderId(order.Id).Id <= 0)
                                    {
                                        PickUpCodeInfo pkCode = new PickUpCodeInfo();
                                        pkCode.OrderId = order.Id;
                                        pkCode.Status = 0;
                                        pkCode.PickCode = PickUpCodeBLL.CreatePickUpCode();
                                        int pickCodeId = PickUpCodeBLL.Add(pkCode);
                                    }
                                    #endregion
                                    #region 砍价订单
                                    if (order.IsActivity == (int)OrderKind.Bargain)
                                    {
                                        BargainOrderBLL.HandleBargainOrderPay(order.FavorableActivityId);
                                    }
                                    #endregion
                                    //发送订单支付成功通知
                                    SendPayOrderMsg(order);
                                    //增加操作记录
                                    OrderActionInfo orderAction = new OrderActionInfo();
                                    orderAction.OrderId = order.Id;
                                    orderAction.OrderOperate = (int)OrderOperate.Pay;
                                    orderAction.StartOrderStatus = (int)OrderStatus.WaitPay;
                                    orderAction.EndOrderStatus = (int)OrderStatus.WaitCheck;
                                    orderAction.Note = "客户微信在线支付";
                                    orderAction.IP = ClientHelper.IP;
                                    orderAction.Date = RequestHelper.DateNow;
                                    orderAction.AdminID = 0;
                                    orderAction.AdminName = string.Empty;
                                    OrderActionBLL.Add(orderAction);
                                }
                                #endregion
                                #region 非正常流程 待付款超时已失效(未退款)--》待审核
                                if (order.OrderStatus == (int)OrderStatus.NoEffect && order.IsRefund == 0)
                                {
                                    order.PayKey = "WxPay";
                                    order.PayName = "微信支付";
                                    order.WxPayTradeNo = transaction_id;
                                    order.OrderStatus = (int)OrderStatus.WaitCheck;
                                    order.PayDate = RequestHelper.DateNow;
                                    OrderBLL.Update(order);                                   
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
                                            else//参团者付款--放到参团者checkout
                                            {
                                                ////增加参团记录
                                                //GroupSignBLL.Add(new GroupSignInfo
                                                //{
                                                //    GroupId = order.FavorableActivityId,
                                                //    UserId = order.UserId,
                                                //    OrderId = order.Id,
                                                //    SignTime = DateTime.Now
                                                //});
                                                ////开团表signcount加1
                                                //GroupBuyBLL.PlusSignCount(order.FavorableActivityId);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 自提订单 生成提货码
                                    //避免重复数据，一个订单对应一个提货码，提货码没有有效期，使用过后失效
                                    if (order.SelfPick == 1 && PickUpCodeBLL.ReadByOrderId(order.Id).Id <= 0)
                                    {
                                        PickUpCodeInfo pkCode = new PickUpCodeInfo();
                                        pkCode.OrderId = order.Id;
                                        pkCode.Status = 0;
                                        pkCode.PickCode = PickUpCodeBLL.CreatePickUpCode();
                                        int pickCodeId = PickUpCodeBLL.Add(pkCode);
                                    }
                                    #endregion
                                    #region 砍价订单
                                    if (order.IsActivity == (int)OrderKind.Bargain)
                                    {
                                        BargainOrderBLL.HandleBargainOrderPay(order.FavorableActivityId);
                                    }
                                    #endregion
                                    #region 扣除支付积分
                                    if (order.Point > 0)
                                    {
                                        //减少积分
                                        UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
                                        uarInfo.RecordType = (int)AccountRecordType.Point;
                                        uarInfo.UserId = order.UserId;
                                        uarInfo.UserName = order.UserName;
                                        uarInfo.Note = "支付订单：" + order.OrderNumber;
                                        uarInfo.Point = -order.Point;
                                        uarInfo.Money = 0;
                                        uarInfo.Date = DateTime.Now;
                                        uarInfo.IP = ClientHelper.IP;
                                        UserAccountRecordBLL.Add(uarInfo);
                                    }
                                    #endregion
                                    #region 减少商品库存
                                    ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Plus);
                                    #endregion
                                    //发送订单支付成功通知
                                    SendPayOrderMsg(order);
                                    //增加操作记录
                                    OrderActionInfo orderAction = new OrderActionInfo();
                                    orderAction.OrderId = order.Id;
                                    orderAction.OrderOperate = (int)OrderOperate.Pay;
                                    orderAction.StartOrderStatus = (int)OrderStatus.WaitPay;
                                    orderAction.EndOrderStatus = (int)OrderStatus.WaitCheck;
                                    orderAction.Note = "客户微信在线支付";
                                    orderAction.IP = ClientHelper.IP;
                                    orderAction.Date = RequestHelper.DateNow;
                                    orderAction.AdminID = 0;
                                    orderAction.AdminName = string.Empty;
                                    OrderActionBLL.Add(orderAction);
                                }
                                #endregion
                                //记录微信支付交易单号
                                if (order.OrderStatus == (int)OrderStatus.WaitCheck && string.IsNullOrEmpty(order.WxPayTradeNo))
                                {
                                    order.WxPayTradeNo = transaction_id;
                                    OrderBLL.Update(order);                                   
                                }
                                notifyResult = true;
                            }
                        }
                    }

                    /************在这里加入商户自己的逻辑***********************************************************/

                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                    //page.Response.Write(res.ToXml());
                    //page.Response.End();
                }
            }
        }

        //查询订单--根据transaction_id
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            Log.Error("OrderQuery", res.ToXml());
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //查询订单--out_trade_no
        private bool QueryOrderByout_trade_no(string out_trade_no)
        {
            WxPayData req = new WxPayData();
            req.SetValue("out_trade_no", out_trade_no);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 发送订单支付成功通知
        /// </summary>
        protected void SendPayOrderMsg(OrderInfo order)
        {
            int isSendNotice = ShopConfig.ReadConfigInfo().PayOrder;
            int isSendEmail = ShopConfig.ReadConfigInfo().PayOrderEmail;
            int isSendMessage = ShopConfig.ReadConfigInfo().PayOrderMsg;
            string key = "PayOrder";
            if (isSendNotice == (int)BoolType.True)
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
                        //result = "通知邮件已发送。";
                    }
                    catch
                    {
                        //result = "未发送通知邮件。";
                    }
                }
                if (isSendMessage == (int)BoolType.True)
                {//发短信
                    //result += "未发送通知短信。";
                }
            }
        }
    }
}