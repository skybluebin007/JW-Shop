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
    public class Login :CommonBasePage
    {
        /// <summary>
        /// 登录结果
        /// </summary>
        protected string result = string.Empty;
        /// <summary>
        /// 登录之后跳转的Url
        /// </summary>
        protected string redirectUrl = string.Empty;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
          
            topNav = 7;
            result = RequestHelper.GetQueryString<string>("Message");
            redirectUrl = HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("RedirectUrl"));

            if (UserBLL.Read(base.UserId).Id > 0)
            {
                if (redirectUrl != string.Empty)
                {
                    ResponseHelper.Redirect(redirectUrl);
                }
                else
                {
                    ResponseHelper.Redirect("/User/Index.html");
                }
            }            
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            redirectUrl = HttpUtility.UrlDecode(RequestHelper.GetForm<string>("RedirectUrl"));
            if (redirectUrl == string.Empty) redirectUrl = RequestHelper.GetQueryString<string>("RedirectUrl");
            string userName = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName"));
            string userPassword = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string autoLogin = StringHelper.SearchSafe(RequestHelper.GetForm<string>("autoLogin"));
            UserInfo user = UserBLL.Read(userName, userPassword);
            if (user.Id > 0)
            {
                switch (user.Status)
                {
                    case (int)UserStatus.NoCheck:
                        result = "该用户未激活";
                        break;
                    case (int)UserStatus.Frozen:
                        result = "该用户已冻结";
                        break;
                    case (int)UserStatus.Normal:
                        user = UserBLL.ReadUserMore(user.Id);
                        UserBLL.UserLoginInit(user);
                        //如果设置了自动登录则保存COOKIES一周
                        if (string.Equals(autoLogin, "1")) UserBLL.AddUserCookieWeekly(user);
                            //否则不保存(关闭浏览器即失效)
                        else UserBLL.AddUserCookie(user);

                        if (redirectUrl != string.Empty)
                        {
                            ResponseHelper.Redirect(redirectUrl);
                        }
                        else
                        {
                            ResponseHelper.Redirect("/User/Index.html");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                result = "用户名或者密码错误";
            }
            string url = "/User/Login.html?Message=" + result;
            if (redirectUrl != string.Empty)
            {
                url += "&RedirectUrl=" + redirectUrl;
            }
            ResponseHelper.Redirect(url);
        }        
    }
}