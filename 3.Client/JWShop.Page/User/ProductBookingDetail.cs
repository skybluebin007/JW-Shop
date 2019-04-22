using System;
using System.IO;
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
    public class ProductBookingDetail : UserBasePage
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 用户等级
        /// </summary>
        /// <summary>
        /// 缺货登记
        /// </summary>
        protected BookingProductInfo bookingProduct = new BookingProductInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserId);
            int id = RequestHelper.GetQueryString<int>("ID");
            bookingProduct = BookingProductBLL.ReadBookingProduct(id, base.UserId);
        }         
    }
}