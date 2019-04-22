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

namespace JWShop.Web.Admin
{
    public partial class UserPasswordAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateUser", PowerCheckType.Single);
            int userID = RequestHelper.GetQueryString<int>("ID");
            if (userID != int.MinValue)
            {
                Name.Text = UserBLL.Read(userID).UserName;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs E)
        {
            int userID = RequestHelper.GetQueryString<int>("ID");
            if (userID != int.MinValue)
            {
                string newPassword = StringHelper.Password(NewPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                UserBLL.ChangePassword(userID, newPassword);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("ChangeUserPassword"), userID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
            }
        }
    }
}