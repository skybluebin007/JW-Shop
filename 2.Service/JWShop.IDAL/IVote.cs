using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 投票接口层说明。
    /// </summary>
    public interface IVote
    {
        /// <summary>
        /// 增加一条投票数据
        /// </summary>
        /// <param name="vote">投票模型变量</param>
        int AddVote(VoteInfo vote);

        /// <summary>
        /// 更新一条投票数据
        /// </summary>
        /// <param name="vote">投票模型变量</param>
        void UpdateVote(VoteInfo vote);

        /// <summary>
        /// 删除多条投票数据
        /// </summary>
        /// <param name="strID">投票的主键值,以,号分隔</param>
        void DeleteVote(int[] strID);




        /// <summary>
        /// 读取一条投票数据
        /// </summary>
        /// <param name="id">投票的主键值</param>
        /// <returns>投票数据模型</returns>
        VoteInfo ReadVote(int id);



        /// <summary>
        /// 获得投票数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票数据列表</returns>
        List<VoteInfo> ReadVoteList(int currentPage, int pageSize, ref int count);

          /// <summary>
      /// 获得投票类型列表--所有
      /// </summary>
      /// <returns></returns>
        List<VoteInfo> ReadVoteList();


        /// <summary>
        /// 改变列表投票数量
        /// </summary>
        /// <param name="id">投票的主键值</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        void ChangeVoteCount(int id, ChangeAction action);

        /// <summary>
        /// 通过下级表改变列表投票数量
        /// </summary>
        /// <param name="strID">下级表的主键值,以,号分隔</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        void ChangeVoteCountByGeneral(string strID, ChangeAction action);

        int GetVoteItemCountByVote(string VoteID);

    }
}
