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

namespace JWShop.Page.Lab
{
    public class Login : CommonBasePage
    {
        protected string cookieUserName;
        /// <summary>
        /// 登录之后跳转的Url
        /// </summary>
        protected string redirectUrl = string.Empty;
        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Submit" && Request.HttpMethod == "POST") this.Submit();

            redirectUrl = RequestHelper.GetQueryString<string>("RedirectUrl");
            if (base.UserId > 0) ResponseHelper.Redirect(string.IsNullOrEmpty(redirectUrl) ? "/user/index.html" : redirectUrl);

            var auto = CookiesHelper.ReadCookie("auto");
            cookieUserName = auto != null ? auto.Value : "";
        }

        private void Submit()
        {
            redirectUrl = RequestHelper.GetQueryString<string>("RedirectUrl");
            string userName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("name"));
            string userNoEncryptPassword = RequestHelper.GetForm<string>("password");
            string userPassword = StringHelper.Password(userNoEncryptPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);

            string[] urlArr = Request.RawUrl.Split('/');
            UserInfo user = UserBLL.Read(userName);
            if (user.Id > 0)
            {
                if (user.UserPassword != userPassword)
                {
                    ResponseHelper.Write("error|用户名或者密码错误");
                    ResponseHelper.End();
                }

                switch (user.Status)
                {
                    case (int)UserStatus.NoCheck:
                        ResponseHelper.Write("error|该用户未激活");
                        ResponseHelper.End();
                        break;
                    case (int)UserStatus.Frozen:
                        ResponseHelper.Write("error|该用户已冻结");
                        ResponseHelper.End();
                        break;
                    default:
                        break;
                }
            }            

            UserBLL.UserLoginInit(user);            
            
            //记住用户名
            if (!string.IsNullOrEmpty(Request.Form["autologin"]))
            {
                CookiesHelper.AddCookie("auto", user.UserName.ToString(), 7, TimeType.Day);
            }
            else CookiesHelper.DeleteCookie("auto");

            if (string.IsNullOrEmpty(redirectUrl))
            {
                if (urlArr[urlArr.Length - 2].ToLower() == "mobile")
                {
                    redirectUrl = "/Mobile/Index.aspx";
                }
                else
                {
                    redirectUrl = "/user/index.html";
                }
            }

            ResponseHelper.Write("ok|登录成功|" + redirectUrl);
            ResponseHelper.End();
        }

    }
}