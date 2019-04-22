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
    public abstract class ShopBasePage : UserBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();

            if (CurrentUser.UserType != (int)UserType.Provider)
            {
                ResponseHelper.Write("<script language='javascript'>alert('您不是供应商。');</script>");
                ResponseHelper.End();
            }       
        }
    }
}
