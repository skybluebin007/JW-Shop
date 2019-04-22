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
    public partial class WechatMenuAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FatherID.DataSource = WechatMenuBLL.ReadRootList();
                FatherID.DataTextField = "Name";
                FatherID.DataValueField = "Id";
                FatherID.DataBind();
                FatherID.Items.Insert(0, new ListItem("作为一级菜单", "0"));
                OrderID.Text = WechatMenuBLL.MaxOrderId("WechatMenu").ToString();
                int menuID = RequestHelper.GetQueryString<int>("ID");
                if (menuID>0)
                { 
                    var theMenu=WechatMenuBLL.Read(menuID);
                    FatherID.Text = theMenu.FatherId.ToString();
                    Name.Text = theMenu.Name;
                    MenuType.Text = theMenu.Type;
                    OrderID.Text = theMenu.OrderId.ToString();
                    if (theMenu.Type == "click") MenuKey.Text = theMenu.Key;
                    else MenuKey.Text = theMenu.Url;
                }
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            WechatMenuInfo wechatmenu = new WechatMenuInfo();
            wechatmenu.Id = RequestHelper.GetQueryString<int>("ID");           
            wechatmenu.Name = Name.Text;
            wechatmenu.FatherId = Convert.ToInt32(FatherID.Text);
            wechatmenu.OrderId = Convert.ToInt32(OrderID.Text);
            wechatmenu.Type = this.MenuType.Text;
            if (wechatmenu.Type == "click") wechatmenu.Key = MenuKey.Text;
            else wechatmenu.Url = MenuKey.Text;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            //添加
            if (wechatmenu.Id<=0)
            {
                if (FatherID.Text == "0" && WechatMenuBLL.ReadRootList().Count >= 3)
                {
                    ScriptHelper.Alert("最多只能添加3个一级菜单");
                    return;
                }
                if (FatherID.Text != "0" && WechatMenuBLL.ReadChildList(Convert.ToInt32(FatherID.Text)).Count >= 5)
                {
                    ScriptHelper.Alert("每个一级菜单下最多只能添加5个二级菜单");
                    return;
                }
                int id = WechatMenuBLL.Add(wechatmenu);               
            }
            else//修改
            {
                //如果更改了父级菜单且原先已有3个一级菜单
               string _oldFatherId = WechatMenuBLL.Read(wechatmenu.Id).FatherId.ToString();
                if (FatherID.Text == "0" && FatherID.Text!=_oldFatherId && WechatMenuBLL.ReadRootList().Count >= 3)
                {
                    ScriptHelper.Alert("最多只能添加3个一级菜单");
                    return;
                }
                //如果更改了父级菜单且该菜单下原先已有5个二级菜单
                if (FatherID.Text != "0" && FatherID.Text!=_oldFatherId && WechatMenuBLL.ReadChildList(Convert.ToInt32(FatherID.Text)).Count >= 5)
                {
                    ScriptHelper.Alert("每个一级菜单下最多只能添加5个二级菜单");
                    return;
                }

                WechatMenuBLL.Update(wechatmenu);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);

        }
    }
}