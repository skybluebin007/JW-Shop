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
    public partial class PointProduct : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadPointProduct", PowerCheckType.Single);

                string productName = RequestHelper.GetQueryString<string>("ProductName");
                DateTime beginDate = RequestHelper.GetQueryString<DateTime>("BeginDate");
                DateTime endDate = RequestHelper.GetQueryString<DateTime>("EndDate");

                ProductName.Text = productName;
                BeginDate.Text = beginDate == DateTime.MinValue ? "" : beginDate.ToString("yyyy-MM-dd");
                EndDate.Text = endDate == DateTime.MinValue ? "" : endDate.ToString("yyyy-MM-dd");

                PointProductSearchInfo searchPointProduct = new PointProductSearchInfo();
                searchPointProduct.ProductName = productName;
                searchPointProduct.BeginDate = beginDate;
                searchPointProduct.EndDate = endDate;
                List<PointProductInfo> pointProductInfoList = PointProductBLL.SearchList(CurrentPage, PageSize, searchPointProduct, ref Count);

                BindControl(pointProductInfoList, RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeletePointProduct", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                foreach (var id in deleteID.Split(','))
                {
                    PointProductBLL.Delete(int.Parse(id));
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("PointProduct"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "PointProduct.aspx?Action=search&";
            URL += "ProductName=" + ProductName.Text + "&";
            URL += "BeginDate=" + BeginDate.Text + "&";
            URL += "EndDate=" + EndDate.Text;
            ResponseHelper.Redirect(URL);
        }

    }
}