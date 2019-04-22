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
    public partial class UserMessageAdd : JWShop.Page.AdminBasePage
    {
        protected UserMessageInfo userMessage = new UserMessageInfo();
        protected int  styleid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            styleid = RequestHelper.GetQueryString<int>("StryleID");
            if (!Page.IsPostBack)
            {
                int userMessageID = RequestHelper.GetQueryString<int>("ID");
                if (userMessageID != int.MinValue)
                {
                    CheckAdminPower("ReadUserMessage", PowerCheckType.Single);
                    userMessage = UserMessageBLL.Read(userMessageID);
                    AdminReplyContent.Text = userMessage.AdminReplyContent;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            UserMessageInfo userMessage = new UserMessageInfo();
            userMessage.Id = RequestHelper.GetQueryString<int>("ID");
            if (userMessage.Id > 0)
            {
                userMessage = UserMessageBLL.Read(userMessage.Id);
                userMessage.IsHandler = RequestHelper.GetForm<int>("IsHandler");
                userMessage.AdminReplyContent = AdminReplyContent.Text;
                userMessage.AdminReplyDate = RequestHelper.DateNow;

                CheckAdminPower("UpdateUserMessage", PowerCheckType.Single);
                UserMessageBLL.Update(userMessage);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("UserMessage"), userMessage.Id);
            }
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("OperateOK"), RequestHelper.RawUrl);
        }
    }
}