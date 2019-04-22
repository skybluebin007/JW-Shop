using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;

namespace JWShop.MssqlDAL
{
	/// <summary>
	/// 商品回复数据层说明。
	/// </summary>
	public sealed class ProductReplyDAL:IProductReply
	{
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
		/// <summary>
		/// 增加一条商品回复数据
		/// </summary>
		/// <param name="productReply">商品回复模型变量</param>
		public int AddProductReply(ProductReplyInfo productReply)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@productID",SqlDbType.Int),
				new SqlParameter("@commentID",SqlDbType.Int),
				new SqlParameter("@content",SqlDbType.NText),
				new SqlParameter("@userIP",SqlDbType.NVarChar),
				new SqlParameter("@postDate",SqlDbType.DateTime),
				new SqlParameter("@userID",SqlDbType.Int),
				new SqlParameter("@userName",SqlDbType.NVarChar)
			};
			parameters[0].Value = productReply.ProductID;
			parameters[1].Value = productReply.CommentID;
			parameters[2].Value = productReply.Content;
			parameters[3].Value = productReply.UserIP;
			parameters[4].Value = productReply.PostDate;
			parameters[5].Value = productReply.UserID;
			parameters[6].Value = productReply.UserName;
			Object id= ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix+"AddProductReply",parameters);
			return(Convert.ToInt32(id));
		}

		
		/// <summary>
		/// 更新一条商品回复数据
		/// </summary>
		/// <param name="productReply">商品回复模型变量</param>
		public void UpdateProductReply(ProductReplyInfo productReply)
		{			 
			SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
				new SqlParameter("@content",SqlDbType.NText)
			};
			parameters[0].Value = productReply.ID;
			parameters[1].Value = productReply.Content;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"UpdateProductReply",parameters);
		}

		/// <summary>
		/// 删除多条商品回复数据
		/// </summary>
		/// <param name="strID">商品回复的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		public void DeleteProductReply(string strID,int userID)
		{		
			SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar),
				new SqlParameter("@userID",SqlDbType.Int)
			};
			parameters[0].Value = strID;
			parameters[1].Value = userID;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteProductReply",parameters);
		}

		/// <summary>
		/// 按分类删除商品回复数据
		/// </summary>
		/// <param name="strCommentID">分类ID,以,号分隔</param>
		public void DeleteProductReplyByCommentID(string strCommentID)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@strCommentID",SqlDbType.NVarChar)
			};
			parameters[0].Value = strCommentID;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteProductReplyByCommentID",parameters);
		}		 


		/// <summary>
		/// 按分类删除商品回复数据
		/// </summary>
		/// <param name="strProductID">分类ID,以,号分隔</param>
		public void DeleteProductReplyByProductID(string strProductID)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@strProductID",SqlDbType.NVarChar)
			};
			parameters[0].Value = strProductID;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteProductReplyByProductID",parameters);
		}		 


		/// <summary>
		/// 读取一条商品回复数据
		/// </summary>
		/// <param name="id">商品回复的主键值</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据模型</returns>
		public ProductReplyInfo ReadProductReply(int id,int userID)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@id", SqlDbType.NVarChar),
				new SqlParameter("@userID",SqlDbType.Int)
			};
			parameters[0].Value = id;
			parameters[1].Value = userID;
			ProductReplyInfo productReply= new ProductReplyInfo();	
			using (SqlDataReader dr =ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix+"ReadProductReply",parameters))
			{
				if (dr.Read())
				{
				productReply.ID=dr.GetInt32(0);
				productReply.ProductID=dr.GetInt32(1);
				productReply.CommentID=dr.GetInt32(2);
				productReply.Content=dr[3].ToString();
				productReply.UserIP=dr[4].ToString();
				productReply.PostDate=dr.GetDateTime(5);
				productReply.UserID=dr.GetInt32(6);
				productReply.UserName=dr[7].ToString();
				}
			}
			return productReply;
		}

		
	
		/// <summary>
		/// 获得商品回复数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据列表</returns>
		public List<ProductReplyInfo> ReadProductReplyList(int currentPage, int pageSize,ref int count,int userID)
		{
            using (var conn = new SqlConnection(connectString))
            {
                List<ProductReplyInfo> productReplyList = new List<ProductReplyInfo>();
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = ShopMssqlHelper.TablePrefix + "ProductReply";
                pc.Fields = "[ID],[ProductID],[CommentID],[Content],[UserIP],[PostDate],[UserID],[UserName]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[ID]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("[UserID]", userID, ConditionType.Equal);
                pc.Count = count;
                count = pc.Count;

                return conn.Query<ProductReplyInfo>(ShopMssqlHelper.TablePrefix + "ReadPageList", new
                {
                    tableName = pc.TableName,
                    fields = pc.Fields,
                    pageSize = pc.PageSize,
                    currentPage = pc.CurrentPage,
                    fieldName = pc.OrderField,
                    orderType = pc.OrderType,
                    condition = pc.MssqlCondition.ToString()
                }, null, true, null, CommandType.StoredProcedure).ToList();
            }
		}

		/// <summary>
		/// 获得商品回复数据列表
		/// </summary>
		/// <param name="commentID">分类ID</param>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据列表</returns>
		public List<ProductReplyInfo> ReadProductReplyList(int commentID,int currentPage, int pageSize,ref int count,int userID)
		{
			List<ProductReplyInfo> productReplyList = new List<ProductReplyInfo>();
			ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
			pc.TableName = ShopMssqlHelper.TablePrefix+"ProductReply";
			pc.Fields = "[ID],[ProductID],[CommentID],[Content],[UserIP],[PostDate],[UserID],[UserName]";
			pc.CurrentPage = currentPage;
			pc.PageSize = pageSize;
			pc.OrderField = "[ID]";
			pc.OrderType = OrderType.Desc;
			pc.MssqlCondition.Add("[UserID]",userID,ConditionType.Equal);
			pc.MssqlCondition.Add("[CommentID]",commentID,ConditionType.Equal);
			pc.Count=count;
			count = pc.Count;	
			using (SqlDataReader dr = pc.ExecuteReader())
			{
				PrepareProductReplyModel(dr,productReplyList);
			}
			return productReplyList;
		}







	}
}