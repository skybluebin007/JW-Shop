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
    public class ChangeMobile : UserBasePage
    {
        protected string mobile;
        protected int wait = 0;

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
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

            string[] verify = StringHelper.Decode(CookiesHelper.ReadCookieValue("verify"), "sms").Split('|');
            if (verify.Length > 0) mobile = verify[0];

            Title = "修改已验证手机";
        }

        private void Submit()
        {
            string mobile = StringHelper.SearchSafe(RequestHelper.GetForm<string>("mobile"));
            string code = StringHelper.AddSafe(RequestHelper.GetForm<string>("code"));

            if (string.IsNullOrEmpty(mobile))
            {
                ResponseHelper.Write("error|请输入手机号码");
                ResponseHelper.End();
            }
            if (!UserBLL.UniqueUser(mobile, base.UserId))
            {
                ResponseHelper.Write("error|手机号码已被占用");
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(code))
            {
                ResponseHelper.Write("error|请输入短信验证码");
                ResponseHelper.End();
            }

            //短信验证码验证
            string[] verify = StringHelper.Decode(CookiesHelper.ReadCookieValue("verify"), "sms").Split('|');
            if (verify.Length != 2 || mobile != verify[0] || code != verify[1])
            {
                ResponseHelper.Write("error|您输入的短信验证码有误，请重新获取");
                ResponseHelper.End();
            }

            CurrentUser.Mobile = mobile;
            UserBLL.Update(CurrentUser);

            CookiesHelper.DeleteCookie("verify");
            ResponseHelper.Write("ok|手机修改成功");
            ResponseHelper.End();
        }
    }
}