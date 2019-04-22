using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 投票选项接口层说明。
	/// </summary>
	public interface IVoteItem
	{
		/// <summary>
		/// 增加一条投票选项数据
		/// </summary>
		/// <param name="voteItem">投票选项模型变量</param>
		int AddVoteItem(VoteItemInfo voteItem);

		/// <summary>
		/// 更新一条投票选项数据
		/// </summary>
		/// <param name="voteItem">投票选项模型变量</param>
		void UpdateVoteItem(VoteItemInfo voteItem);
	 
		/// <summary>
		/// 删除多条投票选项数据
		/// </summary>
		/// <param name="strID">投票选项的主键值,以,号分隔</param>
		void DeleteVoteItem(int[] strID);

		/// <summary>
		/// 删除该类别下的投票选项数据
		/// </summary>
		/// <param name="strVoteID">类别的主键值,以,号分隔</param>
		void DeleteVoteItemByVoteID(string strVoteID); 



		/// <summary>
		/// 读取一条投票选项数据
		/// </summary>
		/// <param name="id">投票选项的主键值</param>
		/// <returns>投票选项数据模型</returns>
		VoteItemInfo ReadVoteItem(int id);


        /// <summary>
        /// 获得投票选项所有数据
        /// </summary>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemAllList();

        /// <summary>
        /// 获得投票选项所有数据
        /// </summary>
        /// <param name="voteID">上级VoteID</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemByVote(int voteID);
                /// <summary>
        /// 获得投票选项所有数据~~随机排序
        /// </summary>
        /// <param name="voteID">分类ID</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemByVoteNEWID(int voteID);

		/// <summary>
		/// 移动投票选项数据排序
		/// </summary>
		/// <param name="action">改变动作,向上:ChangeAction.Up;向下:ChangeActon.Down</param>
		/// <param name="id">当前记录的主键值</param>
		void ChangeVoteItemOrder(ChangeAction action, int id);

		/// <summary>
		/// 改变列表投票选项数量
		/// </summary>
        /// <param name="strID">投票选项的主键值</param>
		/// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        void ChangeVoteItemCount(string strID, ChangeAction action);

		/// <summary>
		/// 通过下级表改变列表投票选项数量
		/// </summary>
		/// <param name="strID">下级表的主键值,以,号分隔</param>
		/// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
		void ChangeVoteItemCountByGeneral(string strID,ChangeAction action);

        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count);

        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count,string orderType);
          /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, VoteItemSearchInfo voteitemSearch, ref int count, string orderType);
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count, string orderType,int ascDesc);

        List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count, string invoteID, string orderType, int ascDesc);
        List<VoteItemInfo> ReadVoteItemList(VoteItemSearchInfo voteItemSearch);
        /// <summary>
        /// 按投票分类获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <param name="voteID">投票选项ID</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemListByVote(int currentPage, int pageSize, ref int count, string voteID);

        /// <summary>
        /// 按投票分类获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <param name="voteID">投票选项ID</param>
        /// <returns>投票选项数据列表</returns>
        List<VoteItemInfo> ReadVoteItemListByVote(int currentPage, int pageSize, ref int count, string voteID, int voteItemID, int ascDesc);
  /// <summary>
        /// 获取当前最大排序号
        /// </summary>
        /// <returns></returns>
        int MaxOrderID();
	}
}