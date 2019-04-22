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
    public partial class AdKeyword : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            CheckAdminPower("ReadAdKeyword", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Delete")
            {
                CheckAdminPower("DeleteAdKeyword", PowerCheckType.Single);
                int id = RequestHelper.GetQueryString<int>("Id");
                AdKeywordBLL.Delete(id);
            }

            var data = AdKeywordBLL.ReadList();
            BindControl(data, RecordList);
        }
    }
}