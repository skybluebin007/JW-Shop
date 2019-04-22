using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Text.RegularExpressions;

namespace JWShop.Page.Mobile
{
   public class UpdateEmail:UserBasePage
    {
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
         
            if (RequestHelper.GetQueryString<string>("Action") == "UpdateEmail")
            {
                UpdateUserEmail();
            }
        }
        /// <summary>
        /// 修改邮箱
        /// </summary>
        protected void UpdateUserEmail()
        {
            string msg = string.Empty;

            try
            {
                UserInfo user = UserBLL.Read(base.UserId);
                string email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
                string safeCode = RequestHelper.GetForm<string>("SafeCode");
                //检查邮箱
                if (msg == string.Empty)
                {
                    if (string.IsNullOrEmpty(email))
                    {
                        msg = "error|请填写邮箱";
                    }
                    else if (!new Regex("^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3}){1,2})$").IsMatch(email))
                    {
                        msg = "error|请填写邮箱";
                    }
                    else
                    {
                        if (!UserBLL.CheckEmail(email, base.UserId))
                        {
                            msg = "error|Email已被其他会员绑定";
                        }
                    }
                }
                //检查验证码
                if (msg == string.Empty)
                {
                    if (safeCode.ToLower() != Cookies.Common.CheckCode.ToLower())
                    {
                        msg = "error|验证码错误";
                    }
                }
                if (msg == string.Empty)
                {
                    string tempSafeCode = Guid.NewGuid().ToString();
                    UserBLL.ChangeUserSafeCode(user.Id, tempSafeCode, RequestHelper.DateNow);
                    string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/mobile/User/BindEmail.html?CheckCode=" + StringHelper.Encode(user.Id + "|" + email + "|" + tempSafeCode, ShopConfig.ReadConfigInfo().SecureKey);
                    EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent("BindEmail");
                    EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                    emailSendRecord.Title = emailContent.EmailTitle;
                    emailSendRecord.Content = emailContent.EmailContent.Replace("$Url$", url).Replace("$Hour$", ShopConfig.ReadConfigInfo().BindEmailTime.ToString());
                    emailSendRecord.IsSystem = (int)BoolType.True;
                    emailSendRecord.EmailList = email;
                    emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                    emailSendRecord.SendStatus = (int)SendStatus.No;
                    emailSendRecord.AddDate = RequestHelper.DateNow;
                    emailSendRecord.SendDate = RequestHelper.DateNow;
                    emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                    EmailSendRecordBLL.SendEmail(emailSendRecord);
                    result = "已发送验证邮件至您填写的邮箱,请在" + ShopConfig.ReadConfigInfo().BindEmailTime.ToString() + "小时内完成验证！<a href=\"http://mail." + email.Substring(email.IndexOf("@") + 1) + "\"  target=\"_blank\">马上登录邮箱</a>";

                    msg = "ok|/mobile/User/UpdateEmail.html?Result=" + Server.UrlEncode(result);

                }
                Response.Clear();
                Response.Write(msg);
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write("error|系统忙，请稍后重试");
            }
            finally
            {
                Response.End();
            }
        }
    }
}
