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
    public class FindPasswordInit : CommonBasePage
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 结果
        /// </summary>
        protected string result = string.Empty;

        protected int wait = 0;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            result = RequestHelper.GetQueryString<string>("Result");
            errorMessage = RequestHelper.GetQueryString<string>("ErrorMessage");

            Title = "找回密码";

            switch (RequestHelper.GetQueryString<string>("Action"))
            {
                case "Post":
                    Post();
                    break;
                case "CheckName":
                    CheckName();
                    break;
            }

            
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected  void Post()
        {
            string userName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            string email = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Email"));
            string safeCode = RequestHelper.GetForm<string>("SafeCode");
            int checkType = RequestHelper.GetForm<int>("checkType");
            string mobile = RequestHelper.GetForm<string>("Mobile");
            string mobileCode = RequestHelper.GetForm<string>("phoneVer");
            UserInfo user = new UserInfo();
            //检查用户名
            if (userName == string.Empty)
            {
                errorMessage = "用户名不能为空";
            }
            if (errorMessage == string.Empty)
            {
                user = UserBLL.Read(userName);
                if (user.Id<= 0)
                {
                    errorMessage = "不存在该用户名";
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
            switch (checkType)
            { 
                case 1://邮箱验证
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
                    //检查用户和Email是否匹配
                    if (errorMessage == string.Empty)
                    {
                       if (user.Email != email)
                        {
                            errorMessage = "用户名和Email不匹配";
                        }
                    }
                    //记录找回密码信息
                    if (errorMessage == string.Empty)
                    {
                        string tempSafeCode = Guid.NewGuid().ToString();
                        UserBLL.ChangeUserSafeCode(user.Id, tempSafeCode, RequestHelper.DateNow);
                        string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/User/ResetPassword.html?CheckCode=" + StringHelper.Encode(user.Id + "|" + email + "|" + userName + "|" + user.Mobile + "|" + tempSafeCode, ShopConfig.ReadConfigInfo().SecureKey);
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
                        Response.Clear();
                        Response.Write("ok|/User/FindPassword.html?Result=" + Server.UrlEncode(result));
                        Response.End();
                        //ResponseHelper.Redirect("/User/FindPassword.aspx?Result=" + Server.UrlEncode(result));
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("error|" + errorMessage);
                        Response.End();
                        //ResponseHelper.Redirect("/User/FindPassword.aspx?ErrorMessage=" + Server.UrlEncode(errorMessage));
                    }
                    break;
                case 0://手机验证
                    //检查Mobile
                    if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(mobile)) {
                        errorMessage = "请填写手机号";
                    }
                    //检查手机号是否匹配
                    if (string.IsNullOrEmpty(errorMessage) && !string.Equals(user.Mobile,mobile))
                    {
                        errorMessage = "请填写正确有效的手机号";
                    }
                    //检查校验码
                    if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(mobileCode))
                    {
                        errorMessage = "请填写短信校验码";
                    }
                    //手机短信校验码
                    if (CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(mobile)) == null)
                    {
                        errorMessage = "校验码失效，请重新获取";
                    }
                    else
                    {
                        string cookieMobileCode = CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(mobile)).Value.ToString();
                        if (cookieMobileCode.ToLower() != mobileCode.ToLower())
                        {
                            errorMessage = "校验码错误";
                        }
                        else
                        {
                            CookiesHelper.DeleteCookie("MobileCode" + StringHelper.AddSafe(mobile));
                        }
                    }
                    //找回密码
                    if (errorMessage == string.Empty)
                    {
                        string tempSafeCode = Guid.NewGuid().ToString();
                        UserBLL.ChangeUserSafeCode(user.Id, tempSafeCode, RequestHelper.DateNow);
                        string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/User/ResetPassword.html?CheckCode=" + StringHelper.Encode(user.Id + "|" + user.Email + "|" + userName + "|" + mobile + "|" + tempSafeCode, ShopConfig.ReadConfigInfo().SecureKey);                       
                       
                        Response.Clear();
                        Response.Write("ok|" + url);
                        Response.End();
                        //ResponseHelper.Redirect("/User/FindPassword.aspx?Result=" + Server.UrlEncode(result));
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("error|" + errorMessage);
                        Response.End();
                        //ResponseHelper.Redirect("/User/FindPassword.aspx?ErrorMessage=" + Server.UrlEncode(errorMessage));
                    }
                    break;
            }        
           


        }
        /// <summary>
        /// 检查账户名
        /// </summary>
        protected void CheckName() {
            string userName = StringHelper.AddSafe(HttpUtility.HtmlDecode(RequestHelper.GetQueryString<string>("UserName")));
            string safeCode = StringHelper.AddSafe(HttpUtility.HtmlDecode(RequestHelper.GetQueryString<string>("SafeCode")));
            //检查用户名
            if (userName == string.Empty)
            {
                errorMessage = "账户名不能为空";
            }
            if (errorMessage == string.Empty)
            {
               var user = UserBLL.Read(userName);
                if (user.Id <= 0)
                {
                    errorMessage = "不存在该账户名";
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
            if (string.IsNullOrEmpty(errorMessage))
            {
                string url = "/user/findpassword.html?u=" + StringHelper.Encode(Server.UrlEncode(userName),ShopConfig.ReadConfigInfo().SecureKey);
                Response.Clear();
                Response.Write("ok|" + url);
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write("error|" + errorMessage);
                Response.End();
            }
        }
    }
}