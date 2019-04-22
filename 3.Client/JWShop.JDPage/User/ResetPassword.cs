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
    public class ResetPassword : CommonBasePage
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 结果
        /// </summary>
        protected string result = string.Empty;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            string checkCode = RequestHelper.GetQueryString<string>("CheckCode");
            if (checkCode != string.Empty)
            {
                string decode = StringHelper.Decode(checkCode, ShopConfig.ReadConfigInfo().SecureKey);
                if (decode.IndexOf('|') > 0)
                {
                    int userID = Convert.ToInt32(decode.Split('|')[0]);
                    string email = decode.Split('|')[1];
                    string loginName = decode.Split('|')[2];
                    string userMobile = decode.Split('|')[3];
                    string safeCode = decode.Split('|')[4];
                    UserInfo user = UserBLL.ReadUserMore(userID);
                    if (user.Id > 0 && (user.UserName == loginName || user.Email == loginName || user.Mobile == loginName) && user.Email == email && user.Mobile == userMobile && safeCode == user.SafeCode)
                    {
                        if (ShopConfig.ReadConfigInfo().FindPasswordTimeRestrict > 0 && user.FindDate.AddHours(ShopConfig.ReadConfigInfo().FindPasswordTimeRestrict) < RequestHelper.DateNow)
                        {
                            errorMessage = "信息过时，请重新申请";
                        }
                    }
                    else
                    {
                        errorMessage = "错误的信息";
                    }
                }
                else
                {
                    errorMessage = "错误的信息";
                }
            }
            result = RequestHelper.GetQueryString<string>("Result");

            Title = "找回密码";
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            string userPassword = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword1"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string checkCode = RequestHelper.GetForm<string>("CheckCode");
            string decode = StringHelper.Decode(checkCode, ShopConfig.ReadConfigInfo().SecureKey);
            int userID = Convert.ToInt32(decode.Split('|')[0]);

            UserBLL.ChangePassword(userID, userPassword);
            UserBLL.ChangeUserSafeCode(userID, string.Empty, RequestHelper.DateNow);
            result = "恭喜您，密码修改成功！" + "&nbsp;&nbsp;点击<a href=\"/user/Login.html\" style=\"color: #1d7fd4\">\"使用新密码登录\"</a>";
            //清除原有的user Cookies
            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().UserCookies);

            ResponseHelper.Redirect("/User/ResetPassword.html?Result=" + Server.UrlEncode(result));
        }
    }
}

