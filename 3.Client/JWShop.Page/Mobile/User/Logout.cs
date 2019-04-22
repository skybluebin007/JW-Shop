using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Mobile
{
    public class Logout : UserBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();

            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().UserCookies);
            CookiesHelper.DeleteCookie("UserPhoto");
            CookiesHelper.DeleteCookie("UserEmail");

            //退出登录时，重置静态用户对象
            base.CurrentUser = new UserInfo();

            ResponseHelper.Redirect("/mobile/login.html");
        }
    }
}