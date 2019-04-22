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
    public partial class WechatMenu : JWShop.Page.AdminBasePage
    {
        protected List<WechatMenuInfo> topMenuList = new List<WechatMenuInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string action=RequestHelper.GetQueryString<string>("Action");
            int menuId = RequestHelper.GetQueryString<int>("ID");           

            if (!string.IsNullOrEmpty(action) && menuId>0)
            {
                switch (action)
                {
                    case "Up":
                        WechatMenuBLL.MoveUpWechatMenu(menuId);                        
                        break;
                    case "Down":
                        WechatMenuBLL.MoveDownWechatMenu(menuId);
                        break;
                    case "Delete":
                        WechatMenuBLL.Delete(menuId);
                        break;
                    default:
                        break;
                }
            }
            topMenuList = WechatMenuBLL.ReadRootList();
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        { 
        }
    }
}