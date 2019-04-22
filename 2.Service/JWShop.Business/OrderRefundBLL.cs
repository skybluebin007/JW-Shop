using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class OrderRefundBLL : BaseBLL
    {
        private static readonly IOrderRefund dal = FactoryHelper.Instance<IOrderRefund>(Global.DataProvider, "OrderRefundDAL");

        public static int Add(OrderRefundInfo entity)
        {
            int id = dal.Add(entity);

            //增加订单详细表中的退款商品数量
            if (id > 0)
            {
                //退商品
                if (entity.OrderDetailId > 0)
                {
                    OrderDetailBLL.ChangeRefundCount(entity.OrderDetailId, entity.RefundCount, ChangeAction.Plus);
                }
                //退订单
                else
                {
                    foreach (var orderDetail in OrderDetailBLL.ReadList(entity.OrderId))
                    {
                        OrderDetailBLL.ChangeRefundCount(orderDetail.Id, orderDetail.BuyCount - orderDetail.RefundCount, ChangeAction.Plus);
                    }
                }
            }

            return id; 
        }

        public static void Update(OrderRefundInfo entity)
        {
            dal.Update(entity);

            //服务工单审核不通过，或被取消。回滚在处理的退款的商品数量
            if (entity.Status == (int)OrderRefundStatus.Reject || entity.Status == (int)OrderRefundStatus.Cancel)
            {
                //退商品
                if (entity.OrderDetailId > 0)
                {
                    OrderDetailBLL.ChangeRefundCount(entity.OrderDetailId, entity.RefundCount, ChangeAction.Minus);
                }
                //退订单
                else
                {
                    var orderRefundList = ReadListValid(entity.OrderId);
                    foreach (var orderDetail in OrderDetailBLL.ReadList(entity.OrderId))
                    {
                        //逐一退商品
                        //如果前面有提交过该商品的退款服务单，则不能回滚这个商品的数量（orderDetail.RefundCount - refundCount）
                        int refundCount = orderRefundList.Where(k => k.OrderDetailId == orderDetail.Id).Sum(k => k.RefundCount);
                        OrderDetailBLL.ChangeRefundCount(orderDetail.Id, orderDetail.RefundCount - refundCount, ChangeAction.Minus);
                    }
                }
            }

            //退款完成，更新原订单(商品)的退款状态，库存回滚
            var order = OrderBLL.Read(entity.OrderId);
            if (entity.Status == (int)OrderRefundStatus.HasReturn)
            {
                //退单个商品
                if (entity.OrderDetailId > 0)
                {
                    //计算已退商品总数是否与订单商品总数相同，如相同则更改订单状态
                    var orderRefundList = ReadList(entity.OrderId);
                    orderRefundList = orderRefundList.Where(k => k.Status == (int)OrderRefundStatus.HasReturn).ToList();
                    var orderDetailList = OrderDetailBLL.ReadList(entity.OrderId);

                    if (orderRefundList.Sum(k => k.RefundCount) == orderDetailList.Sum(k => k.BuyCount))
                    {
                        UpdateOrderRefundStatus(order);
                    }

                    //库存回滚
                    var orderDetail = OrderDetailBLL.Read(entity.OrderDetailId);
                    ProductBLL.ChangeOrderCount(orderDetail.ProductId, -orderDetail.RefundCount);
                    if (!string.IsNullOrEmpty(orderDetail.StandardValueList))
                    {
                        ProductTypeStandardRecordBLL.ChangeOrderCount(orderDetail.ProductId, orderDetail.StandardValueList, orderDetail.RefundCount, ChangeAction.Minus);
                    }

                    if (order.OrderStatus >= (int)OrderStatus.HasShipping)
                    {
                        ProductBLL.ChangeSendCount(orderDetail.ProductId, -orderDetail.RefundCount);
                        if (!string.IsNullOrEmpty(orderDetail.StandardValueList))
                        {
                            ProductTypeStandardRecordBLL.ChangeSendCount(orderDetail.ProductId, orderDetail.StandardValueList, orderDetail.RefundCount, ChangeAction.Minus);
                        }
                    }
                }
                //退订单
                else
                {
                    UpdateOrderRefundStatus(order);

                    //库存回滚
                    var orderRefundList = ReadListValid(entity.OrderId);
                    foreach (var orderDetail in OrderDetailBLL.ReadList(entity.OrderId))
                    {
                        //逐一处理
                        //如果前面有提交过该商品的退款服务单，则不能回滚这个商品的数量
                        int refundCount = orderRefundList.Where(k => k.OrderDetailId == orderDetail.Id).Sum(k => k.RefundCount);
                        int buyCount = orderDetail.BuyCount - refundCount;
                        if (buyCount > 0)
                        {
                            ProductBLL.ChangeOrderCount(orderDetail.ProductId, -buyCount);
                            if (!string.IsNullOrEmpty(orderDetail.StandardValueList))
                            {
                                ProductTypeStandardRecordBLL.ChangeOrderCount(orderDetail.ProductId, orderDetail.StandardValueList, buyCount, ChangeAction.Minus);
                            }

                            if (order.OrderStatus >= (int)OrderStatus.HasShipping)
                            {
                                ProductBLL.ChangeSendCount(orderDetail.ProductId, -buyCount);
                                if (!string.IsNullOrEmpty(orderDetail.StandardValueList))
                                {
                                    ProductTypeStandardRecordBLL.ChangeSendCount(orderDetail.ProductId, orderDetail.StandardValueList, buyCount, ChangeAction.Minus);
                                }
                            }
                        }
                    }
                }
            }
        }

        //更改订单的退款状态
        private static void UpdateOrderRefundStatus(OrderInfo order)
        {
            if (order.IsRefund == (int)BoolType.False)
            {
                order.IsRefund = (int)BoolType.True;
                int status = order.OrderStatus == (int)OrderStatus.WaitCheck || order.OrderStatus == (int)OrderStatus.Shipping ? (int)OrderStatus.NoEffect : (int)OrderStatus.HasReturn;
                int startOrderStatus = order.OrderStatus;
                order.OrderStatus = status;
                OrderBLL.AdminUpdateOrderAddAction(
                    order,
                    "退款完成",
                    status == (int)OrderStatus.WaitCheck || status == (int)OrderStatus.Shipping ? (int)OrderOperate.Refund : (int)OrderOperate.Return,
                    startOrderStatus);
            }
        }

        //更新退款批次号
        public static void UpdateBatchNo(int id, string batchNo)
        {
            dal.UpdateBatchNo(id, batchNo);
        }

        public static void Comment(int id, string content)
        {
            Comment(id, content);
        }

        public static OrderRefundInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static OrderRefundInfo ReadByBatchNo(string batchNo)
        {
            return dal.ReadByBatchNo(batchNo);
        }

        public static List<OrderRefundInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<OrderRefundInfo> ReadList(int orderId)
        {
            return dal.ReadList(orderId);
        }

        public static List<OrderRefundInfo> ReadList(int[] orderIds)
        {
            return dal.ReadList(orderIds);
        }

        public static List<OrderRefundInfo> ReadListByOwnerId(int ownerId)
        {
            return dal.ReadListByOwnerId(ownerId);
        }

        /// <summary>
        /// 获取正在和已完成退款的服务单（排除被拒绝、取消的服务单）
        /// </summary>
        public static List<OrderRefundInfo> ReadListValid(int orderId)
        {
            var orderRefundList = ReadList(orderId);
            orderRefundList = orderRefundList.Where(k => k.Status != (int)OrderRefundStatus.Reject && k.Status != (int)OrderRefundStatus.Cancel).ToList();

            return orderRefundList;
        }

        public static List<OrderRefundInfo> SearchList(int currentPage, int pageSize, OrderRefundSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }

        /// <summary>
        /// 能够退款的有效状态
        /// </summary>
        /// <param name="status">退款服务单的状态</param>
        public static bool CanToReturn(int status)
        {
            return status >= (int)OrderRefundStatus.Submit && status < (int)OrderRefundStatus.HasReturn;
        }
        /// <summary>
        /// 已退款的状态
        /// </summary>
        /// <param name="status">退款服务单的状态</param>
        public static bool HasReturn(int status)
        {
            return status == (int)OrderRefundStatus.HasReturn;
        }
        /// <summary>
        /// 退款是否正在处理
        /// 拒绝、取消、完成 表示 退款已结束
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool RefundGoing(int status)
        {
            return status != (int)OrderRefundStatus.Reject && status != (int)OrderRefundStatus.Cancel && status != (int)OrderRefundStatus.HasReturn;
        }
    }




    /// <summary>
    /// 统一退款业务封装
    /// </summary>
    public class JWRefund
    {
        /// <summary>
        /// 线上线下退款切换。默认线下退款
        /// </summary>
        private static readonly bool RefundOnline = true;
        /// <summary>
        /// 已发货状态下，是否退还运费。默认不退还
        /// </summary>
        private static readonly bool RefundShippingMoneyWhenHasShipping = false;
        /// <summary>
        /// 退款操作完成后跳转的页面(默认Pc管理后台)：如果是PC管理后台操作退款，不用赋值，如果是商户手机管理后台操作退款，则要赋值
        /// </summary>
        public static string RefundRedirectUrl { get; set; }


        /// <summary>
        /// 订单或商品能否被退款
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="orderDetail">退商品时可用，如果是退订单可不传参</param>
        /// <param name="refundCount">退商品时可用，退款商品的数量</param>
        /// <param name="needRefundMoney">需要退款的金额</param>
        /// <returns></returns>
        public static JWRefundMsg CanRefund(OrderInfo order, OrderDetailInfo orderDetail = null, int refundCount = 0, decimal needRefundMoney = 0)
        {
            JWRefundMsg refundMsg = new JWRefundMsg { CanRefund = false };

            if (order.Id < 1)
            {
                refundMsg.ErrorCode = JWRefundErrorCode.ORDER_INVALID;
                return refundMsg;
            }
            if (orderDetail != null)
            {
                //传递过来的退款商品不为空时，如果Id不大于0，则是无效的
                //同时，如果退款商品所在订单的Id和传递过来的订单Id参数不一致，也是无效的
                if (orderDetail.Id < 1 || orderDetail.OrderId != order.Id)
                {
                    refundMsg.ErrorCode = JWRefundErrorCode.ORDER_DETAIL_INVALID;
                    return refundMsg;
                }
            }

            //只有订单状态为待审核、待发货和已发货的可以退款
            if (order.OrderStatus != (int)OrderStatus.WaitCheck && order.OrderStatus != (int)OrderStatus.Shipping && order.OrderStatus != (int)OrderStatus.HasShipping)
            {
                refundMsg.ErrorCode = JWRefundErrorCode.ORDER_STATUS_INVALID;
                return refundMsg;
            }

            //计算本次退款的可退数量
            refundMsg = CanRefundCount(order, orderDetail, refundCount);
            if (!refundMsg.CanRefund) return refundMsg;
            int canRefundCount = refundMsg.CanRefundCount;

            //计算本次退款的可退金额
            refundMsg = CanRefundMoney(order, orderDetail, refundCount, needRefundMoney);
            if (!refundMsg.CanRefund) return refundMsg;
            refundMsg.CanRefundCount = canRefundCount;

            return refundMsg;
        }

        /// <summary>
        /// 计算本次退款的可退数量
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="orderDetail">退商品时可用，如果是退订单可不传参</param>
        /// <param name="refundCount">退商品时可用，退款商品的数量</param>
        /// <returns></returns>
        private static JWRefundMsg CanRefundCount(OrderInfo order, OrderDetailInfo orderDetail = null, int refundCount = 0)
        {
            JWRefundMsg refundMsg = new JWRefundMsg { CanRefund = false };

            //退商品
            if (orderDetail != null && orderDetail.Id > 0)
            {
                //退款数量必须大于0
                if (refundCount < 1)
                {
                    refundMsg.ErrorCode = JWRefundErrorCode.REFUND_COUNT_MUST_GT0_ERROR;
                    return refundMsg;
                }

                refundMsg.CanRefundCount = orderDetail.BuyCount - orderDetail.RefundCount;
            }
            //退订单
            else
            {
                var orderDetailList = OrderDetailBLL.ReadList(order.Id);
                refundMsg.CanRefundCount = orderDetailList.Sum(k => k.BuyCount - k.RefundCount);
            }

            //是否超过最大可退数量，并且保证还有商品可以退款
            if (refundMsg.CanRefundCount < 1 || refundCount > refundMsg.CanRefundCount)
            {
                refundMsg.ErrorCode = JWRefundErrorCode.CAN_REFUND_COUNT_ERROR;
                return refundMsg;
            }

            refundMsg.CanRefund = true;
            return refundMsg;
        }

        /// <summary>
        /// 计算本次退款的可退金额
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="orderDetail">退商品时可用，如果是退订单可不传参</param>
        /// <param name="refundCount">退商品时可用，退款商品的数量</param>
        /// <param name="needRefundMoney">需要退款的金额</param>
        /// <returns></returns>
        private static JWRefundMsg CanRefundMoney(OrderInfo order, OrderDetailInfo orderDetail = null, int refundCount = 0, decimal needRefundMoney = 0)
        {
            var refundMsg = new JWRefundMsg { CanRefund = false };

            //订单可退金额
            decimal orderRefundMoney = 0;
            //如果订单状态为待审核或配货中，则可退运费（因为商品还未发货）
            if (order.OrderStatus == (int)OrderStatus.WaitCheck || order.OrderStatus == (int)OrderStatus.Shipping)
            {
                orderRefundMoney = order.ProductMoney + order.ShippingMoney + order.OtherMoney - order.PointMoney-order.CouponMoney-order.FavorableMoney;
            }
            //如果订单已发货（是否退还运费视设置而定）
            if (order.OrderStatus == (int)OrderStatus.HasShipping)
            {
                orderRefundMoney = order.ProductMoney + order.OtherMoney - order.PointMoney - order.CouponMoney - order.FavorableMoney;
                if (RefundShippingMoneyWhenHasShipping) orderRefundMoney += order.ShippingMoney;
            }

            //已退款或正在处理中的服务单金额
            var orderRefundList = OrderRefundBLL.ReadListValid(order.Id);
            decimal hasRefundMoney = orderRefundList.Sum(k => k.RefundBalance + k.RefundMoney);

            //这里的计算结果就是订单可退的金额
            decimal canRefundMoney = orderRefundMoney - hasRefundMoney;
            refundMsg.CanRefundMoney = canRefundMoney;

            //如果是退单个商品，考虑到订单总价格可能被改变，商品可退金额不能超过订单可退金额
            if (orderDetail != null && orderDetail.Id > 0)
            {
                if (refundCount < 1)
                {
                    refundMsg.ErrorCode = JWRefundErrorCode.REFUND_COUNT_MUST_GT0_ERROR;
                    return refundMsg;
                }

                refundMsg.CanRefundMoney = orderDetail.ProductPrice * refundCount;
                if (refundMsg.CanRefundMoney > canRefundMoney) refundMsg.CanRefundMoney = canRefundMoney;
            }

            //是否超过最大可退金额，并且保证还有金额可以退款
            if (refundMsg.CanRefundMoney <= 0 || needRefundMoney > refundMsg.CanRefundMoney)
            {
                refundMsg.ErrorCode = JWRefundErrorCode.CAN_REFUND_MONEY_ERROR;
                return refundMsg;
            }

            refundMsg.CanRefund = true;
            return refundMsg;
        }

        /// <summary>
        /// 对最终需要提交的退款服务单，做进一步的验证
        /// </summary>
        /// <param name="orderRefund"></param>
        /// <param name="needRefundMoney">本次服务单需退款的金额</param>
        /// <returns></returns>
        public static JWRefundMsg VerifySubmitOrderRefund(OrderRefundInfo orderRefund, decimal needRefundMoney)
        {
            var refundMsg = new JWRefundMsg { CanRefund = false };

            //退款金额必须大于0
            if (needRefundMoney <= 0)
            {
                refundMsg.ErrorCode = JWRefundErrorCode.REFUND_MONEY_MUST_GT0_ERROR;
                return refundMsg;
            }

            var order = OrderBLL.Read(orderRefund.OrderId);

            OrderDetailInfo orderDetail = null;
            if (orderRefund.OrderDetailId > 0)
            {
                orderDetail = OrderDetailBLL.Read(orderRefund.OrderDetailId);
            }

            //验证该退款单能否被退款
            refundMsg = CanRefund(order, orderDetail, orderRefund.RefundCount, needRefundMoney);
            if (!refundMsg.CanRefund) return refundMsg;

            orderRefund.OrderNumber = order.OrderNumber;
            orderRefund.RefundPayKey = order.PayKey;
            orderRefund.RefundPayName = order.PayName;
            orderRefund.OrderDetailId = orderDetail == null ? 0 : orderDetail.Id;
            orderRefund.OwnerId = order.UserId;

            /*-----退款金额分配（余额与第三方支付金额） start----------------------------------------------------*/

            //已退余额金额
            var orderRefundList = OrderRefundBLL.ReadListValid(orderRefund.OrderId);
            decimal hasRefundBalance = orderRefundList.Sum(k => k.RefundBalance);

            //退商品
            if (orderRefund.OrderDetailId > 0)
            {
                //如果需要退款的金额不大于剩余未退的余额金额，则全部退到余额账户
                if (needRefundMoney <= (order.Balance - hasRefundBalance))
                {
                    orderRefund.RefundBalance = needRefundMoney;
                    orderRefund.RefundMoney = 0;
                }
                else
                {
                    orderRefund.RefundBalance = (order.Balance - hasRefundBalance);
                    orderRefund.RefundMoney = needRefundMoney - (order.Balance - hasRefundBalance);
                }
            }
            //退订单
            else
            {
                orderRefund.RefundBalance = (order.Balance - hasRefundBalance);
                orderRefund.RefundMoney = needRefundMoney - (order.Balance - hasRefundBalance);
            }
            /*-----退款金额分配（余额与第三方支付金额） end------------------------------------------------------*/

            refundMsg.CanRefund = true;
            return refundMsg;
        }

        /// <summary>
        /// 退款到账户余额及各支付渠道
        /// </summary>
        /// <param name="orderRefund"></param>
        public static void RefundToAnyPay(OrderRefundInfo orderRefund)
        {
            if (orderRefund.Status != (int)OrderRefundStatus.Returning) return;

            if (RefundOnline)
            {
                if (PayPlugins.ReadPayPlugins(orderRefund.RefundPayKey).IsOnline == 1&&(OrderBLL.Read(orderRefund.OrderId).AliPayTradeNo.Trim()!=string.Empty|| OrderBLL.Read(orderRefund.OrderId).WxPayTradeNo.Trim() != string.Empty))//只有在线付款才走线上流程，并且存在支付交易号（支付宝微信任意一个）
                {
                    //退款到各支付渠道
                    //如需退余额，将在第三方支付退款成功后操作
                    //这样做的好处在于保障了余额能够被准确退还。不会出现退了余额，但支付宝或微信因为人为或意外的原因没退成功的情况。
                    if (orderRefund.RefundMoney > 0)
                    {
                        //HttpContext.Current.Response.Redirect(string.Format("/Plugins/Pay/{0}/Refund.aspx?order_refund_id={1}", orderRefund.RefundPayKey, orderRefund.Id));
                        HttpContext.Current.Response.Redirect(string.Format("/Plugins/Pay/{0}/Refund.aspx?order_refund_id={1}&returnurl={2}", orderRefund.RefundPayKey, orderRefund.Id,RefundRedirectUrl));
                    }
                    else
                    {
                        //只需退款到余额
                        //在这里写自己的逻辑
                        if (orderRefund.RefundBalance > 0)
                        {
                            //HttpContext.Current.Response.Redirect(string.Format("/Admin/OrderRefundToBalance.aspx?order_refund_id={0}", orderRefund.Id));
                            HttpContext.Current.Response.Redirect(string.Format("/Admin/OrderRefundToBalance.aspx?order_refund_id={0}&returnurl={1}", orderRefund.Id, RefundRedirectUrl));
                        }
                    }
                }
                else//线下退款则直接更改状态，所以线下退款只能管理员自己审核。
                {
                    orderRefund.Remark = "退款完成";
                    orderRefund.Status = (int)OrderRefundStatus.HasReturn;
                    orderRefund.TmRefund = DateTime.Now;
                    OrderRefundBLL.Update(orderRefund);

                    OrderRefundActionBLL.Add(new OrderRefundActionInfo
                    {
                        OrderRefundId = orderRefund.Id,
                        Status = 1,
                        Tm = DateTime.Now,
                        UserType = 2,
                        UserId = Cookies.Admin.GetAdminID(false),
                        UserName = Cookies.Admin.GetAdminName(false),
                        Remark = orderRefund.Remark
                    });
                    #region 发送订单退款成功通知
                    int isSendNotice = ShopConfig.ReadConfigInfo().RefundOrder;
                    int isSendEmail = ShopConfig.ReadConfigInfo().RefundOrderEmail;
                    int isSendMessage = ShopConfig.ReadConfigInfo().RefundOrderMsg;
                    string key = "RefundOrder";
                    OrderInfo order = OrderBLL.Read(orderRefund.OrderId);
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
                    #endregion
                    if (string.IsNullOrEmpty(RefundRedirectUrl))
                    {
                        HttpContext.Current.Response.Redirect(string.Format("/Admin/OrderRefundAdd.aspx?id={0}", orderRefund.Id));
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(RefundRedirectUrl);
                    }
                }
            }
            else
            {
                //处理订单、商品的退款状态，线下退款
                //在这里写自己的逻辑
                HttpContext.Current.Response.Redirect(string.Format("/Admin/OrderRefundToBalance.aspx?order_refund_id={0}&returnurl={1}", orderRefund.Id,RefundRedirectUrl));
            }
        }

    }

    public class JWRefundMsg
    {
        public bool CanRefund { get; set; }
        public JWRefundErrorCode ErrorCode { get; set; }
        public string ErrorCodeMsg { get { return EnumHelper.ReadEnumChineseName<JWRefundErrorCode>((int)this.ErrorCode); } }
        public int CanRefundCount { get; set; }
        public decimal CanRefundMoney { get; set; }
    }

    public enum JWRefundErrorCode
    {
        /// <summary>
        /// 无效的订单
        /// </summary>
        [Enum("抱歉，该订单无效，不能进行退款")]
        ORDER_INVALID = 100,
        /// <summary>
        /// 无效的订单状态
        /// </summary>
        [Enum("抱歉，当前不能进行退款，待审核、待发货和已发货状态的订单才可以退款")]
        ORDER_STATUS_INVALID = 110,
        /// <summary>
        /// 无效的商品
        /// </summary>
        [Enum("抱歉，该商品无效，不能进行退款")]
        ORDER_DETAIL_INVALID = 101,
        /// <summary>
        /// 退款数量必须大于0
        /// </summary>
        [Enum("抱歉，退款数量必须大于0，请返回修改")]
        REFUND_COUNT_MUST_GT0_ERROR = 200,
        /// <summary>
        /// 退款金额必须大于0
        /// </summary>
        [Enum("抱歉，退款金额必须大于0，请返回修改")]
        REFUND_MONEY_MUST_GT0_ERROR = 201,
        /// <summary>
        /// 请求退款的数量已超过最大可退款数量
        /// </summary>
        [Enum("抱歉，请求退款的数量已超过最大可退款数量，请返回修改或联系网站客服")]
        CAN_REFUND_COUNT_ERROR = 210,
        /// <summary>
        /// 请求退款的金额已超过最大可退款金额
        /// </summary>
        [Enum("抱歉，请求退款的金额已超过最大可退款金额，请返回修改或联系网站客服")]
        CAN_REFUND_MONEY_ERROR = 211,
    }
}