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
    public partial class NavMenuAdd : JWShop.Page.AdminBasePage
    {
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {               
                int navMenuID = RequestHelper.GetQueryString<int>("ID");
                if (navMenuID != int.MinValue)
                {
                    CheckAdminPower("ReadNavMenu", PowerCheckType.Single);
                    NavMenuInfo navMenu = NavMenuBLL.Read(navMenuID);
                    OrderID.Text = navMenu.OrderId.ToString();
                    Name.Text = navMenu.Name;
                    LinkUrl.Text = navMenu.LinkUrl;
                    Introduce.Text = navMenu.Introduce;

                    IsShow.Checked = Convert.ToBoolean(navMenu.IsShow);
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
            NavMenuInfo navMenu = new NavMenuInfo();
            navMenu.Id = RequestHelper.GetQueryString<int>("ID");            
            navMenu.OrderId = Convert.ToInt32(OrderID.Text);
            navMenu.Name = Name.Text;
            navMenu.LinkUrl = LinkUrl.Text;
            navMenu.Introduce = Introduce.Text;
            navMenu.IsShow = Convert.ToInt32(IsShow.Checked);            

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (navMenu.Id == int.MinValue)
            {
                CheckAdminPower("AddNavMenu", PowerCheckType.Single);
                int id = NavMenuBLL.Add(navMenu);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), "导航菜单", id);
            }
            else
            {
                CheckAdminPower("UpdateNavMenu", PowerCheckType.Single);
                NavMenuBLL.Update(navMenu);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), "导航菜单", navMenu.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, "/Admin/NavMenu.aspx");
        }
    }
}