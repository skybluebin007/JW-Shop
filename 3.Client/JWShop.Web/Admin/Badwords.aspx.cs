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
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class Badwords : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadConfig", PowerCheckType.Single);
                txtBadwords.Text = BadwordFilterHelper.ReadBadwords();
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateConfig", PowerCheckType.Single);

            BadwordFilterHelper.UpdateBadwords(txtBadwords.Text);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateBadwordsConfig"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}
