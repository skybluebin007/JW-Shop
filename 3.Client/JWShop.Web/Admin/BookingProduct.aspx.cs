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
    public partial class BookingProduct : JWShop.Page.AdminBasePage
	{
		/// <summary>
		/// 页面加载方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
                CheckAdminPower( "ReadBookingProduct", PowerCheckType.Single);
                RelationUser.Text = RequestHelper.GetQueryString<string>("RelationUser");
                ProductName.Text = RequestHelper.GetQueryString<string>("ProductName");
                IsHandler.Text = RequestHelper.GetQueryString<string>("IsHandler");        
                List<BookingProductInfo> bookingProductList = new List<BookingProductInfo>();
                BookingProductSearchInfo bookingProductSearch = new BookingProductSearchInfo();
                bookingProductSearch.RelationUser = RequestHelper.GetQueryString<string>("RelationUser");
                bookingProductSearch.ProductName = RequestHelper.GetQueryString<string>("ProductName");
                bookingProductSearch.IsHandler = RequestHelper.GetQueryString<int>("IsHandler");
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                bookingProductList = BookingProductBLL.SearchBookingProductList(CurrentPage, PageSize, bookingProductSearch, ref Count);
                BindControl(bookingProductList, RecordList, MyPager);   
			}
		}
	
		/// <summary>
		/// 删除按钮点击方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			CheckAdminPower( "DeleteBookingProduct", PowerCheckType.Single);
			string deleteID = RequestHelper.GetIntsForm("SelectID");
			if(deleteID != string.Empty)
			{
				BookingProductBLL.DeleteBookingProduct(deleteID,0);
				AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("BookingProduct"), deleteID);
				ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
			}
		}
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "BookingProduct.aspx?Action=search&";
            URL += "RelationUser=" + RelationUser.Text + "&";
            URL += "ProductName=" + ProductName.Text + "&";
            URL += "IsHandler=" + IsHandler.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "BookingProduct.aspx?Action=search&";
            URL += "RelationUser=" + RelationUser.Text + "&";
            URL += "ProductName=" + ProductName.Text + "&";
            URL += "IsHandler=" + IsHandler.Text;
            ResponseHelper.Redirect(URL);
        }
	}
}