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
    public partial class AdImage : JWShop.Page.AdminBasePage
    {
        protected int adType;
        protected int classId;
        protected FlashInfo theFlash = new FlashInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            CheckAdminPower("ReadAdImage", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Delete")
            {
                CheckAdminPower("DeleteAdImage", PowerCheckType.Single);
                int id = RequestHelper.GetQueryString<int>("Id");
                if (id > 0)
                {
                    AdImageBLL.Delete(id);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("FlashPhoto"), id);
                }
            }

            classId = RequestHelper.GetQueryString<int>("class_id");
            adType = RequestHelper.GetQueryString<int>("fp_type");

            if (adType > 0) theFlash = FlashBLL.Read(adType);
            var images = AdImageBLL.ReadList(adType);            

            BindControl(images, RecordList);
        }
    }
}