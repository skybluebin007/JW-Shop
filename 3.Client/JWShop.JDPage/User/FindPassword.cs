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
        protected string errorMessage=RequestHelper.GetQueryString<string>("ErrorMessage");
       
        /// <summary>
        /// 结果
        /// </summary>
        protected string result=RequestHelper.GetQueryString<string>("Result");
        /// <summary>
        /// 接收账户名
        /// </summary>
        protected string loginName { 
            get { return StringHelper.AddSafe(StringHelper.Decode(Server.UrlDecode(RequestHelper.GetQueryString<string>("u")), ShopConfig.ReadConfigInfo().SecureKey)); } 
        }
        /// <summary>
        /// 接收操作方法名
        /// </summary>
        protected string action {
            get { return RequestHelper.GetQueryString<string>("Action"); }
        }
        
        protected int wait = 0;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            //不带任何参数访问则跳至验证身份页面
            if (string.IsNullOrEmpty(loginName) && string.IsNullOrEmpty(action) && string.IsNullOrEmpty(result)) ResponseHelper.Redirect("/user/findpasswordinit.html");          
         
            Title = "找回密码";

            //短信验证码有效期
            string verify_send = CookiesHelper.ReadCookieValue("verify_send");
            DateTime tmNow = DateTime.Now;
            DateTime tm;
            try
            {
                tm = DateTime.Parse(verify_send);
            }
            catch { tm = tmNow.AddSeconds(-60); }
            wait = 60 - Convert.ToInt32((tmNow - tm).TotalSeconds);
            if (wait < 0) wait = 0;

            if (action == "Post")
            {
                Post();
            }
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected  void Post()
        {
            string userName = StringHelper.SearchSafe(Server.UrlDecode(RequestHelper.GetForm<string>("UserName")));
            string email = StringHelper.SearchSafe(HttpUtility.HtmlDecode(RequestHelper.GetForm<string>("Email")));
            string safeCode = StringHelper.AddSafe(HttpUtility.HtmlDecode(RequestHelper.GetForm<string>("SafeCode")));
            int checkType = RequestHelper.GetForm<int>("checkType");
            string mobile = StringHelper.AddSafe(HttpUtility.HtmlDecode(RequestHelper.GetForm<string>("Mobile")));
            string mobileCode = StringHelper.AddSafe(HttpUtility.HtmlDecode(RequestHelper.GetForm<string>("phoneVer")));
            UserInfo user = new UserInfo();
            //检查用户名
            if (userName == string.Empty)
            {
                errorMessage = "账户名错误";
            }
            if (errorMessage == string.Empty)
            {
                user = UserBLL.Read(userName);
                if (user.Id<= 0)
                {
                    errorMessage = "账户不存在";
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
                            errorMessage = "账户名和Email不匹配";
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
    }
}