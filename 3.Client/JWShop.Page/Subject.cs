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
    public class Subject : CommonBasePage
    {
        protected List<ThemeActivityInfo> themes = new List<ThemeActivityInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            themes = ThemeActivityBLL.ReadList();

            Title = "活动专题";
        }
    }
}