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
using System.Threading.Tasks;

namespace JWShop.Web.Admin
{
    public partial class ChangePassword : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("UpdatePassword", PowerCheckType.Single);
            Name.Text = Cookies.Admin.GetAdminName(false);
        }

        protected void SubmitButton_Click(object sender, EventArgs E)
        {
            string oldPassword = StringHelper.Password(Password.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword = StringHelper.Password(NewPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            AdminInfo admin = AdminBLL.Read(Cookies.Admin.GetAdminID(false));
            if (admin.Password == oldPassword)
            {
                AdminBLL.ChangePassword(Cookies.Admin.GetAdminID(false), oldPassword, newPassword);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("ChangePassword"));
                Task.Run(() => {
                    //安全码
                    ShopConfigInfo config = ShopConfig.ReadConfigInfo();
                    config.SecureKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    ShopConfig.UpdateConfigInfo(config);
                });
                //清除现有cookie
                CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
            }
            else
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OldPasswordError"), RequestHelper.RawUrl);
            }
        }
    }
}