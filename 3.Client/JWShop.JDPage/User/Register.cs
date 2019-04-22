using System;
using System.Data;
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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace JWShop.Page
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
            result = RequestHelper.GetQueryString<string>("Result");
            errorMessage = RequestHelper.GetQueryString<string>("ErrorMessage");
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


            if (RequestHelper.GetQueryString<string>("Action") == "Register")
            {
                PostBack();
            }
            Title = "会员注册";
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected void PostBack()
        {
            string userName = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName")));
            string email = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("Email")));
            string userPassword1 = RequestHelper.GetForm<string>("UserPassword1");
            string userPassword2 = RequestHelper.GetForm<string>("UserPassword2");
            string Mobile = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("Phone")));
            string commendUser = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("CommendUser")));
            string phoneCode = RequestHelper.GetForm<string>("phoneVer");
            string safeCode = RequestHelper.GetForm<string>("SafeCode");
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
                    errorMessage = "用户名已经被注册";
                }
            }
            
            //if (errorMessage == string.Empty)
            //{
            //    Regex rg = new Regex("^([a-zA-Z0-9_\u4E00-\u9FA5])+$");
            //    if (!rg.IsMatch(userName))
            //    {
            //        errorMessage = "用户名只能包含字母、数字、下划线、中文";
            //    }
            //}
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
            #region 检查手机 邮箱 验证码
                if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
                {//短信验证
                    if (errorMessage == string.Empty)
                    {

                        if (!ShopCommon.CheckMobile(Mobile))
                        {
                            errorMessage = "手机号码错误";
                        }
                    }
                    if (errorMessage == string.Empty)
                    {
                        if (!UserBLL.CheckMobile(Mobile, 0))
                        {
                            errorMessage = "手机号码已经被注册";
                        }
                    }

                    //手机短信验证码
                    if (errorMessage == string.Empty)
                    {
                        if (CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(Mobile)) == null)
                        {
                            errorMessage = "校验码失效，请重新获取";
                        }
                        else
                        {
                            string mobileCode = CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(Mobile)).Value.ToString();
                            if (phoneCode.ToLower() != mobileCode.ToLower())
                            {
                                errorMessage = "校验码错误";
                            }
                            else
                            {//验证通过后即清除校验码Cookies
                                CookiesHelper.DeleteCookie("MobileCode" + StringHelper.AddSafe(Mobile));
                            }
                        }
                    }
                }
                else
                {//邮件认证
                    if (errorMessage == string.Empty)
                    {
                        if (!UserBLL.CheckEmail(email)) {
                            errorMessage = "Email已被注册";
                        }
                    }
                    if (errorMessage == string.Empty)
                    {
                        //普通验证码
                        if (safeCode.ToLower() != Cookies.Common.CheckCode.ToLower())
                        {
                            errorMessage = "验证码错误";
                        }
                    }
                }
           
            #endregion
            string[] urlArr = Request.RawUrl.Split('/');

            //注册用户
            if (errorMessage == string.Empty)
            {
                UserInfo user = new UserInfo();
                user.UserName = userName;
                user.UserPassword = StringHelper.Password(userPassword1, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                user.Email = email;
                user.Mobile = Mobile;
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
                #region 纪录推荐用户
                //if (commendUser != string.Empty)
                //{
                //    UserInfo comUser = UserBLL.ReadUserByUserName(commendUser);
                //    if (comUser.ID > 0)
                //    {
                //        user.CommendUserID = comUser.ID;
                //    }
                //}
                //user.HasRepayed = 0;
                #endregion
                int userID = UserBLL.Add(user);


                if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
                {
                    //短信验证，直接登录                 
                        HttpCookie cookie = new HttpCookie(ShopConfig.ReadConfigInfo().UserCookies);
                        cookie["User"] = StringHelper.Encode(userName, ShopConfig.ReadConfigInfo().SecureKey);
                        cookie["Password"] = StringHelper.Encode(userPassword1, ShopConfig.ReadConfigInfo().SecureKey);
                        cookie["Key"] = StringHelper.Encode(ClientHelper.Agent, ShopConfig.ReadConfigInfo().SecureKey);
                        HttpContext.Current.Response.Cookies.Add(cookie);

                        user = UserBLL.ReadUserMore(userID);
                        UserBLL.UserLoginInit(user);
                        //UserBLL.(user);
                        if (urlArr[urlArr.Length - 2].ToLower() == "mobile")
                        {
                            // ResponseHelper.Redirect("/Mobile/Index.aspx");  
                            Response.Write("ok|/Mobile/Index.html");
                            Response.End();
                        }
                        else
                        {
                            //  ResponseHelper.Redirect("/User/Index.aspx"); 
                            Response.Write("ok|/User/Index.html");
                            Response.End();
                        }
                }
                else if (ShopConfig.ReadConfigInfo().RegisterCheck == 2)
                {
                    try
                    {
                        //邮件验证,发送验证邮件
                        string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/User/ActiveUser.aspx?CheckCode=" + StringHelper.Encode(userID + "|" + email + "|" + userName, ShopConfig.ReadConfigInfo().SecureKey);
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
                        Response.Write("ok|" + result);
                       
                    }
                    catch (Exception ex)
                    {
                        Response.Write("error|激活邮件发送失败,请联系网站客服");
                    }
                    finally {
                        Response.End();
                    }
                }
                else
                {
                    //人工审核
                    result = "恭喜您，注册成功，请等待我们的审核！";
                    Response.Write("ok|" + result);
                    Response.End();
                }

                if (urlArr[urlArr.Length - 2].ToLower() == "mobile")
                {
                    ResponseHelper.Redirect("/Mobile/Register.aspx?Result=" + Server.UrlEncode(result));
                }
                else
                {
                    ResponseHelper.Redirect("/User/Register.aspx?Result=" + Server.UrlEncode(result));
                }
            }
            else
            {
                Response.Write("error|" + errorMessage);
                Response.End();
                //if (urlArr[urlArr.Length - 2].ToLower() == "mobile")
                //{
                //    ResponseHelper.Redirect("/Mobile/Register.aspx?ErrorMessage=" + Server.UrlEncode(errorMessage));
                //}
                //else
                //{
                //    ResponseHelper.Redirect("/User/Register.aspx?ErrorMessage=" + Server.UrlEncode(errorMessage));
                //}                
            }
        }




    }
}
