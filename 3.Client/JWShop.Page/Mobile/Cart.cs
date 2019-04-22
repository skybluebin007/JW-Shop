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
using System.Linq;

namespace JWShop.Page.Mobile
{
    public class Cart : CommonBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 11;

            Title = "购物车";
        }
    }
}