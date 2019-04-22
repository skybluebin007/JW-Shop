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
    public partial class Flash : JWShop.Page.AdminBasePage
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!Page.IsPostBack)
			{
                CheckAdminPower("ReadFlash", PowerCheckType.Single);
                string action = RequestHelper.GetQueryString<string>("Action");
                if (action == "Delete")
                {
                    CheckAdminPower("DeleteFlash", PowerCheckType.Single);
                    int id = RequestHelper.GetQueryString<int>("Id");
                    if (id > 0)
                    {
                        FlashBLL.Delete(id);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Flash"), id);
                        foreach (var adImage in AdImageBLL.ReadList(id)) {
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("FlashPhoto"), adImage.Id);
                        }
                        AdImageBLL.DeleteByAdType(id);
                        //ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
                    }
                }

                BindControl(FlashBLL.SearchList(CurrentPage,PageSize,ref Count), RecordList, MyPager);   
			}        
        }
    }
}