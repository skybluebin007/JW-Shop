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
    public partial class BookingProductAdd : JWShop.Page.AdminBasePage
    {
       
        protected BookingProductInfo bookingProduct = new BookingProductInfo();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int bookingProductID = RequestHelper.GetQueryString<int>("ID");
                if (bookingProductID != int.MinValue)
                {
                    CheckAdminPower( "ReadBookingProduct", PowerCheckType.Single);
                    bookingProduct = BookingProductBLL.ReadBookingProduct(bookingProductID,0);
                    HandlerNote.Text = bookingProduct.HandlerNote;
                    IsHandler.Text = bookingProduct.IsHandler.ToString();
                }
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            BookingProductInfo bookingProduct = new BookingProductInfo();
            bookingProduct.ID = RequestHelper.GetQueryString<int>("ID");
            bookingProduct.IsHandler = Convert.ToInt32(IsHandler.Text); ;
            bookingProduct.HandlerDate = RequestHelper.DateNow;
            bookingProduct.HandlerAdminID = Cookies.Admin.GetAdminID(false);
            bookingProduct.HandlerAdminName = Cookies.Admin.GetAdminName(false);
            bookingProduct.HandlerNote = HandlerNote.Text;
            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            CheckAdminPower( "UpdateBookingProduct", PowerCheckType.Single);
            BookingProductBLL.UpdateBookingProduct(bookingProduct);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("BookingProduct"), bookingProduct.ID);
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}