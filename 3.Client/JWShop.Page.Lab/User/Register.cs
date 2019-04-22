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

namespace JWShop.Page.Lab
{
    public class Register : CommonBasePage
    {
        protected int wait = 0;

        protected override void PageLoad()
        {
            base.PageLoad();

            string redirectUrl = RequestHelper.GetQueryString<string>("RedirectUrl");
            if (base.UserId > 0) ResponseHelper.Redirect(string.IsNullOrEmpty(redirectUrl) ? "/user/index.html" : redirectUrl);

            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "CheckMobile":
                    CheckMobile();
                    break;
                case "Submit":
                    if (Request.HttpMethod == "POST") Submit();
                    break;
            }

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
        }

        private void CheckMobile()
        {
            string mobile = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("value"));

            if (!UserBLL.UniqueUser(mobile))
            {
                ResponseHelper.Write(JsonConvert.SerializeObject(new { error = "手机号码已被占用" }));
                ResponseHelper.End();
            }

            ResponseHelper.Write(JsonConvert.SerializeObject(new { ok = "" }));
            ResponseHelper.End();
        }

        private void Submit()
        {
            string userName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("name"));
            string userPassword = StringHelper.Password(RequestHelper.GetForm<string>("password"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string code = StringHelper.AddSafe(RequestHelper.GetForm<string>("code"));

            #region validate
            if (string.IsNullOrEmpty(userName))
            {
                ResponseHelper.Write("error|请输入手机号码");
                ResponseHelper.End();
            }
            if (!UserBLL.UniqueUser(userName))
            {
                ResponseHelper.Write("error|手机号码已被占用");
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(userPassword))
            {
                ResponseHelper.Write("error|请输入密码");
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(code))
            {
                ResponseHelper.Write("error|请输入短信验证码");
                ResponseHelper.End();
            }

            //webservice validate
            //如果是图楼会员卡用户，可以直接登录
            string msg;
            bool isSuccess = false;// WebService.Member.IsMember(userName, out msg);
            if (isSuccess)
            {
                ResponseHelper.Write("error|尊敬的图楼会员卡用户，您拥有免注册特权，请直接登录");
                ResponseHelper.End();
            }
            #endregion

            UserInfo user = new UserInfo();
            user.UserName = userName;
            user.Mobile = userName;
            user.UserPassword = userPassword;
            user.UserType = (int)UserType.Member;
            user.Status = (int)UserStatus.Normal;

            //短信验证码验证
            string[] verify = StringHelper.Decode(CookiesHelper.ReadCookieValue("verify"), "sms").Split('|');
            if (verify.Length != 2 || userName != verify[0] || code != verify[1])
            {
                ResponseHelper.Write("error|您输入的短信验证码有误，请重新获取");
                ResponseHelper.End();
            }

            string redirectUrl = RequestHelper.GetQueryString<string>("RedirectUrl");
            if (string.IsNullOrEmpty(redirectUrl)) redirectUrl = "/user/index.html";

            int id = UserBLL.Add(user);
            if (id < 0)
            {
                ResponseHelper.Write("error|注册失败");
                ResponseHelper.End();
            }

            user = UserBLL.Read(id);
            UserBLL.UserLoginInit(user);
            ResponseHelper.Write("ok|注册成功|" + redirectUrl);
            ResponseHelper.End();
        }

    }
}