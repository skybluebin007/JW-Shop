﻿using System;
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

namespace JWShop.Page
{
    public class Index : UserBasePage
    {
        protected decimal money, point;
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected DataTable dt = new DataTable();
        protected string type = "";
        protected int[] arrT = new int[4];
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();
            type = RequestHelper.GetQueryString<string>("type");

            topNav = 7;
            if (type == "")
                type = "1";
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
            if (!string.IsNullOrEmpty(type) && type != "1")
            {
                int tt = 0;
                switch (type)
                {
                    case "2": tt = 2; break;
                    case "3": tt = 4; break;
                    case "4": tt = 5; break;
                }
                orderSearch.OrderStatus = tt;
            }

            orderList = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count);            
            //commonPagerClass.CurrentPage = currentPage;
            //commonPagerClass.PageSize = pageSize;
            //commonPagerClass.Count = count;
            //commonPagerClass.FirstPage = "<<首页";
            //commonPagerClass.PreviewPage = "<<上一页";
            //commonPagerClass.NextPage = "下一页>>";
            //commonPagerClass.LastPage = "末页>>";
            //commonPagerClass.ListType = false;
            //commonPagerClass.DisCount = false;
            //commonPagerClass.PrenextType = true;
            commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
            #region 对应状态个数
            //OrderSearchInfo orderSearch1 = new OrderSearchInfo();
            //orderSearch1.UserId = base.UserId;
            //if (!string.IsNullOrEmpty(keywords))
            //{
            //    orderSearch1.OrderNumber = keywords;
            //}
            arrT[0] = OrderBLL.SearchList(currentPage, pageSize, new OrderSearchInfo { UserId=base.UserId,IsDelete=0}, ref count).Count;
            orderSearch.OrderStatus = 2;
            arrT[1] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            orderSearch.OrderStatus = 4;
            arrT[2] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            orderSearch.OrderStatus = 5;
            arrT[3] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            #endregion
        }
    }
}