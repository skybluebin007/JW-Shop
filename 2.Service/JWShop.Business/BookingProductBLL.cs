using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
	/// <summary>
	/// 缺货登记业务逻辑。
	/// </summary>
	public sealed class BookingProductBLL
	{
        
		private static readonly IBookingProduct dal = FactoryHelper.Instance<IBookingProduct>(Global.DataProvider,"BookingProductDAL");

        /// <summary>
        /// 增加一条缺货登记数据
        /// </summary>
        /// <param name="bookingProduct">缺货登记模型变量</param>
        public static int AddBookingProduct(BookingProductInfo bookingProduct)
        {
            bookingProduct.ID = dal.AddBookingProduct(bookingProduct);
            return bookingProduct.ID;
        }

		/// <summary>
		/// 更新一条缺货登记数据
		/// </summary>
		/// <param name="bookingProduct">缺货登记模型变量</param>
		public static void UpdateBookingProduct(BookingProductInfo bookingProduct)
		{
			dal.UpdateBookingProduct(bookingProduct);
		} 

		/// <summary>
		/// 删除多条缺货登记数据
		/// </summary>
		/// <param name="strID">缺货登记的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		public static void DeleteBookingProduct(string strID,int userID)
		{
			if (userID != 0)
			{
				strID=dal.ReadBookingProductIDList(strID,userID);
			}
			dal.DeleteBookingProduct(strID,userID);
		}




		/// <summary>
		/// 读取一条缺货登记数据
		/// </summary>
		/// <param name="id">缺货登记的主键值</param>
		/// <param name="userID">用户ID</param>
		/// <returns>缺货登记数据模型</returns>
		public static BookingProductInfo ReadBookingProduct(int id,int userID)
		{
			return dal.ReadBookingProduct(id,userID);
		}


        /// <summary>
        /// 搜索缺货登记数据列表
        /// </summary>
        /// <param name="bookingProduct">BookingProductSearchInfo模型变量</param>
        /// <returns>缺货登记数据列表</returns>
        public static List<BookingProductInfo> SearchBookingProductList(BookingProductSearchInfo bookingProduct)
        {
            return dal.SearchBookingProductList(bookingProduct);
        }

        /// <summary>
        /// 搜索缺货登记数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="bookingProduct">BookingProductSearchInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>缺货登记数据列表</returns>
        public static List<BookingProductInfo> SearchBookingProductList(int currentPage, int pageSize, BookingProductSearchInfo bookingProduct, ref int count)
        {
            return dal.SearchBookingProductList(currentPage, pageSize, bookingProduct, ref count);
        }
	}
}