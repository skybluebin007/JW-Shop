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

namespace JWShop.Web.Admin
{
    public partial class NavMenu : JWShop.Page.AdminBasePage
    {
        protected List<NavMenuInfo> navMenuList = new List<NavMenuInfo>();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("ReadNavMenu", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            int navMenuID = RequestHelper.GetQueryString<int>("ID");
            if (action != string.Empty && navMenuID != int.MinValue)
            {
                switch (action)
                {
                    case "Delete":
                        CheckAdminPower("DeleteNavMenu", PowerCheckType.Single);
                        NavMenuBLL.Delete(navMenuID);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), "导航菜单", navMenuID);
                        break;
                    default:
                        break;
                }
            }

            navMenuList = NavMenuBLL.ReadList();
            //BindControl(NavMenuBLL.ReadRootList(), RecordList);
        }

    }
}