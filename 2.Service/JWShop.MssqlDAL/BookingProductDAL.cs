using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
	/// <summary>
	/// 缺货登记数据层说明。
	/// </summary>
	public sealed class BookingProductDAL:IBookingProduct
	{

        /// <summary>
        /// 增加一条缺货登记数据
        /// </summary>
        /// <param name="bookingProduct">缺货登记模型变量</param>
        public int AddBookingProduct(BookingProductInfo bookingProduct)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@productID",SqlDbType.Int),
				new SqlParameter("@productName",SqlDbType.NVarChar),
				new SqlParameter("@relationUser",SqlDbType.NVarChar),
				new SqlParameter("@email",SqlDbType.NVarChar),
				new SqlParameter("@tel",SqlDbType.NVarChar),
				new SqlParameter("@userNote",SqlDbType.NVarChar),
				new SqlParameter("@bookingDate",SqlDbType.DateTime),
				new SqlParameter("@bookingIP",SqlDbType.NVarChar),
				new SqlParameter("@isHandler",SqlDbType.Int),
				new SqlParameter("@handlerDate",SqlDbType.DateTime),
				new SqlParameter("@handlerAdminID",SqlDbType.Int),
				new SqlParameter("@handlerAdminName",SqlDbType.NVarChar),
				new SqlParameter("@handlerNote",SqlDbType.NVarChar),
				new SqlParameter("@userID",SqlDbType.Int),
				new SqlParameter("@userName",SqlDbType.NVarChar)
			};
            parameters[0].Value = bookingProduct.ProductID;
            parameters[1].Value = bookingProduct.ProductName;
            parameters[2].Value = bookingProduct.RelationUser;
            parameters[3].Value = bookingProduct.Email;
            parameters[4].Value = bookingProduct.Tel;
            parameters[5].Value = bookingProduct.UserNote;
            parameters[6].Value = bookingProduct.BookingDate;
            parameters[7].Value = bookingProduct.BookingIP;
            parameters[8].Value = bookingProduct.IsHandler;
            parameters[9].Value = bookingProduct.HandlerDate;
            parameters[10].Value = bookingProduct.HandlerAdminID;
            parameters[11].Value = bookingProduct.HandlerAdminName;
            parameters[12].Value = bookingProduct.HandlerNote;
            parameters[13].Value = bookingProduct.UserID;
            parameters[14].Value = bookingProduct.UserName;
            Object id = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddBookingProduct", parameters);
            return (Convert.ToInt32(id));
        }

		/// <summary>
		/// 更新一条缺货登记数据
		/// </summary>
		/// <param name="bookingProduct">缺货登记模型变量</param>
		public void UpdateBookingProduct(BookingProductInfo bookingProduct)
		{			 
			SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
				new SqlParameter("@isHandler",SqlDbType.Int),
				new SqlParameter("@handlerDate",SqlDbType.DateTime),
				new SqlParameter("@handlerAdminID",SqlDbType.Int),
				new SqlParameter("@handlerAdminName",SqlDbType.NVarChar),
				new SqlParameter("@handlerNote",SqlDbType.NVarChar)
			};
			parameters[0].Value = bookingProduct.ID;
			parameters[1].Value = bookingProduct.IsHandler;
			parameters[2].Value = bookingProduct.HandlerDate;
			parameters[3].Value = bookingProduct.HandlerAdminID;
			parameters[4].Value = bookingProduct.HandlerAdminName;
			parameters[5].Value = bookingProduct.HandlerNote;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"UpdateBookingProduct",parameters);
		}

		/// <summary>
		/// 删除多条缺货登记数据
		/// </summary>
		/// <param name="strID">缺货登记的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		public void DeleteBookingProduct(string strID,int userID)
		{		
			SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar),
				new SqlParameter("@userID",SqlDbType.Int)
			};
			parameters[0].Value = strID;
			parameters[1].Value = userID;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteBookingProduct",parameters);
		}





		/// <summary>
		/// 读取一条缺货登记数据
		/// </summary>
		/// <param name="id">缺货登记的主键值</param>
		/// <param name="userID">用户ID</param>
		/// <returns>缺货登记数据模型</returns>
		public BookingProductInfo ReadBookingProduct(int id,int userID)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@id", SqlDbType.Int),
				new SqlParameter("@userID",SqlDbType.Int)
			};
			parameters[0].Value = id;
			parameters[1].Value = userID;
			BookingProductInfo bookingProduct= new BookingProductInfo();	
			using (SqlDataReader dr =ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix+"ReadBookingProduct",parameters))
			{
				if (dr.Read())
				{
                    bookingProduct.ID = dr.GetInt32(0);
                    bookingProduct.ProductID = dr.GetInt32(1);
                    bookingProduct.ProductName = dr[2].ToString();
                    bookingProduct.RelationUser = dr[3].ToString();
                    bookingProduct.Email = dr[4].ToString();
                    bookingProduct.Tel = dr[5].ToString();
                    bookingProduct.UserNote = dr[6].ToString();
                    bookingProduct.BookingDate = dr.GetDateTime(7);
                    bookingProduct.BookingIP = dr[8].ToString();
                    bookingProduct.IsHandler = dr.GetInt32(9);
                    bookingProduct.HandlerDate = dr.GetDateTime(10);
                    bookingProduct.HandlerAdminID = dr.GetInt32(11);
                    bookingProduct.HandlerAdminName = dr[12].ToString();
                    bookingProduct.HandlerNote = dr[13].ToString();
                    bookingProduct.UserID = dr.GetInt32(14);
                    bookingProduct.UserName = dr[15].ToString();
				}
			}
			return bookingProduct;
		}

		/// <summary>
		/// 准备缺货登记模型
		/// </summary>
		/// <param name="dr">Datareader</param>
		/// <param name="bookingProductList">缺货登记的数据列表</param>
		public void PrepareBookingProductModel(SqlDataReader dr,List<BookingProductInfo> bookingProductList)
		{
			while (dr.Read())
			{
				BookingProductInfo bookingProduct= new BookingProductInfo();
                bookingProduct.ID = dr.GetInt32(0);
                bookingProduct.ProductID = dr.GetInt32(1);
                bookingProduct.ProductName = dr[2].ToString();
                bookingProduct.RelationUser = dr[3].ToString();
                bookingProduct.Email = dr[4].ToString();
                bookingProduct.Tel = dr[5].ToString();
                bookingProduct.UserNote = dr[6].ToString();
                bookingProduct.BookingDate = dr.GetDateTime(7);
                bookingProduct.BookingIP = dr[8].ToString();
                bookingProduct.IsHandler = dr.GetInt32(9);
                bookingProduct.HandlerDate = dr.GetDateTime(10);
                bookingProduct.HandlerAdminID = dr.GetInt32(11);
                bookingProduct.HandlerAdminName = dr[12].ToString();
                bookingProduct.HandlerNote = dr[13].ToString();
                bookingProduct.UserID = dr.GetInt32(14);
                bookingProduct.UserName = dr[15].ToString();
				bookingProductList.Add(bookingProduct);
			}
		} 

		/// <summary>
		/// 读取符合要求的ID串
		/// </summary>
		/// <param name="strID">缺货登记的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		/// <returns>缺货登记的主键值,以,号分隔</returns>
		public string ReadBookingProductIDList(string strID,int userID)
		{
			string idList= string.Empty;
			SqlParameter[] parameters ={
				new SqlParameter("@strID", SqlDbType.NVarChar),
				new SqlParameter("@userID", SqlDbType.Int)
			};
			parameters[0].Value = strID;
			parameters[1].Value = userID;
			using (SqlDataReader dr =ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix+"ReadBookingProductIDList",parameters))
			{
				while (dr.Read())
				{
					if(idList==string.Empty)
					{
						idList=dr.GetInt32(0).ToString();
					}
					else
					{
						idList+=","+dr.GetInt32(0).ToString();
					}
				}
			}
			return idList;
		}

        /// <summary>
        /// 搜索缺货登记数据列表
        /// </summary>
        /// <param name="bookingProductSearch">BookingProductSearchInfo模型变量</param>
        /// <returns>缺货登记数据列表</returns>
        public List<BookingProductInfo> SearchBookingProductList(BookingProductSearchInfo bookingProductSearch)
        {
            string condition = PrepareCondition(bookingProductSearch).ToString();
            List<BookingProductInfo> bookingProductList = new List<BookingProductInfo>();
            SqlParameter[] parameters ={
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = condition;
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchBookingProductList", parameters))
            {
                PrepareBookingProductModel(dr, bookingProductList);
            }
            return bookingProductList;
        }

        /// <summary>
        /// 搜索缺货登记数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="bookingProductSearch">BookingProductSearchInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>缺货登记数据列表</returns>
        public List<BookingProductInfo> SearchBookingProductList(int currentPage, int pageSize, BookingProductSearchInfo bookingProductSearch, ref int count)
        {
            List<BookingProductInfo> bookingProductList = new List<BookingProductInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "ProductBooking";
            pc.Fields = "[ID],[ProductID],[ProductName],[RelationUser],[Email],[Tel],[UserNote],[BookingDate],[BookingIP],[IsHandler],[HandlerDate],[HandlerAdminID],[HandlerAdminName],[HandlerNote],[UserID],[UserName]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[ID]";
            pc.OrderType = OrderType.Desc;
            pc.MssqlCondition = PrepareCondition(bookingProductSearch);
            
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareBookingProductModel(dr, bookingProductList);
            }
            return bookingProductList;
        }


        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="bookingProductSearch">BookingProductSearchInfo模型变量</param>
        public MssqlCondition PrepareCondition(BookingProductSearchInfo bookingProductSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[ProductName]", bookingProductSearch.ProductName, ConditionType.Like);			
            mssqlCondition.Add("[RelationUser]", bookingProductSearch.RelationUser, ConditionType.Like);
            mssqlCondition.Add("[Email]", bookingProductSearch.Email, ConditionType.Like);
            mssqlCondition.Add("[Tel]", bookingProductSearch.Tel, ConditionType.Like);
            mssqlCondition.Add("[IsHandler]", bookingProductSearch.IsHandler, ConditionType.Equal);
            mssqlCondition.Add("[UserID]", bookingProductSearch.UserID, ConditionType.Equal);

            return mssqlCondition;
        }
	}
}