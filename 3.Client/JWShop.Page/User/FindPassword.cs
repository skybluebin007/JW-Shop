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

namespace JWShop.Page
{
    public class FindPassword : CommonBasePage
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 结果
        /// </summary>
        protected string result = string.Empty;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            result = RequestHelper.GetQueryString<string>("Result");
            errorMessage = RequestHelper.GetQueryString<string>("ErrorMessage");
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            string userName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            string email = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Email"));
            string safeCode = RequestHelper.GetForm<string>("SafeCode");
            int userID = 0;
            //检查用户名
            if (userName == string.Empty)
            {
                errorMessage = "用户名不能为空";
            }
            if (errorMessage == string.Empty)
            {
                userID = UserBLL.Read(userName).Id;
                if (userID == 0)
                {
                    errorMessage = "不存在该用户名";
                }
            }
            //检查Email
            if (errorMessage == string.Empty)
            {
                if (email == string.Empty)
                {
                    errorMessage = "Email不能为空";
                }
            }
            if (errorMessage == string.Empty)
            {
                if (UserBLL.CheckEmail(email))
                {
                    errorMessage = "不存在该Email";
                }
            }
            //检查验证码
            if (errorMessage == string.Empty)
            {
                if (safeCode.ToLower() != Cookies.Common.CheckCode.ToLower())
                {
                    errorMessage = "验证码错误";
                }
            }
            //检查用户和Email是否匹配
            if (errorMessage == string.Empty)
            {
                UserInfo user = UserBLL.Read(userID);
                if (user.Email != email)
                {
                    errorMessage = "用户名和Email不匹配";
                }
            }
            //记录找回密码信息
            if (errorMessage == string.Empty)
            {
                string tempSafeCode = Guid.NewGuid().ToString();
                UserBLL.ChangeUserSafeCode(userID, tempSafeCode, RequestHelper.DateNow);
                string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/User/ResetPassword.aspx?CheckCode=" + StringHelper.Encode(userID + "|" + email + "|" + userName + "|" + tempSafeCode, ShopConfig.ReadConfigInfo().SecureKey);
                EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent("FindPassword");
                EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                emailSendRecord.Title = emailContent.EmailTitle;
                emailSendRecord.Content = emailContent.EmailContent.Replace("$Url$", url);
                emailSendRecord.IsSystem = (int)BoolType.True;
                emailSendRecord.EmailList = email;
                emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                emailSendRecord.SendStatus = (int)SendStatus.No;
                emailSendRecord.AddDate = RequestHelper.DateNow;
                emailSendRecord.SendDate = RequestHelper.DateNow;
                emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                EmailSendRecordBLL.SendEmail(emailSendRecord);
                result = "您的申请已提交，请登录邮箱重设你的密码！<a href=\"http://mail." + email.Substring(email.IndexOf("@") + 1) + "\"  target=\"_blank\">马上登录</a>";
                ResponseHelper.Redirect("/User/FindPassword.aspx?Result=" + Server.UrlEncode(result));
            }
            else
            {
                ResponseHelper.Redirect("/User/FindPassword.aspx?ErrorMessage=" + Server.UrlEncode(errorMessage));
            }

        }        
    }
}