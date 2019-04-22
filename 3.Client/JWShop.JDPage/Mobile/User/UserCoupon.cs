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

namespace JWShop.Page.Mobile
{
   public class UserCoupon : UserBasePage
    {
       protected string type = string.Empty;
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        /// <summary>
        /// 用户优惠券列表
        /// </summary>
        protected List<UserCouponInfo> userCouponList = new List<UserCouponInfo>();
        /// <summary>
        /// 优惠券列表
        /// </summary>
        protected List<CouponInfo> couponList = new List<CouponInfo>();
       protected override void PageLoad()
       {
           base.PageLoad();
           type = RequestHelper.GetQueryString<string>("type");
           int currentPage = RequestHelper.GetQueryString<int>("Page");
           if (currentPage < 1)
           {
               currentPage = 1;
           }
           int pageSize = 10;
           int count = 0;

           UserCouponSearchInfo userCouponSearch = new UserCouponSearchInfo();
           userCouponSearch.UserId = base.UserId;
           if (!string.IsNullOrEmpty(type))
           {
               if (type == "1")//未使用
               {
                   userCouponSearch.IsUse = (int)BoolType.False;
                   userCouponSearch.IsTimeOut = (int)BoolType.False;
               }
               else if (type == "2")//已使用
               {
                   userCouponSearch.IsUse = (int)BoolType.True;
                   userCouponSearch.IsTimeOut = -1;//不限期限有没过期
               }
               else//已过期
               {
                   userCouponSearch.IsUse = (int)BoolType.False;
                   userCouponSearch.IsTimeOut = (int)BoolType.True;
               }
           }
           else {//所有
               userCouponSearch.IsTimeOut =-1;
           }
           userCouponList = UserCouponBLL.SearchList(currentPage, pageSize, userCouponSearch, ref count);
           commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

           string idList = string.Empty;
           foreach (UserCouponInfo userCoupon in userCouponList)
           {
               if (idList == string.Empty)
               {
                   idList = userCoupon.CouponId.ToString();
               }
               else
               {
                   idList += "," + userCoupon.CouponId.ToString();
               }
           }
           if (idList != string.Empty)
           {
               CouponSearchInfo couponSearch = new CouponSearchInfo();
               couponSearch.InCouponIds = Array.ConvertAll<string, int>(idList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), i => Convert.ToInt32(i));
               couponList = CouponBLL.SearchList(couponSearch);
           }

       }
    }
}
