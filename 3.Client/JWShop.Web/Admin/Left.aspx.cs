using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class Left : JWShop.Page.AdminBasePage
    {
        protected List<MenuInfo> menuList = new List<MenuInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("ReadMenu", PowerCheckType.Single);

            int id = RequestHelper.GetQueryString<int>("Id");
            if (id < 1) id = 1;
            menuList = MenuBLL.ReadMenuChildList(id);
        }
    }
}