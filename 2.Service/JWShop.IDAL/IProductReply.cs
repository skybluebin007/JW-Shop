using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 商品回复接口层说明。
	/// </summary>
	public interface IProductReply
	{
		/// <summary>
		/// 增加一条商品回复数据
		/// </summary>
		/// <param name="productReply">商品回复模型变量</param>
		int AddProductReply(ProductReplyInfo productReply);

		/// <summary>
		/// 更新一条商品回复数据
		/// </summary>
		/// <param name="productReply">商品回复模型变量</param>
		void UpdateProductReply(ProductReplyInfo productReply);
	 
		/// <summary>
		/// 删除多条商品回复数据
		/// </summary>
		/// <param name="strID">商品回复的主键值,以,号分隔</param>
		/// <param name="userID">用户ID</param>
		void DeleteProductReply(string strID,int userID);

		/// <summary>
		/// 删除该类别下的商品回复数据
		/// </summary>
		/// <param name="strCommentID">类别的主键值,以,号分隔</param>
		void DeleteProductReplyByCommentID(string strCommentID); 

		/// <summary>
		/// 删除该类别下的商品回复数据
		/// </summary>
		/// <param name="strProductID">类别的主键值,以,号分隔</param>
		void DeleteProductReplyByProductID(string strProductID); 


		/// <summary>
		/// 读取一条商品回复数据
		/// </summary>
		/// <param name="id">商品回复的主键值</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据模型</returns>
		ProductReplyInfo ReadProductReply(int id,int userID);

	
		/// <summary>
		/// 获得商品回复数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据列表</returns>
		List<ProductReplyInfo> ReadProductReplyList(int currentPage, int pageSize,ref int count,int userID);

		/// <summary>
		/// 获得商品回复数据列表
		/// </summary>
		/// <param name="commentID">上级ProductCommentID</param>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <param name="userID">用户ID</param>
		/// <returns>商品回复数据列表</returns>
		List<ProductReplyInfo> ReadProductReplyList(int commentID, int currentPage, int pageSize,ref int count,int userID);






	}
}