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
    public class UserCouponAjax : UserAjaxBasePage
    {
        /// <summary>
        /// 操作
        /// </summary>
        protected string action = string.Empty;
        /// <summary>
        /// 用户优惠券列表
        /// </summary>
        protected List<UserCouponInfo> userCouponList = new List<UserCouponInfo>();
        /// <summary>
        /// 优惠券列表
        /// </summary>
        protected List<CouponInfo> couponList = new List<CouponInfo>();
        /// <summary>
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            action = RequestHelper.GetQueryString<string>("Action");
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            switch (action)
            {
                case "NotUse":
                case "HasUse":
                case"TimeOut":
                    UserCouponSearchInfo userCouponSearch = new UserCouponSearchInfo();
                    userCouponSearch.UserId = base.UserId;
                    if (action == "NotUse")
                    {
                        userCouponSearch.IsUse = (int)BoolType.False;
                        userCouponSearch.IsTimeOut = (int)BoolType.False;
                    }
                    else if (action == "HasUse")
                    {
                        userCouponSearch.IsUse = (int)BoolType.True;
                        userCouponSearch.IsTimeOut = -1;//不限期限有没过期
                    }
                    else
                    {
                        userCouponSearch.IsUse = (int)BoolType.False;
                        userCouponSearch.IsTimeOut = (int)BoolType.True;
                    }
                    userCouponList = UserCouponBLL.SearchList(currentPage, pageSize, userCouponSearch, ref count);
                    ajaxPagerClass.CurrentPage = currentPage;
                    ajaxPagerClass.PageSize = pageSize;
                    ajaxPagerClass.Count = count;
                    ajaxPagerClass.FirstPage = "<<首页";
            ajaxPagerClass.PreviewPage = "<<上一页";
            ajaxPagerClass.NextPage = "下一页>>";
            ajaxPagerClass.LastPage = "末页>>";
            ajaxPagerClass.ListType = false;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.PrenextType = true;

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
                    break;
                case "AddCoupon":
                    break;
                case "AddUserCoupon":
                    AddUserCoupon();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 添加优惠劵
        /// </summary>
        protected void AddUserCoupon()
        {
            string result = string.Empty;
            string number = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Number"));
            string password = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Password"));
            if (number == string.Empty || password == string.Empty)
            {
                result = "请填写优惠券卡号和密码";
            }
            else
            {
                UserCouponInfo userCoupon = UserCouponBLL.Read(number, password);
                if (userCoupon.Id <= 0)
                {
                    result = "卡号或者密码错误";
                }
                else
                {

                    if (userCoupon.UserId > 0)
                    {
                        result = "该优惠券已经绑定了用户";
                    }
                    else
                    {
                        if (userCoupon.IsUse == (int)BoolType.True)
                        {
                            result = "该优惠券已经使用了"; 
                        }
                        else
                        {
                            CouponInfo coupon = CouponBLL.Read(userCoupon.CouponId);
                            if (RequestHelper.DateNow >= coupon.UseStartDate && RequestHelper.DateNow <= coupon.UseEndDate)
                            {
                                userCoupon.UserId = base.UserId;
                                userCoupon.UserName = base.UserName;
                                UserCouponBLL.Update(userCoupon);
                            }
                            else
                            {
                                result = "该优惠券没在使用期限内";
                            }
                        }
                    }
                    
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}

