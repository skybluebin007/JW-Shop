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
    public abstract class AjaxBasePage :BasePage
    {
        protected ClassRelation RelClass = new ClassRelation();

        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            ClearCache();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        protected void ClearCache()
        {
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Expires = 0;
        }
    }
}
