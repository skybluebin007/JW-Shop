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
    public class OrderRefundDetail : UserBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected OrderRefundInfo orderRefund = new OrderRefundInfo();
        protected List<OrderRefundActionInfo> orderRefundActionList = new List<OrderRefundActionInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "CancelRefund") this.CancelRefund();

            int id = RequestHelper.GetQueryString<int>("id");
            orderRefund = OrderRefundBLL.Read(id);
            orderRefundActionList = OrderRefundActionBLL.ReadList(id);

            order = OrderBLL.Read(orderRefund.OrderId);

            Title = "退款记录";
        }

        private void CancelRefund()
        {
            int id = RequestHelper.GetQueryString<int>("id");
            var submitOrderRefund = OrderRefundBLL.Read(id);

            if (submitOrderRefund.OwnerId != base.UserId)
            {
                ResponseHelper.Write("error|无效的请求");
                ResponseHelper.End();
            }

            if (OrderRefundBLL.CanToReturn(submitOrderRefund.Status))
            {
                submitOrderRefund.Status = (int)OrderRefundStatus.Cancel;
                submitOrderRefund.Remark = "用户取消了退款";
                OrderRefundBLL.Update(submitOrderRefund);

                OrderRefundActionInfo submitOrderRefundAction = new OrderRefundActionInfo
                {
                    OrderRefundId = submitOrderRefund.Id,
                    Status = (int)BoolType.False,
                    Remark = submitOrderRefund.Remark,
                    Tm = DateTime.Now,
                    UserType = 1,
                    UserId = base.UserId,
                    UserName = base.UserName
                };

                OrderRefundActionBLL.Add(submitOrderRefundAction);

                ResponseHelper.Write("ok|");
                ResponseHelper.End();
            }
            else
            {
                ResponseHelper.Write("error|无效的请求");
                ResponseHelper.End();
            }
        }

    }
}