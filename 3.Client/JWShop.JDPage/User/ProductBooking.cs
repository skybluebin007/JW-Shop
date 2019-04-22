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

namespace JWShop.Page
{
    public class ProductBooking : UserBasePage
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 用户等级
        /// </summary>
        /// <summary>
        /// 缺货列表
        /// </summary>
        protected List<BookingProductInfo> bookingProductList = new List<BookingProductInfo>();

        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserId);
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            BookingProductSearchInfo bookingProductSearch = new BookingProductSearchInfo();
            bookingProductSearch.UserID = base.UserId;
            bookingProductList = BookingProductBLL.SearchBookingProductList(currentPage, pageSize, bookingProductSearch, ref count);
            commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
        }         
    }
}
