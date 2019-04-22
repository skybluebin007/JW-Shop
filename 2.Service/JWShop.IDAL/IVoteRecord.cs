using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 投票记录接口层说明。
	/// </summary>
	public interface IVoteRecord
	{
		/// <summary>
		/// 增加一条投票记录数据
		/// </summary>
		/// <param name="voteRecord">投票记录模型变量</param>
		int AddVoteRecord(VoteRecordInfo voteRecord);
	 
		/// <summary>
		/// 删除多条投票记录数据
		/// </summary>
		/// <param name="strID">投票记录的主键值,以,号分隔</param>
		void DeleteVoteRecord(int[] strID);


		/// <summary>
		/// 删除该类别下的投票记录数据
		/// </summary>
		/// <param name="strItemID">类别的主键值,以,号分隔</param>
		void DeleteVoteRecordByItemID(int[] strItemID); 

		/// <summary>
		/// 删除该类别下的投票记录数据
		/// </summary>
		/// <param name="strVoteID">类别的主键值,以,号分隔</param>
		void DeleteVoteRecordByVoteID(string strVoteID); 


		/// <summary>
		/// 读取一条投票记录数据
		/// </summary>
		/// <param name="id">投票记录的主键值</param>
		/// <returns>投票记录数据模型</returns>
		VoteRecordInfo ReadVoteRecord(int id);


        VoteRecordInfo ReadVoteHistoryRecord(int voteID, string ip, string itemID);
		/// <summary>
		/// 获得投票记录数据列表
		/// </summary>
        /// <param name="voteID">上级voteID</param>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <returns>投票记录数据列表</returns>
        List<VoteRecordInfo> ReadVoteRecordList(int currentPage, int pageSize, VoteRecordSearchInfo searchInfo, ref int count);


            /// <summary>
        /// 是否投过票
        /// </summary>
        /// <param name="VoteItemID">投票项目ID</param>
        /// <param name="IpAddress">投票IP</param>
        /// <returns></returns>
        bool HasVoted(int VoteItemID, string IpAddress);

        List<VoteRecordInfo> HasVotedList(int VoteItemID, string IpAddress);

	}
}