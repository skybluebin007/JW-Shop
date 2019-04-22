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
using Newtonsoft.Json;

namespace JWShop.Page
{
    public class UpdateMobile : UserBasePage
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        protected UserInfo user = new UserInfo();

        protected int wait = 0;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.Read(base.UserId);
            //userGradeName = UserGradeBLL.Read(base.GradeID).Name;

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

            if (RequestHelper.GetQueryString<string>("Action") == "UpdateMobile")
            {
                UpdateUserMobile();
            }
        }
        /// <summary>
        /// 修改手机号码
        /// </summary>
        protected void UpdateUserMobile()
        {
            string msg = string.Empty;
            try
            {
                UserInfo user = UserBLL.Read(base.UserId);
                user.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
                string safeCode = RequestHelper.GetForm<string>("phoneVer");
                //手机短信验证码
                if (CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(user.Mobile)) == null)
                {
                    msg = "error|校验码失效，请重新获取";
                }
                else
                {
                    string mobileCode = CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(user.Mobile)).Value.ToString();
                    if (safeCode.ToLower() != mobileCode.ToLower())
                    {
                        msg = "error|校验码错误";
                    }
                    else
                    {
                        CookiesHelper.DeleteCookie("MobileCode" + StringHelper.AddSafe(user.Mobile));
                    }
                }
              if(msg==string.Empty){
                if (string.IsNullOrEmpty(user.Mobile))
                {
                    msg = "error|请填写手机号码";
                }
                if (!ShopCommon.CheckMobile(user.Mobile))
                {
                    msg = "error|手机号码错误";
                }
                if (!UserBLL.CheckMobile(user.Mobile, base.UserId))
                {
                    msg = "error|手机号码已被其他会员注册";
                }
                else
                {
                    UserBLL.Update(user);
                    msg = "ok|修改成功";
                }
              }
                Response.Clear();
                Response.Write(msg);
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write("error|系统忙，请稍后重试");
            }
            finally {
                Response.End();
            }
        }
    }
}
