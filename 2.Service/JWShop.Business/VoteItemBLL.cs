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
    /// 投票选项业务逻辑。
    /// </summary>
    public sealed class VoteItemBLL
    {

        private static readonly IVoteItem dal = FactoryHelper.Instance<IVoteItem>(Global.DataProvider, "VoteItemDAL");

        /// <summary>
        /// 增加一条投票选项数据
        /// </summary>
        /// <param name="voteItem">投票选项模型变量</param>
        public static int AddVoteItem(VoteItemInfo voteItem)
        {
            voteItem.ID = dal.AddVoteItem(voteItem);
            //VoteBLL.ChangeVoteCount(VoteBLL.GetLastClassID(voteItem.VoteID), ChangeAction.Plus);
            VoteBLL.ChangeVoteCountByGeneral(voteItem.ID.ToString(), ChangeAction.Plus);
            return voteItem.ID;
        }

        /// <summary>
        /// 更新一条投票选项数据
        /// </summary>
        /// <param name="voteItem">投票选项模型变量</param>
        public static void UpdateVoteItem(VoteItemInfo voteItem)
        {
            VoteItemInfo tempVoteItem = ReadVoteItem(voteItem.ID);
            dal.UpdateVoteItem(voteItem);
            if (voteItem.VoteID != tempVoteItem.VoteID)
            {
                int[] voteIds = Array.ConvertAll<string, int>(tempVoteItem.VoteID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                foreach (int _vid in voteIds) {
                    VoteBLL.ChangeVoteCount(_vid, ChangeAction.Minus);
                }
               voteIds = Array.ConvertAll<string, int>(voteItem.VoteID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                foreach (int _vid in voteIds)
                {
                    VoteBLL.ChangeVoteCount(_vid, ChangeAction.Plus);
                }                         
            }
        }


        /// <summary>
        /// 删除多条投票选项数据
        /// </summary>
        /// <param name="strID">投票选项的主键值,以,号分隔</param>
        public static void DeleteVoteItem(string strID)
        {
           // VoteRecordBLL.DeleteVoteRecordByItemID(strID);
            VoteBLL.ChangeVoteCountByGeneral(strID, ChangeAction.Minus);
            int[] itemIdS = Array.ConvertAll<string, int>((strID+",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            dal.DeleteVoteItem(itemIdS);
            //删除该项对应的评论

            UserMessageBLL.DeleteUserMessageByUserID(itemIdS);
        }

        /// <summary>
        /// 按分类删除投票选项数据
        /// </summary>
        /// <param name="strVoteID">分类ID,以,号分隔</param>
        public static void DeleteVoteItemByVoteID(string strVoteID)
        {
            VoteRecordBLL.DeleteVoteRecordByVoteID(strVoteID);
            dal.DeleteVoteItemByVoteID(strVoteID);
        }



        /// <summary>
        /// 读取一条投票选项数据
        /// </summary>
        /// <param name="id">投票选项的主键值</param>
        /// <returns>投票选项数据模型</returns>
        public static VoteItemInfo ReadVoteItem(int id)
        {
            return dal.ReadVoteItem(id);
        }


        /// <summary>
        /// 获得投票选项所有数据
        /// </summary>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemAllList()
        {
            return dal.ReadVoteItemAllList();
        }

        /// <summary>
        /// 按分类ID获得投票选项所有数据
        /// </summary>
        /// <param name="voteID">分类ID</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemByVote(int voteID)
        {
            return dal.ReadVoteItemByVote(voteID);
        }
                /// <summary>
        /// 获得投票选项所有数据~~随机排序
        /// </summary>
        /// <param name="voteID">分类ID</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemByVoteNEWID(int voteID) {
            return dal.ReadVoteItemByVoteNEWID(voteID);
        }

        /// <summary>
        /// 移动数据投票选项排序
        /// </summary>
        /// <param name="action">改变动作,向上:ChangeAction.Up;向下:ChangeActon.Down</param>
        /// <param name="id">当前记录的主键值</param>
        public static void ChangeVoteItemOrder(ChangeAction action, int id)
        {
            dal.ChangeVoteItemOrder(action, id);
        }

        /// <summary>
        /// 改变列表投票选项数量
        /// </summary>
        /// <param name="strID">投票选项的主键值</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        public static void ChangeVoteItemCount(string strID, ChangeAction action)
        {
            dal.ChangeVoteItemCount(strID, action);
        }


        /// <summary>
        /// 通过下级表改变列表投票选项数量
        /// </summary>
        /// <param name="strID">下级表的主键值,以,号分隔</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        public static void ChangeVoteItemCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeVoteItemCountByGeneral(strID, action);
        }

        /// <summary>
        /// 读取ItemName
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="voteItemLsit"></param>
        /// <returns></returns>
        public static string ReadItemName(string strID, List<VoteItemInfo> voteItemLsit)
        {
            string result = string.Empty;
            if (strID != string.Empty)
            {
                foreach (string id in strID.Split(','))
                {
                    foreach (VoteItemInfo voteItem in voteItemLsit)
                    {
                        if (voteItem.ID == Convert.ToInt32(id))
                        {
                            result += voteItem.ItemName + ",";
                        }
                    }
                }
            }
            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadVoteItemList(currentPage, pageSize, ref count);
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count,string orderType)
        {
            return dal.ReadVoteItemList(currentPage, pageSize, ref count, orderType);
        }
          /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, VoteItemSearchInfo voteitemSearch, ref int count, string orderType) {
            return dal.ReadVoteItemList(currentPage, pageSize, voteitemSearch, ref count, orderType);
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count, string orderType,int ascDesc)
        {
            return dal.ReadVoteItemList(currentPage, pageSize, ref count, orderType, ascDesc);
        }
        public static List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count, string invoteID, string orderType, int ascDesc) {
            return dal.ReadVoteItemList(currentPage, pageSize, ref count,invoteID, orderType, ascDesc);
        }
        public static List<VoteItemInfo> ReadVoteItemList(VoteItemSearchInfo voteItemSearch) {
            return dal.ReadVoteItemList(voteItemSearch);
        }
        /// <summary>
        /// 按投票分类获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <param name="voteID">投票选项ID</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemListByVote(int currentPage, int pageSize, ref int count, string voteID)
        {
            return dal.ReadVoteItemListByVote(currentPage, pageSize, ref count, voteID);
        }

        /// <summary>
        /// 按投票分类获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <param name="voteID">投票选项ID</param>
        /// <returns>投票选项数据列表</returns>
        public static List<VoteItemInfo> ReadVoteItemListByVote(int currentPage, int pageSize, ref int count, string voteID,int voteItemID,int ascDesc)
        {
            return dal.ReadVoteItemListByVote(currentPage, pageSize, ref count, voteID, voteItemID, ascDesc);
        }
          /// <summary>
        /// 获取当前最大排序号
        /// </summary>
        /// <returns></returns>
        public static int MaxOrderID() { return dal.MaxOrderID(); }
    }
}