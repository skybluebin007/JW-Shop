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
    public partial class FavorableActivityGift : JWShop.Page.AdminBasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
                CheckAdminPower( "ReadGift", PowerCheckType.Single);
                var searchInfo = new FavorableActivityGiftSearchInfo();
                searchInfo.Name = RequestHelper.GetQueryString<string>("Name");
                BindControl(FavorableActivityGiftBLL.SearchList(CurrentPage, PageSize, searchInfo, ref Count), RecordList, MyPager);   
			}
		}
	
		protected void DeleteButton_Click(object sender, EventArgs e)
		{
            CheckAdminPower( "DeleteGift", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                FavorableActivityGiftBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Gift"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
		}
	}
}