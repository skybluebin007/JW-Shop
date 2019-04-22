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
    public partial class MenuAdd : JWShop.Page.AdminBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int fatherID = 0;
                fatherID = RequestHelper.GetQueryString<int>("FatherID");
                if (fatherID == int.MinValue)
                {
                    fatherID = 1;
                }
                MenuInfo fatherMenu = MenuBLL.ReadMenuCache(fatherID);
                FatherID.DataSource = MenuBLL.ReadMenuAllNamedChildList(fatherID);
                FatherID.DataTextField = "MenuName";
                FatherID.DataValueField = "ID";
                FatherID.DataBind();
                FatherID.Items.Insert(0, new ListItem(fatherMenu.MenuName, fatherMenu.ID.ToString()));
                for (int i = 1; i <= 60; i++)
                {
                    MenuImage.Items.Add(new ListItem("<img src=\"/Admin/Style/Icon/" + i + "-icon.gif\"/>", i.ToString()));
                }
                MenuImage.SelectedIndex = 0;

                int menuID = RequestHelper.GetQueryString<int>("ID");
                if (menuID != int.MinValue)
                {
                    CheckAdminPower("ReadMenu", PowerCheckType.Single);
                    MenuInfo menu = MenuBLL.ReadMenuCache(menuID);
                    FatherID.Text = menu.FatherID.ToString();
                    OrderID.Text = menu.OrderID.ToString();
                    MenuName.Text = menu.MenuName;
                    MenuImage.SelectedValue = menu.MenuImage.ToString();
                    URL.Text = menu.URL;
                }
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            MenuInfo menu = new MenuInfo();
            menu.ID = RequestHelper.GetQueryString<int>("ID");
            menu.FatherID = Convert.ToInt32(FatherID.Text);
            menu.OrderID = Convert.ToInt32(OrderID.Text);
            menu.MenuName = MenuName.Text;
            menu.MenuImage = Convert.ToInt32(MenuImage.Text);
            menu.URL = URL.Text;
            menu.Date = RequestHelper.DateNow;
            menu.IP = ClientHelper.IP;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (menu.ID == int.MinValue)
            {
                CheckAdminPower("AddMenu", PowerCheckType.Single);
                int id = MenuBLL.AddMenu(menu);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Menu"), id);
            }
            else
            {
                CheckAdminPower("UpdateMenu", PowerCheckType.Single);
                MenuBLL.UpdateMenu(menu);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Menu"), menu.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}