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
    public class Login : CommonBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            if (base.IsAdminLogin())
            {
                Response.Redirect("/mobileadmin/index.html");
            }      
        }
    }
}
