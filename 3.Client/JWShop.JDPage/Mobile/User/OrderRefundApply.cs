using System;
using System.Data;
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

namespace JWShop.Page.Mobile
{
    public class OrderRefundApply : UserBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected OrderDetailInfo orderDetail = new OrderDetailInfo();
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();
        /// <summary>
        /// 最多可退金额
        /// </summary>
        protected decimal canRefundMoney = 0;

        protected override void PageLoad()
        {
            base.PageLoad();

            int orderId = RequestHelper.GetQueryString<int>("orderId");
            int orderDetailId = RequestHelper.GetQueryString<int>("orderDetailId");

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "CalcCanRefundMoney")
            {
                int refundCount = RequestHelper.GetQueryString<int>("num");
                order = OrderBLL.Read(orderId);
                orderDetail = OrderDetailBLL.Read(orderDetailId);
                var refundMsg = JWRefund.CanRefund(order, orderDetail, refundCount);
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(refundMsg));
                ResponseHelper.End();
            }
            if (action == "Submit")
            {
                this.Submit();
            }

            order = OrderBLL.Read(orderId);
            if (order.Id > 0)
            {
                JWRefundMsg refundMsg = new JWRefundMsg();
                if (orderDetailId > 0)
                {
                    orderDetail = OrderDetailBLL.Read(orderDetailId);
                    refundMsg = JWRefund.CanRefund(order, orderDetail, 1);
                }
                else
                {
                    refundMsg = JWRefund.CanRefund(order);
                }
                canRefundMoney = refundMsg.CanRefundMoney;
                orderRefundList = OrderRefundBLL.ReadListValid(order.Id);
            }

            Title = "退款申请";
        }

        private void Submit()
        {
            int orderId = RequestHelper.GetQueryString<int>("orderId");
            int orderDetailId = RequestHelper.GetQueryString<int>("orderDetailId");

            int needRefundCount = RequestHelper.GetForm<int>("refund_count");
            decimal needRefundMoney = RequestHelper.GetForm<decimal>("refund_money");
            string refundRemark = StringHelper.AddSafe(RequestHelper.GetForm<string>("refund_remark"));

            OrderRefundInfo orderRefund = new OrderRefundInfo();
            orderRefund.RefundNumber = ShopCommon.CreateOrderRefundNumber();
            orderRefund.OrderId = orderId;
            if (orderDetailId > 0)
            {
                orderRefund.OrderDetailId = orderDetailId;
                orderRefund.RefundCount = needRefundCount;
            }
            orderRefund.Status = (int)OrderRefundStatus.Submit;
            orderRefund.TmCreate = DateTime.Now;
            orderRefund.RefundRemark = refundRemark;
            orderRefund.UserType = 1;
            orderRefund.UserId = base.UserId;
            orderRefund.UserName = base.UserName;

            var refundMsg = JWRefund.VerifySubmitOrderRefund(orderRefund, needRefundMoney);
            if (refundMsg.CanRefund)
            {
                int id = OrderRefundBLL.Add(orderRefund);
                OrderRefundActionBLL.Add(new OrderRefundActionInfo
                {
                    OrderRefundId = id,
                    Status = (int)BoolType.True,
                    Tm = DateTime.Now,
                    UserType = 1,
                    UserId = base.UserId,
                    UserName = base.UserName,
                    Remark = "用户提交退款申请"
                });
                ResponseHelper.Write("ok|" + id);
                ResponseHelper.End();
            }
            else
            {
                ResponseHelper.Write("error|" + refundMsg.ErrorCodeMsg);
                ResponseHelper.End();
            }
        }

    }
}