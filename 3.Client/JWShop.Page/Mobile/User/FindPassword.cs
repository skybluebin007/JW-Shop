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

namespace JWShop.Page.Mobile
{
    public class FindPassword : CommonBasePage
    {
        protected int step = 1;
        protected string errorMsg = "";
        protected string code = "";
        protected UserInfo user = new UserInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 12;

            step = RequestHelper.GetQueryString<int>("step");
            if (step < 1) step = 1;
            errorMsg = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("msg"));
            
            string userName = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("u"));
            if(!string.IsNullOrEmpty(userName)) user = UserBLL.Read(userName);

            if (step == 3)
            {
                string[] verify = StringHelper.Decode(CookiesHelper.ReadCookieValue("verify"), "sms").Split('|');
                code = verify.Length > 1 ? verify[1] : "";
            }

            Title = "找回密码";
        }

        protected override void PostBack()
        {
            step = RequestHelper.GetForm<int>("step");
            string userName = StringHelper.AddSafe(RequestHelper.GetForm<string>("name"));
            if (string.IsNullOrEmpty(userName))
            {
                ResponseHelper.Redirect("/mobile/user/findpassword.html?step=1&msg=您输入的账户名不存在，请重新输入。");
            }

            user = UserBLL.Read(userName);
            if (user.Id < 1)
            {
                ResponseHelper.Redirect("/mobile/user/findpassword.html?step=1&msg=您输入的账户名不存在，请重新输入。");
            }

            //提交“填写帐户名”步骤
            if (step <= 1)
            {
                ResponseHelper.Redirect("/mobile/user/findpassword.html?step=2&u=" + user.UserName);
            }
            //提交“验证身份”步骤
            if (step == 2)
            {
                string code = StringHelper.AddSafe(RequestHelper.GetForm<string>("code"));
                string[] verify = StringHelper.Decode(CookiesHelper.ReadCookieValue("verify"), "sms").Split('|');
                if (verify.Length != 2 || userName != verify[0] || code != verify[1])
                {
                    ResponseHelper.Redirect("/mobile/user/findpassword.html?step=2&u=" + user.UserName + "&msg=您输入的短信验证码有误，请重新获取。");
                }

                ResponseHelper.Redirect("/mobile/user/findpassword.html?step=3&u=" + user.UserName);
            }
            //提交“设置新密码”步骤
            if (step == 3)
            {
                string code = StringHelper.AddSafe(RequestHelper.GetForm<string>("code"));
                string[] verify = StringHelper.Decode(CookiesHelper.ReadCookieValue("verify"), "sms").Split('|');
                if (verify.Length != 2 || userName != verify[0] || code != verify[1])
                {
                    ResponseHelper.Redirect("/mobile/user/findpassword.html?step=2&u=" + user.UserName + "&msg=您输入的短信验证码有误，请重新获取。");
                }

                string newPassword = StringHelper.Password(RequestHelper.GetForm<string>("password"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                UserBLL.ChangePassword(user.Id, newPassword);
                ResponseHelper.Redirect("/mobile/user/findpassword.html?step=4");
            }

        }

    }
}