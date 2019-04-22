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

namespace JWShop.Web.Admin
{
    public partial class OrderRefundAdd : JWShop.Page.AdminBasePage
    {
        protected OrderRefundInfo orderRefund = new OrderRefundInfo();
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected List<OrderRefundActionInfo> orderRefundActionList = new List<OrderRefundActionInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("OrderRefundApprove", PowerCheckType.Single);

                int orderRefundId = RequestHelper.GetQueryString<int>("Id");
                orderRefund = OrderRefundBLL.Read(orderRefundId);
                if (orderRefund.Id > 0)
                {
                    order = OrderBLL.Read(orderRefund.OrderId);
                    if (orderRefund.OrderDetailId > 0)
                    {
                        orderDetailList.Add(OrderDetailBLL.Read(orderRefund.OrderDetailId));
                    }
                    else
                    {
                        orderDetailList = OrderDetailBLL.ReadList(orderRefund.OrderId);
                    }

                    orderRefundActionList = OrderRefundActionBLL.ReadList(orderRefund.Id);
                }
            }
        }

        protected void ApproveButton_Click(object sender, EventArgs e)
        {
            this.Approve((int)BoolType.True);
        }

        protected void RejectButton_Click(object sender, EventArgs e)
        {
            this.Approve((int)BoolType.False);
        }

        private void Approve(int approveStatus)
        {
            int orderRefundId = RequestHelper.GetQueryString<int>("Id");
            var submitOrderRefund = OrderRefundBLL.Read(orderRefundId);
            string remark = Remark.Text.Trim();
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
                            ScriptHelper.Alert("拼团正在进行，暂不能退款", RequestHelper.RawUrl);
                        }
                    }
                    if (approveStatus == (int)BoolType.True)
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Approve;
                        submitOrderRefund.Remark = "系统审核通过，等待处理退款： " + remark;
                    }
                    else
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Reject;
                        submitOrderRefund.Remark = "系统审核不通过： " + remark;
                    }
                    break;
                case (int)OrderRefundStatus.Returning:
                    ScriptHelper.Alert("正在处理退款，请不要重复退款", RequestHelper.RawUrl);
                    break;
                case (int)OrderRefundStatus.HasReturn:
                    ScriptHelper.Alert("退款已完成，请不要重复退款", RequestHelper.RawUrl);
                    break;
                case (int)OrderRefundStatus.Reject:
                    ScriptHelper.Alert("退款已被拒绝，请不要重复退款", RequestHelper.RawUrl);
                    break;
                default:
                    ScriptHelper.Alert("无效的操作", RequestHelper.RawUrl);
                    break;
            }

            OrderRefundBLL.Update(submitOrderRefund);

            //退款操作记录
            AddOrderRefundAction(submitOrderRefund, approveStatus);

            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), submitOrderRefund.Id);
            ScriptHelper.Alert("操作成功", RequestHelper.RawUrl);
        }

        protected void RefundButton_Click(object sender, EventArgs e)
        {
            int orderRefundId = RequestHelper.GetQueryString<int>("Id");
            var submitOrderRefund = OrderRefundBLL.Read(orderRefundId);

            //更改状态为退款中...
            if (submitOrderRefund.Status == (int)OrderRefundStatus.Approve)
            {
                submitOrderRefund.Status = (int)OrderRefundStatus.Returning;
                submitOrderRefund.Remark = "正在处理退款";

                OrderRefundBLL.Update(submitOrderRefund);

                //退款操作记录
                AddOrderRefundAction(submitOrderRefund, (int)BoolType.True);
            }

            //退款到账户余额及各支付渠道
            if (submitOrderRefund.Status == (int)OrderRefundStatus.Returning)
            {
                JWRefund.RefundToAnyPay(submitOrderRefund);
              
            }
            else
            {
                ScriptHelper.Alert("无效的操作", RequestHelper.RawUrl);
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            int orderRefundId = RequestHelper.GetQueryString<int>("Id");
            var submitOrderRefund = OrderRefundBLL.Read(orderRefundId);

            //更改状态为已取消...
            submitOrderRefund.Status = (int)OrderRefundStatus.Cancel;
            submitOrderRefund.Remark = "系统取消了退款";
            OrderRefundBLL.Update(submitOrderRefund);

            //退款操作记录
            AddOrderRefundAction(submitOrderRefund, (int)BoolType.False);
            ScriptHelper.Alert("操作成功", RequestHelper.RawUrl);
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