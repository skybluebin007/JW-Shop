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

namespace JWShop.Page
{
    public class ProductBookingAdd : CommonBasePage
    {       
        /// <summary>
        /// 商品名称
        /// </summary>
        protected string productName = string.Empty;
        /// <summary>
        /// 产品ID
        /// </summary>
        protected int productID = 0;
        /// <summary>
        /// 用户模型
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            productName = RequestHelper.GetQueryString<string>("ProductName");
            productID = RequestHelper.GetQueryString<int>("ProductID");
            user = UserBLL.Read(base.UserId);       

            Title = productName + "缺货登记";
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            BookingProductInfo bookingProduct = new BookingProductInfo();
            bookingProduct.ProductID = RequestHelper.GetForm<int>("ProductID");
            bookingProduct.ProductName =StringHelper.AddSafe(RequestHelper.GetForm<string>("ProductName"));
            bookingProduct.RelationUser =StringHelper.AddSafe( RequestHelper.GetForm<string>("RelationUser"));
            bookingProduct.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            bookingProduct.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
            bookingProduct.UserNote = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserNote"));
            bookingProduct.BookingDate = RequestHelper.DateNow;
            bookingProduct.BookingIP = ClientHelper.IP;
            bookingProduct.IsHandler = (int)BoolType.False;
            bookingProduct.HandlerDate = RequestHelper.DateNow;
            bookingProduct.HandlerAdminID = 0;
            bookingProduct.HandlerAdminName = string.Empty;
            bookingProduct.HandlerNote = string.Empty;
            bookingProduct.UserID = base.UserId;
            bookingProduct.UserName = base.UserName;
            int id = BookingProductBLL.AddBookingProduct(bookingProduct);
            ScriptHelper.AlertFront("登记成功", "/ProductDetail.html?ID=" + bookingProduct.ProductID.ToString());
        }
    }
}
