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
    public partial class PasswordAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateAdmin", PowerCheckType.Single);
            int tempAdminID = RequestHelper.GetQueryString<int>("ID");
            if (tempAdminID != int.MinValue)
            {
                Name.Text = AdminBLL.Read(tempAdminID).Name;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs E)
        {
            int tempAdminID = RequestHelper.GetQueryString<int>("ID");
            if (tempAdminID != int.MinValue)
            {
                string newPassword = StringHelper.Password(NewPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                AdminBLL.ChangePassword(tempAdminID, newPassword);
                Task.Run(() => {
                    //安全码
                    ShopConfigInfo config = ShopConfig.ReadConfigInfo();
                    config.SecureKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    ShopConfig.UpdateConfigInfo(config);
                });
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("ChangeAdminPassword"), tempAdminID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
            }
        }
    }
}