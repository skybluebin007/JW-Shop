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
    public partial class OrderRefundToBalance : JWShop.Page.AdminBasePage
    {
        protected string message = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("OrderRefundConfirm", PowerCheckType.Single);

            int id = RequestHelper.GetQueryString<int>("order_refund_id");
            
            var orderRefund = OrderRefundBLL.Read(id);
            if (orderRefund.Status != (int)OrderRefundStatus.Returning)
            {
                message = "无效的退款状态";
                return;
            }

            if (orderRefund.RefundBalance <= 0)
            {
                message = "退款金额必须大于0";
                return;
            }

            //需退款到第三方支付平台的退款单，一律在第三方支付退款成功后的回调页面操作
            if (orderRefund.RefundMoney > 0)
            {
                message = "请通过第三方平台进行退款";
                return;
            }

            var user = UserBLL.Read(orderRefund.OwnerId);
            if (user.Id < 0)
            {
                message = "无效的退款用户";
                return;
            }

            var order = OrderBLL.Read(orderRefund.OrderId);
            if (order.Id < 0)
            {
                message = "无效的退款订单";
                return;
            }


            //***********业务逻辑***************************************//
            /*************************************************************
            if (isSuccess)
            {
                msg = "退款完成";

                //更新状态
                orderRefund.Status = (int)OrderRefundStatus.HasReturn;
                orderRefund.TmRefund = DateTime.Now;
                OrderRefundBLL.Update(orderRefund);
            }
            else
            {
                message = msg;
            }

            OrderRefundActionBLL.Add(new OrderRefundActionInfo
            {
                OrderRefundId = orderRefund.Id,
                Status = isSuccess ? (int)BoolType.True : (int)BoolType.False,
                Tm = DateTime.Now,
                UserType = 2,
                UserId = base.AdminID,
                UserName = Cookies.Admin.GetAdminName(false),
                Remark = msg
            });
            *************************************************************/
        }
    }
}