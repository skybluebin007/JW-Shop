using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 缺货登记接口层说明。
	/// </summary>
	public interface IBookingProduct
	{
		/// <summary>
		/// 增加一条缺货登记数据
		/// </summary>
		/// <param name="bookingProduct">缺货登记模型变量</param>
		int AddBookingProduct(BookingProductInfo bookingProduct);

		/// <summary>
		/// 更新一条缺货登记数据
		/// </summary>
		/// <param name="bookingProduct">缺货登记模型变量</param>
		void UpdateBookingProduct(BookingProductInfo bookingProduct);	 

		/// <summary>
		/// 删除多条缺货登记数据
		/// </summary>
		/// <param name="strID">缺货登记的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		void DeleteBookingProduct(string strID,int userID);




		/// <summary>
		/// 读取一条缺货登记数据
		/// </summary>
		/// <param name="id">缺货登记的主键值</param>
		/// <param name="userID">用户ID</param>
		/// <returns>缺货登记数据模型</returns>
		BookingProductInfo ReadBookingProduct(int id,int userID);

		/// <summary>
		/// 读取符合要求的ID串
		/// </summary>
		/// <param name="strID">缺货登记的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		/// <returns>缺货登记的主键值,以,号分隔</returns>
		string ReadBookingProductIDList(string strID,int userID);

        /// <summary>
        /// 搜索缺货登记数据列表
        /// </summary>
        /// <param name="bookingProduct">BookingProductSearchInfo模型变量</param>
        /// <returns>缺货登记数据列表</returns>
        List<BookingProductInfo> SearchBookingProductList(BookingProductSearchInfo bookingProduct);

        /// <summary>
        /// 搜索缺货登记数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="bookingProduct">BookingProductSearchInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>缺货登记数据列表</returns>
        List<BookingProductInfo> SearchBookingProductList(int currentPage, int pageSize, BookingProductSearchInfo bookingProduct, ref int count);




	}
}