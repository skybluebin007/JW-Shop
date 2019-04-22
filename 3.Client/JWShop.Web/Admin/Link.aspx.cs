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
    public partial class Link : JWShop.Page.AdminBasePage
    {
      
        protected int classID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            CheckAdminPower("ReadLink", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Delete")
            {
               
                CheckAdminPower("DeleteLink", PowerCheckType.Single);
                string deleteID = RequestHelper.GetQueryString<string>("ID");
                if (deleteID != string.Empty)
                {
                    LinkBLL.DeleteLink(deleteID);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Link"), deleteID);
                    ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), "Link.aspx");
                }
            }
            int linkID = RequestHelper.GetQueryString<int>("ID");
            if (action != string.Empty && linkID != int.MinValue)
            {
                CheckAdminPower("UpdateLink", PowerCheckType.Single);
                switch (action)
                {
                    case "Up":
                        LinkBLL.ChangeLinkOrder(ChangeAction.Up, linkID);
                        break;
                    case "Down":
                        LinkBLL.ChangeLinkOrder(ChangeAction.Down, linkID);
                        break;
                    default:
                        break;
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("Link"), linkID);
            }
            classID = RequestHelper.GetQueryString<int>("ClassID");
            if (classID == int.MinValue)
            {
                classID = 1;
            }
            BindControl(LinkBLL.ReadLinkCacheListByClass(classID), RecordList);   
        }
    }
}