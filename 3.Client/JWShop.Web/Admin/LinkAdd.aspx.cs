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
    public partial class LinkAdd : JWShop.Page.AdminBasePage
    {
        protected int classID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                classID = RequestHelper.GetQueryString<int>("ClassID");
          
                int linkID = RequestHelper.GetQueryString<int>("ID");
                if (linkID != int.MinValue)
                {
                    CheckAdminPower("ReadLink", PowerCheckType.Single);
                    LinkInfo link = LinkBLL.ReadLinkCache(linkID);

                    TextDisplay.Text = link.Display;

                    URL.Text = link.URL;
                    Remark.Text = link.Remark;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LinkInfo link = new LinkInfo();
            link.ID = RequestHelper.GetQueryString<int>("ID");
            link.LinkClass =1;

            link.Display = TextDisplay.Text;

            link.URL = URL.Text;
            link.Remark = Remark.Text;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (link.ID == int.MinValue)
            {
                CheckAdminPower("AddLink", PowerCheckType.Single);
                int id = LinkBLL.AddLink(link);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Link"), id);
            }
            else
            {
                CheckAdminPower("UpdateLink", PowerCheckType.Single);
                LinkBLL.UpdateLink(link);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Link"), link.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
          
        }
    }
}