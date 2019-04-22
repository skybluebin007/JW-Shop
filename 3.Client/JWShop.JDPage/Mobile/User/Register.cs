using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;

namespace JWShop.Page.Mobile
{
    public class Register : CommonBasePage
    {
        protected int wait = 0;
        /// <summary>
        /// 错误提示
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 注册结果
        /// </summary>
        protected string result = string.Empty;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 12;

            result = RequestHelper.GetQueryString<string>("Result");
            errorMessage = RequestHelper.GetQueryString<string>("ErrorMessage");
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            string userName = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName")));
            string email = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("Email")));
            string userPassword1 = RequestHelper.GetForm<string>("UserPassword1");
            string userPassword2 = RequestHelper.GetForm<string>("UserPassword2");
            string safeCode = RequestHelper.GetForm<string>("SafeCode");
            string Phone = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("Phone")));
            string phoneCode = RequestHelper.GetForm<string>("PhoneCode");
            //检查用户名
            if (userName == string.Empty)
            {
                errorMessage = "用户名不能为空";
            }
            if (errorMessage == string.Empty)
            {
                string forbiddinName = ShopConfig.ReadConfigInfo().ForbiddenName;
                if (forbiddinName != string.Empty)
                {
                    foreach (string TempName in forbiddinName.Split('|'))
                    {
                        if (userName.IndexOf(TempName.Trim()) != -1)
                        {
                            errorMessage = "用户名含有非法字符";
                            break;
                        }
                    }
                }
            }
            if (errorMessage == string.Empty)
            {
                if (!UserBLL.UniqueUser(userName))
                {
                    errorMessage = "用户名已经被占用";
                }
            }
            if (errorMessage == string.Empty)
            {
                Regex rg = new Regex("^([a-zA-Z0-9_\u4E00-\u9FA5])+$");
                if (!rg.IsMatch(userName))
                {
                    errorMessage = "用户名只能包含字母、数字、下划线、中文";
                }
            }
            //检查密码
            if (errorMessage == string.Empty)
            {
                if (userPassword1 == string.Empty || userPassword2 == string.Empty)
                {
                    errorMessage = "密码不能为空";
                }
            }
            if (errorMessage == string.Empty)
            {
                if (userPassword1 != userPassword2)
                {
                    errorMessage = "两次密码不一致";
                }
            }
           
            //检查手机 邮箱 验证码
            if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
            {//短信验证
                if (errorMessage == string.Empty)
                {

                    if (!ShopCommon.CheckMobile(Phone))
                    {
                        errorMessage = "手机号码错误";
                    }
                }
                if (errorMessage == string.Empty)
                {
                    if (!UserBLL.CheckMobile(Phone, 0))
                    {
                        errorMessage = "手机号码已经被注册";
                    }
                }
                if (errorMessage == string.Empty)
                {
                   
                    if (CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(Phone)) == null)
                    {
                        errorMessage = "验证码失效，请重新获取验证码";
                    }
                    else
                    {
                        string mobileCode = CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(Phone)).Value.ToString();
                        if (phoneCode.ToLower() != mobileCode.ToLower())
                        {
                            errorMessage = "验证码错误";
                        }
                        else
                        {
                            CookiesHelper.DeleteCookie("MobileCode" + StringHelper.AddSafe(Phone));
                        }
                    }
                }
            }
            else
            {//邮件验证 
                if (errorMessage == string.Empty)
                {
                    if (errorMessage == string.Empty)
                    {
                        if (!UserBLL.CheckEmail(email))
                        {
                            errorMessage = "Email已被注册";
                        }
                    }
                    if (safeCode.ToLower() != Cookies.Common.CheckCode.ToLower())
                    {
                        errorMessage = "验证码错误";
                    }
                }
            }
            //注册用户
            if (errorMessage == string.Empty)
            {
                UserInfo user = new UserInfo();
                user.UserName = userName;
                user.UserPassword = StringHelper.Password(userPassword1, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                user.Mobile = Phone;
                user.Email = email;
                user.RegisterIP = ClientHelper.IP;
                user.RegisterDate = RequestHelper.DateNow;
                user.LastLoginIP = ClientHelper.IP;
                user.LastLoginDate = RequestHelper.DateNow;
                user.FindDate = RequestHelper.DateNow;
                user.Sex = (int)SexType.Secret;
                if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
                {//短信验证，用户状态为已验证，可直接登录
                    user.Status = (int)UserStatus.Normal;
                }
                else
                {//邮件验证，用户状态为未验证，需登录邮件手动激活后再登录
                    user.Status = (int)UserStatus.NoCheck;
                }
                int userID = UserBLL.Add(user);
                if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
                {
                    //短信验证，直接登录
                    HttpCookie cookie = new HttpCookie(ShopConfig.ReadConfigInfo().UserCookies);
                    cookie["User"] = StringHelper.Encode(userName, ShopConfig.ReadConfigInfo().SecureKey);
                    cookie["Password"] = StringHelper.Encode(userPassword1, ShopConfig.ReadConfigInfo().SecureKey);
                    cookie["Key"] = StringHelper.Encode(ClientHelper.Agent, ShopConfig.ReadConfigInfo().SecureKey);
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    user = UserBLL.Read(userID);
                    UserBLL.UserLoginInit(user);
                    ResponseHelper.Redirect("/Mobile/User/Index.html");
                }
                else if (ShopConfig.ReadConfigInfo().RegisterCheck == 2)
                {
                    try
                    {
                        //邮件验证
                        string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/Mobile/User/ActiveUser.html?CheckCode=" + StringHelper.Encode(userID + "|" + email + "|" + userName, ShopConfig.ReadConfigInfo().SecureKey);
                        EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent("Register");
                        EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                        emailSendRecord.Title = emailContent.EmailTitle;
                        emailSendRecord.Content = emailContent.EmailContent.Replace("$UserName$", user.UserName).Replace("$Url$", url);
                        emailSendRecord.IsSystem = (int)BoolType.True;
                        emailSendRecord.EmailList = email;
                        emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                        emailSendRecord.SendStatus = (int)SendStatus.No;
                        emailSendRecord.AddDate = RequestHelper.DateNow;
                        emailSendRecord.SendDate = RequestHelper.DateNow;
                        emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                        EmailSendRecordBLL.SendEmail(emailSendRecord);
                        result = "恭喜您，注册成功，请登录邮箱激活！<a href=\"http://mail." + email.Substring(email.IndexOf("@") + 1) + "\"  target=\"_blank\">马上激活</a>";
                    }
                    catch (Exception ex)
                    {
                        ScriptHelper.AlertFront("激活邮件发送失败,请联系网站客服");
                    }
                }
                else
                {
                    //人工审核
                    result = "恭喜您，注册成功，请等待我们的审核！";
                }
                ResponseHelper.Redirect("/Mobile/User/Register.html?Result=" + Server.UrlEncode(result));
            }
            else
            {
                ScriptHelper.AlertFront(errorMessage);
                //ResponseHelper.Redirect("/Mobile/User/Register.html?ErrorMessage=" + Server.UrlEncode(errorMessage));
            }
        }

    }
}