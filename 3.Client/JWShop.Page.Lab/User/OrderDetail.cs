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

namespace JWShop.Page.Lab
{
    public class OrderDetail : UserBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            int orderId = RequestHelper.GetQueryString<int>("id");

            if (CurrentUser.UserType == (int)UserType.Member)
            {
                order = OrderBLL.Read(orderId, base.UserId);
            }
            else
            {
                order = OrderBLL.ReadByShopId(orderId, base.UserId);
            }

            if (order.Id <= 0)
            {
                ScriptHelper.AlertFront("订单不存在", "/user/index.html");
            }

            orderDetailList = OrderDetailBLL.ReadList(orderId);
            int[] productIds = orderDetailList.Select(k => k.ProductId).ToArray();
            if (productIds.Length > 0)
            {
                int count = 0;
                productList = ProductBLL.SearchList(1, productIds.Length, new ProductSearchInfo { InProductId = string.Join(",", productIds) }, ref count);
            }

            Title = "我的订单";
        }
    }
}