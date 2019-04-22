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

namespace JWShop.Page.Mobile
{
    public class Order : UserBasePage
    {
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        /// <summary>
        /// 订单列表
        /// </summary>
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            string startDate = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("StartDate"));
            string endDate = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("EndDate"));
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.UserId = base.UserId;
            orderSearch.IsDelete = (int)BoolType.False;
            if (startDate != string.Empty) orderSearch.StartAddDate = Convert.ToDateTime(startDate);
            if (endDate != string.Empty) orderSearch.EndAddDate = Convert.ToDateTime(endDate);

            orderList = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count);
            commonPagerClass.CurrentPage = currentPage;
            commonPagerClass.PageSize = pageSize;
            commonPagerClass.Count = count;
        }
    }
}