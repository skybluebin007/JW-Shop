using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;

namespace JWShop.Page.Admin
{
  public  class OrderRefundDetail:AdminBase
    {
        protected OrderRefundInfo orderRefund = new OrderRefundInfo();
        protected OrderInfo order = new OrderInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("id");
            orderRefund = OrderRefundBLL.Read(id);
            order = OrderBLL.Read(orderRefund.OrderId);
            order.OrderDetailList = OrderDetailBLL.ReadList(orderRefund.OrderId);

            topNav = 0;
        }
        /// <summary>
        /// 退款操作：通过、拒绝、退款、取消
        /// </summary>
        protected override void PostBack()
        {          
            string param = RequestHelper.GetForm<string>("param").ToLower();
            int id = RequestHelper.GetForm<int>("id");
            string returnUrl = "/mobileadmin/orderrefunddetail.html?id=" + id;
            if (id <= 0)
            {
                ScriptHelper.AlertFront("请求参数错误", returnUrl);               
            }
            OrderRefundInfo orderRefund = OrderRefundBLL.Read(id);
            switch (param)
            {
                case "approve":                  
                        Approve((int)BoolType.True, orderRefund);                   
                    break;
                case "reject":                   
                        Approve((int)BoolType.False, orderRefund);                   
                    break;
                case "finish":
                    //更改状态为退款中...
                    if (orderRefund.Status == (int)OrderRefundStatus.Approve)
                    {
                        orderRefund.Status = (int)OrderRefundStatus.Returning;
                        orderRefund.Remark = "正在处理退款";

                        OrderRefundBLL.Update(orderRefund);

                        //退款操作记录
                        AddOrderRefundAction(orderRefund, (int)BoolType.True);
                    }

                    //退款到账户余额及各支付渠道
                    if (orderRefund.Status == (int)OrderRefundStatus.Returning)
                    {
                        //退款操作完成后跳转回本页面
                        JWRefund.RefundRedirectUrl = returnUrl;
                        JWRefund.RefundToAnyPay(orderRefund);
                        
                    }
                    else
                    {
                        ScriptHelper.AlertFront("无效的操作", returnUrl);
                    }
                    break;
                case "cancel":
                    if (orderRefund.Status == (int)OrderRefundStatus.Approve || orderRefund.Status == (int)OrderRefundStatus.Returning)
                    {
                        //更改状态为已取消...
                        orderRefund.Status = (int)OrderRefundStatus.Cancel;
                        orderRefund.Remark = "系统取消了退款";
                        OrderRefundBLL.Update(orderRefund);

                        //退款操作记录
                        AddOrderRefundAction(orderRefund, (int)BoolType.False);
                        ScriptHelper.AlertFront("操作成功", returnUrl);
                    }
                    else
                    {
                        ScriptHelper.AlertFront("无效的操作", returnUrl);
                    }
                    break;
            }
        }

        /// <summary>
        /// 审核订单退款申请
        /// </summary>
        /// <param name="approveStatus">1：通过；0：拒绝</param>
        private void Approve(int approveStatus, OrderRefundInfo orderRefund)
        {
            var submitOrderRefund = orderRefund;
            string returnUrl = "/mobileadmin/orderrefunddetail.html?id=" + orderRefund.Id;
            switch (submitOrderRefund.Status)
            {
                case (int)OrderRefundStatus.Submit:
                    CheckAdminPower("OrderRefundApprove", PowerCheckType.Single);
                    //如果是团购单，且拼团正在进行中，暂不能申请退款
                    var order = OrderBLL.Read(submitOrderRefund.OrderId);
                    if (order.IsActivity == (int)OrderKind.GroupBuy && order.FavorableActivityId > 0)
                    {
                        var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                        if (groupBuy.StartTime <= DateTime.Now && groupBuy.EndTime >= DateTime.Now && groupBuy.Quantity > groupBuy.SignCount)
                        {
                            ScriptHelper.AlertFront("拼团正在进行，暂不能退款", returnUrl);
                        }
                    }
                    if (approveStatus == (int)BoolType.True)
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Approve;
                        submitOrderRefund.Remark = "系统审核通过，等待处理退款： ";
                    }
                    else
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Reject;
                        submitOrderRefund.Remark = "系统审核不通过： ";
                    }
                    break;
                case (int)OrderRefundStatus.Returning:
                    ScriptHelper.AlertFront("正在处理退款，请不要重复退款", returnUrl);
                    break;
                case (int)OrderRefundStatus.HasReturn:
                    ScriptHelper.AlertFront("退款已完成，请不要重复退款", returnUrl);
                    break;
                case (int)OrderRefundStatus.Reject:
                    ScriptHelper.AlertFront("退款已被拒绝，请不要重复退款", returnUrl);
                    break;
                default:
                    ScriptHelper.AlertFront("无效的操作", returnUrl);
                    break;
            }

            OrderRefundBLL.Update(submitOrderRefund);

            //退款操作记录
            AddOrderRefundAction(submitOrderRefund, approveStatus);

            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), submitOrderRefund.Id);
            ScriptHelper.AlertFront("操作成功", returnUrl);

        }
        //增加退款操作记录
        private void AddOrderRefundAction(OrderRefundInfo entity, int status)
        {
            OrderRefundActionInfo submitOrderRefundAction = new OrderRefundActionInfo
            {
                OrderRefundId = entity.Id,
                Status = status,
                Remark = entity.Remark,
                Tm = DateTime.Now,
                UserType = 2,
                UserId = Cookies.Admin.GetAdminID(false),
                UserName = Cookies.Admin.GetAdminName(false)
            };

            OrderRefundActionBLL.Add(submitOrderRefundAction);
        }
    }
}
