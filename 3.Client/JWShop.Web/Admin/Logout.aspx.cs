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

namespace JWShop.Web.Admin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("LogoutSystem"));
            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);            
            ResponseHelper.Redirect("/Admin/Login.aspx");
        }
    }
}
