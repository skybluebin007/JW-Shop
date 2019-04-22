using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;

namespace JWShop.Page.Admin
{
   public class ResetPassword:CommonBasePage
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 结果
        /// </summary>
        protected string result = string.Empty;
        protected override void PageLoad()
        {
            base.PageLoad();
           
                string checkCode = RequestHelper.GetQueryString<string>("CheckCode");
                if (checkCode != string.Empty)
                {
                    string decode = StringHelper.Decode(checkCode, ShopConfig.ReadConfigInfo().SecureKey);
                    if (decode.IndexOf('|') > 0)
                    {
                        int adminID = Convert.ToInt32(decode.Split('|')[0]);
                        string email = decode.Split('|')[1];
                        string loginName = decode.Split('|')[2];
                        string safeCode = decode.Split('|')[3];
                        AdminInfo admin = AdminBLL.Read(adminID);
                        if (admin.Id > 0 && admin.Name == loginName && admin.Email == email && safeCode == admin.SafeCode)
                        {
                            //邮件链接15分钟失效
                            if (admin.FindDate.AddMinutes(15) < RequestHelper.DateNow)
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
            
        }

        protected override void PostBack()
        {
            string newPassword = RequestHelper.GetForm<string>("NewPassword");
            string newPassword2 = RequestHelper.GetForm<string>("NewPassword2");
            //如果账号不存在
            if (!string.Equals(newPassword, newPassword2, StringComparison.OrdinalIgnoreCase))
            {
                ScriptHelper.AlertFront("两次密码不一致");
            }
            else
            {
               // 验证通过,重置密码

                    string checkCode = RequestHelper.GetForm<string>("CheckCode");
                    string decode = StringHelper.Decode(checkCode, ShopConfig.ReadConfigInfo().SecureKey);
                    int adminID = Convert.ToInt32(decode.Split('|')[0]);
                    newPassword = StringHelper.Password(newPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                    // 重置密码
                    AdminBLL.ChangePassword(adminID, newPassword);
                    //清空safecode，finddate恢复
                    AdminBLL.ChangeAdminSafeCode(adminID, string.Empty, RequestHelper.DateNow);
                    //错误次数清零，解锁
                    AdminBLL.UpdateStatus(adminID);
                    string msg = "ok";
                    //清除原有的user Cookies
                    CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);
                    CookiesHelper.DeleteCookie("AdminSign");

                    ResponseHelper.Redirect("/mobileadmin/ResetPassword.html?Result=" + Server.UrlEncode(msg));

                

            }
        }
    }
}
