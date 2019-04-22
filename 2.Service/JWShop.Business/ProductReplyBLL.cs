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
	/// 商品回复业务逻辑。
	/// </summary>
	public sealed class ProductReplyBLL
	{
        
		private static readonly IProductReply dal = FactoryHelper.Instance<IProductReply>(Global.DataProvider,"ProductReplyDAL");

		/// <summary>
		/// 增加一条商品回复数据
		/// </summary>
		/// <param name="productReply">商品回复模型变量</param>
		public static int AddProductReply(ProductReplyInfo productReply)
		{
			productReply.ID = dal.AddProductReply(productReply);
			ProductCommentBLL.ChangeProductCommentCount(productReply.CommentID,ChangeAction.Plus);
			return productReply.ID;
		}



		/// <summary>
		/// 更新一条商品回复数据
		/// </summary>
		/// <param name="productReply">商品回复模型变量</param>
		public static void UpdateProductReply(ProductReplyInfo productReply)
		{
			ProductReplyInfo tempProductReply = ReadProductReply(productReply.ID,0);
			dal.UpdateProductReply(productReply);
			if (productReply.CommentID != tempProductReply.CommentID)
			{
				ProductCommentBLL.ChangeProductCommentCount(tempProductReply.CommentID,ChangeAction.Minus);
				ProductCommentBLL.ChangeProductCommentCount(productReply.CommentID,ChangeAction.Plus);
			}
		} 

		
		/// <summary>
		/// 删除多条商品回复数据
		/// </summary>
		/// <param name="strID">商品回复的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		public static void DeleteProductReply(string strID,int userID)
		{
			ProductCommentBLL.ChangeProductCommentCountByGeneral(strID,ChangeAction.Minus);              
			dal.DeleteProductReply(strID,userID);
		}

		/// <summary>
		/// 按分类删除商品回复数据
		/// </summary>
		/// <param name="strCommentID">分类ID,以,号分隔</param>
		public static void DeleteProductReplyByCommentID(string strCommentID)
		{
			dal.DeleteProductReplyByCommentID(strCommentID);
		}    

		/// <summary>
		/// 按分类删除商品回复数据
		/// </summary>
		/// <param name="strProductID">分类ID,以,号分隔</param>
		public static void DeleteProductReplyByProductID(string strProductID)
		{
			dal.DeleteProductReplyByProductID(strProductID);
		}    


		/// <summary>
		/// 读取一条商品回复数据
		/// </summary>
		/// <param name="id">商品回复的主键值</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据模型</returns>
		public static ProductReplyInfo ReadProductReply(int id,int userID)
		{
			return dal.ReadProductReply(id,userID);
		}


		/// <summary>
		/// 读取商品回复数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据列表</returns>
		public static List<ProductReplyInfo> ReadProductReplyList(int currentPage, int pageSize,ref int count,int userID)
		{
			return dal.ReadProductReplyList(currentPage, pageSize,ref count,userID);
		}

		/// <summary>
		/// 按分类ID获得商品回复数据列表
		/// </summary>
		/// <param name="commentID">分类ID</param>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据列表</returns>
		public static List<ProductReplyInfo> ReadProductReplyList(int commentID, int currentPage, int pageSize,ref int count,int userID)
		{
			return dal.ReadProductReplyList(commentID,currentPage, pageSize,ref count,userID);
		}



	}
}