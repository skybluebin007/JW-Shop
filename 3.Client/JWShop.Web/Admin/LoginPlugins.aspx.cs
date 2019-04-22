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
    public partial class LoginPlugins : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadPay", PowerCheckType.Single);
                BindControl(JWShop.Common.LoginPlugins.ReadLoginPluginsList(), RecordList);
            }
        }
    }
}