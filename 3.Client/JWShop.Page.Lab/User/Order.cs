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

namespace JWShop.Page.Lab
{
    public class Order : UserBasePage
    {
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected int orderStatus = int.MinValue;
        protected DataTable dt = new DataTable();
        protected CommonPagerClass pager = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            string orderNumber = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("OrderNum"));
            orderStatus = RequestHelper.GetQueryString<int>("s");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
            int count = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            if (!string.IsNullOrEmpty(orderNumber)) orderSearch.OrderNumber = orderNumber;
            if (orderStatus > 0) orderSearch.OrderStatus = orderStatus;
            if (CurrentUser.UserType == (int)UserType.Member)
                orderSearch.UserId = base.UserId;
            else
                orderSearch.ShopId = base.UserId;
            orderSearch.IsDelete = (int)BoolType.False;
            orderList = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count);

            dt = CurrentUser.UserType == (int)UserType.Member ? UserBLL.UserIndexStatistics(base.UserId) : UserBLL.ShopIndexStatistics(base.UserId);

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            Title = "我的订单";
        }
    }
}