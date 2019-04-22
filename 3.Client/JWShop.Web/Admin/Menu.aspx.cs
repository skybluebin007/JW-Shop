using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class Menu : JWShop.Page.AdminBasePage
    {
        protected int fatherID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("ReadMenu", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            int menuID = RequestHelper.GetQueryString<int>("ID");
            fatherID = RequestHelper.GetQueryString<int>("FatherID");
            if (fatherID == int.MinValue)
            {
                fatherID = 1;
            }
            if (action != string.Empty && menuID != int.MinValue)
            {
                switch (action)
                {
                    case "Up":
                        CheckAdminPower("UpdateMenu", PowerCheckType.Single);
                        MenuBLL.MoveUpMenu(menuID);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("Menu"), menuID);
                        break;
                    case "Down":
                        CheckAdminPower("UpdateMenu", PowerCheckType.Single);
                        MenuBLL.MoveDownMenu(menuID);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("Menu"), menuID);
                        break;
                    case "Delete":
                        CheckAdminPower("DeleteMenu", PowerCheckType.Single);
                        MenuBLL.DeleteMenu(menuID);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Menu"), menuID);
                        break;
                    default:
                        break;
                }
            }
            BindControl(MenuBLL.ReadMenuAllNamedChildList(fatherID), RecordList);  
        }
    }
}