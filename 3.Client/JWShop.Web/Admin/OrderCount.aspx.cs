using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class OrderCount : JWShop.Page.AdminBasePage
    {
        protected string queryString = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsOrder", PowerCheckType.Single);
                ShopCommon.BindYearMonth(Year, Month);
                int year = RequestHelper.GetQueryString<int>("Year");
                year = (year == int.MinValue) ? DateTime.Now.Year : year;
                int month = RequestHelper.GetQueryString<int>("Month");
                Year.Text = year.ToString();
                Month.Text = month.ToString();
                queryString = "?Date=" + year.ToString() + "|" + month.ToString();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "OrderCount.aspx?Action=search&";
            URL += "Year=" + Year.Text + "&";
            URL += "Month=" + Month.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}