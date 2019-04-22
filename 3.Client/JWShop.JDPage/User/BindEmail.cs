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
using System.Text.RegularExpressions;

namespace JWShop.Page
{
   public class BindEmail:UserBasePage
    {
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
                   
                    string safeCode = decode.Split('|')[2];
                    UserInfo user = UserBLL.ReadUserMore(userID);
                    if (user.Id > 0 && safeCode == user.SafeCode)
                    {
                        if (ShopConfig.ReadConfigInfo().BindEmailTime > 0 && user.FindDate.AddHours(ShopConfig.ReadConfigInfo().BindEmailTime) < RequestHelper.DateNow)
                        {
                            result = "信息过时，<a href=\"/user/UpdateEmail.html\">请重新申请验证邮箱</a>";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(email))
                            {
                                result = "错误的信息";
                            }
                            else if (!new Regex("^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3}){1,2})$").IsMatch(email))
                            {
                                result = "错误的信息";
                            }
                            else
                            {
                                if (!UserBLL.CheckEmail(email, user.Id))
                                {
                                    result = "此邮箱已被其他会员绑定";
                                }
                                else
                                {
                                    user.Email = email;
                                    UserBLL.Update(user);
                                    CookiesHelper.AddCookie("UserEmail", user.Email);
                                    result = "恭喜您，邮箱验证成功！您可进入<a href=\"/user/useradd.html\">个人信息</a>查看刚才绑定的邮箱";
                                }
                            }
                           
                        }
                    }
                    else
                    {
                        result = "错误的信息";
                    }
                }
                else
                {
                    result = "错误的信息";
                }
            }
           
        }
    }
}
