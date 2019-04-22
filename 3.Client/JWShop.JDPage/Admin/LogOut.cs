using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
namespace JWShop.Page.Admin
{
  public  class LogOut:AdminBase
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("LogoutSystem"));
            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);
            ResponseHelper.Redirect("/MobileAdmin/Login.html");
        }
    }
}
