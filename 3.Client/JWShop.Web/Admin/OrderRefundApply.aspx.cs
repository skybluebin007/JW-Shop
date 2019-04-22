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
    public partial class OrderRefundApply : JWShop.Page.AdminBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected OrderDetailInfo orderDetail = new OrderDetailInfo();
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();
        /// <summary>
        /// 最多可退金额
        /// </summary>
        protected decimal canRefundMoney = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
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

            if (!Page.IsPostBack)
            {
                order = OrderBLL.Read(orderId);
                if (order.Id > 0)
                {
                    CheckAdminPower("OrderRefundApply", PowerCheckType.Single);

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
                    RefundMoney.Text = canRefundMoney.ToString();
                    orderRefundList = OrderRefundBLL.ReadListValid(order.Id);
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("OrderRefundApply", PowerCheckType.Single);

            int orderId = RequestHelper.GetQueryString<int>("orderId");
            int orderDetailId = RequestHelper.GetQueryString<int>("orderDetailId");
            decimal needRefundMoney = Convert.ToDecimal(RefundMoney.Text);

            OrderRefundInfo orderRefund = new OrderRefundInfo();
            orderRefund.RefundNumber = ShopCommon.CreateOrderRefundNumber();
            orderRefund.OrderId = orderId;
            if (orderDetailId > 0)
            {
                orderRefund.OrderDetailId = orderDetailId;
                orderRefund.RefundCount = Convert.ToInt32(RefundCount.Text);
            }
            orderRefund.Status = (int)OrderRefundStatus.Submit;
            orderRefund.TmCreate = DateTime.Now;
            orderRefund.RefundRemark = RefundRemark.Text;
            orderRefund.UserType = 2;
            orderRefund.UserId = Cookies.Admin.GetAdminID(false);
            orderRefund.UserName = Cookies.Admin.GetAdminName(false);

            var refundMsg = JWRefund.VerifySubmitOrderRefund(orderRefund, needRefundMoney);
            if (refundMsg.CanRefund)
            {
                int id = OrderRefundBLL.Add(orderRefund);
                OrderRefundActionBLL.Add(new OrderRefundActionInfo
                {
                    OrderRefundId = id,
                    Status = (int)BoolType.True,
                    Tm = DateTime.Now,
                    UserType = 2,
                    UserId = orderRefund.UserId,
                    UserName = orderRefund.UserName,
                    Remark = "系统提交退款申请"
                });
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("OrderRefund"), id);
                ScriptHelper.Alert("退款申请成功", RequestHelper.RawUrl);
            }
            else
            {
                ScriptHelper.Alert(refundMsg.ErrorCodeMsg);
            }
        }

    }
}