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
    public partial class ThemeActivity : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadThemeActivity", PowerCheckType.Single);

                var themes = ThemeActivityBLL.ReadList();
                themes = themes.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                BindControl(themes, RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteThemeActivity", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                ThemeActivityBLL.Delete(Array.ConvertAll<string, int>(deleteID.Split(','), k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ThemeActivity"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected string GetUrl(object activityId)
        {
            return string.Concat(new object[] { "http://", HttpContext.Current.Request.Url.Host, (HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : ""), "/ThemeActivityDetail.html?ID=", activityId });

           
        }
    }
}