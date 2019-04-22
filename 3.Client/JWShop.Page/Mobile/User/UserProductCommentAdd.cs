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

namespace JWShop.Page.Mobile
{
    public class UserProductCommentAdd : UserBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected string productIds;

        protected override void PageLoad()
        {
            base.PageLoad();

            int orderId = RequestHelper.GetQueryString<int>("orderId");

            order = OrderBLL.Read(orderId, base.UserId);
            orderDetailList = OrderDetailBLL.ReadList(orderId);
            productIds = string.Join(",", orderDetailList.Select(k => k.ProductId).ToArray());

            Title = "订单评价";
        }
    }
}