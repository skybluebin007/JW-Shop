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
    public partial class FavorableActivity : JWShop.Page.AdminBasePage
    {
        //活动状态 默认 ：进行中
        protected int timePeriod=2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadFavorableActivity", PowerCheckType.Single);
                timePeriod = RequestHelper.GetQueryString<int>("timePeriod") < 1 ? 2 : RequestHelper.GetQueryString<int>("timePeriod");
                //BindControl(FavorableActivityBLL.ReadList(CurrentPage, PageSize, ref Count), RecordList, MyPager);
                BindControl(FavorableActivityBLL.ReadList(CurrentPage, PageSize, ref Count,timePeriod), RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteFavorableActivity", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                FavorableActivityBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("FavorableActivity"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
    }
}