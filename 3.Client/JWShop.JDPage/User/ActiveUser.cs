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
    public class ActiveUser : CommonBasePage
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
                    string userName = decode.Split('|')[2];
                    UserInfo user = UserBLL.Read(userID);
                    if (user.Id > 0 && user.UserName==userName && user.Email==email)
                    {
                        if (user.Status == (int)UserStatus.NoCheck)
                        {
                            user.Status = (int)UserStatus.Normal;
                            UserBLL.Update(user);
                            result = "恭喜您，成功激活用户。请<a href=\"/user/Login.html\" > 登录</a>";
                        }
                        else
                        {
                            result = "该用户已经激活了。请<a href=\"/user/Login.html\" > 登录</a>";
                        }
                    }
                    else
                    {
                        result = "错误的激活信息";
                    }
                }
                else
                {
                    result = "错误的激活信息";
                }
            }
            else
            {
                result = "激活信息格式错误";
            }
        }
    }
}
