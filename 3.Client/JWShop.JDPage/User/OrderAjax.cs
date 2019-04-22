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

namespace JWShop.Page
{
    public class OrderAjax : UserAjaxBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "OrderOperate":
                    UserOrderOperate();
                    break;
                case "OrderSend":
                    OrderSend();
                    break;
                case "OrderDel":
                    UserOrderDel();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 用户删除订单--逻辑删除
        /// </summary>
        private void UserOrderDel()
        {
            string result = "";
            if (base.UserId <= 0)
            {
                result = "登录状态已过期，请重新登录";
            }
            else
            {
                int orderID = RequestHelper.GetQueryString<int>("OrderID");
                OrderInfo order = OrderBLL.Read(orderID, base.UserId);
                if (order.Id > 0 && order.IsDelete == (int)BoolType.False)
                {
                    order.IsDelete = (int)BoolType.True;
                    OrderBLL.Update(order);
                }
                else
                {
                    result = "订单不存在";
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 用户操作订单
        /// </summary>
        private void UserOrderOperate()
        {
            string result = string.Empty;
            if (base.UserId <= 0)
            {
                ResponseHelper.Write("登录状态已过期，请重新登录");
                ResponseHelper.End();
            }
            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            int orderOperate = RequestHelper.GetQueryString<int>("operate");
            OrderInfo order = OrderBLL.Read(orderID, base.UserId);
            //根据订单应支付金额计算赠送的积分额
            int orderPoint = (int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
            if (order.Id > 0 && ((orderOperate == (int)OrderOperate.Cancle && (order.OrderStatus == (int)OrderStatus.WaitCheck || order.OrderStatus == (int)OrderStatus.WaitPay))
                || (orderOperate == (int)OrderOperate.Received && order.OrderStatus == (int)OrderStatus.HasShipping)))
            {
                if (orderOperate == (int)OrderOperate.Cancle)
                {//取消订单
                    int startOrderStatus = order.OrderStatus;
                    order.OrderStatus = (int)OrderStatus.NoEffect;
                    //库存变化
                    ProductBLL.ChangeOrderCountByOrder(orderID, ChangeAction.Minus);
                    OrderBLL.UserUpdateOrderAddAction(order, "用户取消订单", orderOperate, startOrderStatus);

                    //返还积分--用户付款时兑换抵现的积分额
                    if (order.Point > 0)
                    {
                        var accountRecord = new UserAccountRecordInfo
                        {
                            RecordType = (int)AccountRecordType.Point,
                            Money = 0,
                            Point = order.Point,
                            Date = DateTime.Now,
                            IP = ClientHelper.IP,
                            Note = "取消订单：" + order.OrderNumber + "，退回用户积分",
                            UserId = base.UserId,
                            UserName = base.UserName
                        };
                        UserAccountRecordBLL.Add(accountRecord);
                    }
                    //发送取消订单通知
                    int isSendNotice = ShopConfig.ReadConfigInfo().CancleOrder;
                    int isSendEmail = ShopConfig.ReadConfigInfo().CancleOrderEmail;
                    int isSendMessage = ShopConfig.ReadConfigInfo().CancleOrderMsg;
                    string key = "CancleOrder";
                    SendOrderOperateMsg(isSendNotice, isSendEmail, isSendMessage, key, order);
                }
                else if (orderOperate == (int)OrderOperate.Received)
                {//确认收货
                    //赠送积分
                    //int sendPoint = OrderBLL.ReadOrderSendPoint(order.Id);
                    if (orderPoint > 0)
                    {
                        var accountRecord = new UserAccountRecordInfo
                        {
                            RecordType = (int)AccountRecordType.Point,
                            Money = 0,
                            Point = orderPoint,
                            Date = DateTime.Now,
                            IP = ClientHelper.IP,
                            Note = ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber),
                            UserId = base.UserId,
                            UserName = base.UserName
                        };
                        UserAccountRecordBLL.Add(accountRecord);
                    }
                    int startOrderStatus = order.OrderStatus;
                    order.OrderStatus = (int)OrderStatus.ReceiveShipping;
                    OrderBLL.UserUpdateOrderAddAction(order, "用户确认收货", orderOperate, startOrderStatus);

                    //发送订单收货通知
                    int isSendNotice = ShopConfig.ReadConfigInfo().CancleOrder;
                    int isSendEmail = ShopConfig.ReadConfigInfo().CancleOrderEmail;
                    int isSendMessage = ShopConfig.ReadConfigInfo().CancleOrderMsg;
                    string key = "ReceivedOrder";
                    SendOrderOperateMsg(isSendNotice, isSendEmail, isSendMessage, key, order);
                }
            }
            else
            {
                result = "订单不存在或状态错误";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        //供应商发货
        private void OrderSend()
        {
            string result = string.Empty;
            if (base.UserId <= 0)
            {
                ResponseHelper.Write("登录状态已过期，请重新登录");
                ResponseHelper.End();
            }

            DateTime shippingDate = RequestHelper.GetForm<DateTime>("ShippingDate");
            string shippingNumber = StringHelper.AddSafe(RequestHelper.GetForm<string>("ShippingNumber"));
            if (shippingDate < DateTime.Now.AddDays(-30))
            {
                ResponseHelper.Write("配送日期输入有误");
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(shippingNumber))
            {
                ResponseHelper.Write("请输入配送单号");
                ResponseHelper.End();
            }

            int orderId = RequestHelper.GetQueryString<int>("orderId");
            OrderInfo order = OrderBLL.ReadByShopId(orderId, base.UserId);
            if (order.Id > 0)
            {
                int startOrderStatus = order.OrderStatus;

                order.OrderStatus = (int)OrderStatus.HasShipping;
                order.ShippingNumber = shippingNumber;
                order.ShippingDate = shippingDate;
                //更新商品库存数量
                ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);
                OrderBLL.UserUpdateOrderAddAction(order, "供应商发货", (int)OrderOperate.Send, startOrderStatus);
            }
            else
            {
                result = "订单不存在或者您没有操作这个订单的权限";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        private void OperateEvent(OrderInfo order, int orderStatus, int orderID)
        {
            //if (orderStatus == (int)OrderStatus.WaitCheck || orderStatus == (int)OrderStatus.WaitPay)
            //{
            //    int startOrderStatus = order.OrderStatus;
            //    order.OrderStatus = (int)OrderStatus.NoEffect;
            //    //库存变化
            //    ProductBLL.ChangeProductOrderCountByOrder(orderID, ChangeAction.Minus);
            //    OrderBLL.UserUpdateOrderAddAction(order, "用户取消订单", (int)JWShop.Entity.OrderOperate.Cancle, startOrderStatus);
            //}
            //else
            //{
            //    //赠送积分
            //    int sendPoint = OrderBLL.ReadOrderSendPoint(order.ID);
            //    if (sendPoint > 0)
            //    {
            //        UserAccountRecordBLL.AddUserAccountRecord(0, sendPoint, ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber), order.UserId, order.UserName);
            //    }
            //    int startOrderStatus = order.OrderStatus;
            //    order.OrderStatus = (int)OrderStatus.ReceiveShipping;
            //    OrderBLL.UserUpdateOrderAddAction(order, "用户确认收货", (int)JWShop.Entity.OrderOperate.Received, startOrderStatus);
            //}
        }
        /// <summary>
        /// 操作订单发送通知
        /// </summary>
        /// <param name="isSendNotice">是否发送通知</param>
        /// <param name="isSendEmail">是否发送邮件</param>
        /// <param name="isSendMessage">是否发送短信</param>
        /// <param name="key">操作名称</param>
        /// <param name="order">操作的订单</param>
        protected void SendOrderOperateMsg(int isSendNotice, int isSendEmail, int isSendMessage, string key, OrderInfo order)
        {
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
        }
    }
}