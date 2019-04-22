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
    public partial class AdminGroup : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadAdminGroup", PowerCheckType.Single);
                BindControl(AdminGroupBLL.ReadList(), RecordList);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteAdminGroup", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                AdminGroupBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("AdminGroup"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
    }
}