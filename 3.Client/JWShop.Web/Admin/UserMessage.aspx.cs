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
    public partial class UserMessage : JWShop.Page.AdminBasePage
    {
        protected int classID = 0;
        protected int styleid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {        
              
                CheckAdminPower("ReadUserMessage", PowerCheckType.Single);
                classID = RequestHelper.GetQueryString<int>("MessageClass");
                UserMessageSeachInfo userMessageSearch = new UserMessageSeachInfo();         
                userMessageSearch.MessageClass = RequestHelper.GetQueryString<int>("MessageClass");
                userMessageSearch.Title = RequestHelper.GetQueryString<string>("Title");
                userMessageSearch.StartPostDate = RequestHelper.GetQueryString<DateTime>("StartPostDate");
                userMessageSearch.EndPostDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndPostDate"));
                userMessageSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
                userMessageSearch.IsHandler = RequestHelper.GetQueryString<int>("IsHandler");
                MessageClass.Text = RequestHelper.GetQueryString<string>("MessageClass");
                Title.Text = RequestHelper.GetQueryString<string>("Title");
                StartPostDate.Text = RequestHelper.GetQueryString<string>("StartPostDate");
                EndPostDate.Text = RequestHelper.GetQueryString<string>("EndPostDate");
                UserName.Text = RequestHelper.GetQueryString<string>("UserName");
                IsHandler.Text = RequestHelper.GetQueryString<string>("IsHandler");
                BindControl(UserMessageBLL.SearchList(CurrentPage, PageSize, userMessageSearch, ref Count), RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteUserMessage", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                UserMessageBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("UserMessage"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserMessage.aspx?Action=search&";
            URL += "IsHandler=" + IsHandler.Text + "&";
            URL += "MessageClass=" + MessageClass.Text + "&";
            URL += "Title=" + Title.Text + "&";
            URL += "StartPostDate=" + StartPostDate.Text + "&";
            URL += "EndPostDate=" + EndPostDate.Text + "&";
            URL += "UserName=" + UserName.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}