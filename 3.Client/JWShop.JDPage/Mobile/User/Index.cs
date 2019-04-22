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
    public class Index : UserBasePage
    {
        protected decimal money, point;
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected DataTable dt = new DataTable();
        protected string type = string.Empty;
        protected int[] arrT = new int[4];
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();
            //检查用户的待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime(base.UserId);
            type = RequestHelper.GetQueryString<string>("type");

            topNav = 7;
            
            string keywords = RequestHelper.GetQueryString<string>("keywords");
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
            int count = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.UserId = base.UserId;
            orderSearch.IsDelete = 0;
            if (!string.IsNullOrEmpty(keywords))
            {
                orderSearch.OrderNumber = keywords;
            }
            if (!string.IsNullOrEmpty(type))
            {
                int tt = 0;
                switch (type)
                {
                    case "2": tt = 2; break;
                    case "3": tt = 4; break;
                    case "4": tt = 5; break;
                    case "1": tt = 1; break;
                }
                orderSearch.OrderStatus = tt;
            }

            orderList = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count);
            
            commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
            #region 对应状态个数            
            arrT[0] = OrderBLL.SearchList(new OrderSearchInfo { UserId = base.UserId, IsDelete = 0 }).Count;
            orderSearch.OrderStatus = 2;
            arrT[1] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = 1;
            arrT[2] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = 5;
            arrT[3] = OrderBLL.SearchList(orderSearch).Count;
            #endregion
        }
    }
}