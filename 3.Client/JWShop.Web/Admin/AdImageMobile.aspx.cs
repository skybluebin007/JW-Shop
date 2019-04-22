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
    public partial class AdImageMobile : JWShop.Page.AdminBasePage
    {
        protected int adType;
        protected int classId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            CheckAdminPower("ReadAdImage", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Delete")
            {
                CheckAdminPower("DeleteAdImage", PowerCheckType.Single);
                int id = RequestHelper.GetQueryString<int>("Id");
                AdImageBLL.Delete(id);
            }

            classId = RequestHelper.GetQueryString<int>("class_id");
            adType = RequestHelper.GetQueryString<int>("ad_type");

            var images = AdImageBLL.ReadList(adType);
            if (adType == (int)AdImageType.MobileFloorClass)
            {
                if (classId < 1)
                {
                    var rootList = ProductClassBLL.ReadRootList();
                    if (rootList != null && rootList.Count > 0) classId = rootList[0].Id;
                }
                images = images.Where(k => k.ClassId == classId).ToList();
            }

            BindControl(images, RecordList);
        }
    }
}