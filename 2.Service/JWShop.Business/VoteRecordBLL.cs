using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
	/// <summary>
	/// 投票记录业务逻辑。
	/// </summary>
	public sealed class VoteRecordBLL
	{
        
		private static readonly IVoteRecord dal = FactoryHelper.Instance<IVoteRecord>(Global.DataProvider,"VoteRecordDAL");

		/// <summary>
		/// 增加一条投票记录数据
		/// </summary>
		/// <param name="voteRecord">投票记录模型变量</param>
		public static int AddVoteRecord(VoteRecordInfo voteRecord)
		{
			voteRecord.ID = dal.AddVoteRecord(voteRecord);
			VoteItemBLL.ChangeVoteItemCount(voteRecord.ItemID,ChangeAction.Plus);
			return voteRecord.ID;
		}


        public static VoteRecordInfo ReadVoteHistoryRecord(int voteID, string ip, string itemID)
        {
            
            return dal.ReadVoteHistoryRecord(voteID, ip, itemID);
        }

		
		/// <summary>
		/// 删除多条投票记录数据
		/// </summary>
		/// <param name="strID">投票记录的主键值,以,号分隔</param>
		public static void DeleteVoteRecord(string strID)
		{
			VoteItemBLL.ChangeVoteItemCountByGeneral(strID,ChangeAction.Minus);
            int[] ids = Array.ConvertAll<string, int>((strID + ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
			dal.DeleteVoteRecord(ids);
		}

		/// <summary>
		/// 按分类删除投票记录数据
		/// </summary>
		/// <param name="strItemID">分类ID,以,号分隔</param>
		public static void DeleteVoteRecordByItemID(string strItemID)
		{
            int[] itemids = Array.ConvertAll<string, int>((strItemID + ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            dal.DeleteVoteRecordByItemID(itemids);
		}    

		/// <summary>
		/// 按分类删除投票记录数据
		/// </summary>
		/// <param name="strVoteID">分类ID,以,号分隔</param>
		public static void DeleteVoteRecordByVoteID(string strVoteID)
		{
			dal.DeleteVoteRecordByVoteID(strVoteID);
		}    


		/// <summary>
		/// 读取一条投票记录数据
		/// </summary>
		/// <param name="id">投票记录的主键值</param>
		/// <returns>投票记录数据模型</returns>
		public static VoteRecordInfo ReadVoteRecord(int id)
		{
			return dal.ReadVoteRecord(id);
		}


		/// <summary>
		/// 按分类ID获得投票记录数据列表
		/// </summary>
        /// <param name="voteID">分类ID</param>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="count">总数量</param>
		/// <returns>投票记录数据列表</returns>
        public static List<VoteRecordInfo> ReadVoteRecordList(int currentPage, int pageSize, VoteRecordSearchInfo searchInfo, ref int count)
		{
            return dal.ReadVoteRecordList(currentPage, pageSize,searchInfo, ref count);
		}

            /// <summary>
        /// 是否投过票
        /// </summary>
        /// <param name="VoteItemID">投票项目ID</param>
        /// <param name="IpAddress">投票IP</param>
        /// <returns></returns>
        public static bool HasVoted(int voteItemID, string ipAddress) { return dal.HasVoted(voteItemID, ipAddress); }

        public static List<VoteRecordInfo> HasVotedList(int VoteItemID, string IpAddress) {
            return dal.HasVotedList(VoteItemID, IpAddress);
        }

	}
}