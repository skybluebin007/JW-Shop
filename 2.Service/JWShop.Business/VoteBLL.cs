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
    /// 投票业务逻辑。
    /// </summary>
   public sealed class VoteBLL:BaseBLL
    {
       private static readonly IVote dal = FactoryHelper.Instance<IVote>(Global.DataProvider, "VoteDAL");
    
        private static readonly string cacheKey = CacheKey.ReadCacheKey("Vote");
        /// <summary>
        /// 增加一条投票数据
        /// </summary>
        /// <param name="vote">投票模型变量</param>
        public static int AddVote(VoteInfo vote)
        {
            vote.ID = dal.AddVote(vote);
            CacheHelper.Remove(cacheKey);
            return vote.ID;
        }



        /// <summary>
        /// 更新一条投票数据
        /// </summary>
        /// <param name="vote">投票模型变量</param>
        public static void UpdateVote(VoteInfo vote)
        {
            CacheHelper.Remove(cacheKey);
            dal.UpdateVote(vote);
            
        }

        /// <summary>
        /// 删除多条投票数据
        /// </summary>
        /// <param name="strID">投票的主键值,以,号分隔</param>
        public static void DeleteVote(int[] strID)
        {
            CacheHelper.Remove(cacheKey);
            //VoteItemBLL.DeleteVoteItemByVoteID(strID);
            dal.DeleteVote(strID);
            
        }




        /// <summary>
        /// 读取一条投票数据
        /// </summary>
        /// <param name="id">投票的主键值</param>
        /// <returns>投票数据模型</returns>
        public static VoteInfo ReadVote(int id)
        {
           
            if (CacheHelper.Read(cacheKey) == null)
            {
                List<VoteInfo> voteList = ReadVoteList();
                return voteList.FirstOrDefault(k => k.ID == id) ?? new VoteInfo();
            }
            return ((List<VoteInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.ID == id) ?? new VoteInfo();
        }
       /// <summary>
       /// 根据voteid读取最底层分类
       /// </summary>
       /// <param name="voteId"></param>
       /// <returns></returns> 
       public static VoteInfo ReadLastVoteClass(string voteId) {
           VoteInfo result = new VoteInfo();
           int _voteId = 0;
           List<VoteInfo> voteList = ReadVoteList();
           if (!string.IsNullOrEmpty(voteId))
           {
               char[] splitChar = { '|' };
               string[] classArr = voteId.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
               if (classArr.Length > 0)
               {
                  _voteId=Convert.ToInt32(classArr[classArr.Length - 1]);
               }              
           }
           if (_voteId > 0) result = voteList.FirstOrDefault(k => k.ID == _voteId);
           return result;
        }
        /// <summary>
        /// 读取投票数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票数据列表</returns>
        public static List<VoteInfo> ReadVoteList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadVoteList(currentPage, pageSize, ref count);
        }
      /// <summary>
      /// 获得投票类型列表--所有
      /// </summary>
      /// <returns></returns>
        public static List<VoteInfo> ReadVoteList() {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadVoteList());
            }
            return (List<VoteInfo>)CacheHelper.Read(cacheKey);        
        }
        /// <summary>
        /// 读取名字已经缩进好的分类列表
        /// </summary>
        /// <returns>商品分类数据列表</returns>
        public static List<VoteInfo> ReadNamedList()
        {
            int count = 0;
            List<VoteInfo> result = new List<VoteInfo>();
            var classes = ReadVoteList();
            foreach (VoteInfo _vote in classes)
            {
                if (_vote.FatherID == 0)
                {
                    result.Add(_vote);
                    result.AddRange(ReadChilds(_vote.ID, 2));
                }
            }
            CacheHelper.Remove(cacheKey);
            return result;
        }
       /// <summary>
       /// 获取投票类型顶级分类列表
       /// </summary>
       /// <returns></returns>
        public static List<VoteInfo> ReadVoteRootList()
        {
            var voteList = ReadVoteList();
            return voteList.Where(k => k.FatherID == 0).ToList();
        }
        public static List<VoteInfo> ReadChilds(int parentId, int depth)
        {
            int count = 0;
            List<VoteInfo> result = new List<VoteInfo>();
            var classes = ReadVoteList();
            foreach (var entity in classes)
            {
                if (entity.FatherID == parentId)
                {
                    var temp = entity;
                    string tempString = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        tempString += HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    temp.Title = tempString + temp.Title;
                    result.Add(temp);
                    result.AddRange(ReadChilds(temp.ID, depth + 1));
                }
            }

            return result;
        }
        public static List<VoteInfo> ReadChilds(int parentId)
        {
            int count = 0;
            List<VoteInfo> result = new List<VoteInfo>();
            var classes = ReadVoteList();

            foreach (var entity in classes)
            {
                if (entity.FatherID == parentId)
                {
                    var temp = entity;

                    result.Add(temp);

                }
            }

            return result;
        }
        /// <summary>
        /// 获取最近一级分类
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static int GetLastClassID(string classID)
        {
            if (!string.IsNullOrEmpty(classID))
            {
                char[] splitChar = { '|' };
                string[] classArr = classID.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                if (classArr.Length > 0)
                {
                    return Convert.ToInt32(classArr[classArr.Length - 1]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static int GetTopClassID(string classID)
        {
            if (!string.IsNullOrEmpty(classID))
            {
                char[] splitChar = { '|' };
                string[] classArr = classID.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                if (classArr.Length > 0)
                {
                    return Convert.ToInt32(classArr[0]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 读取文章完整的上级分类ID(|1|2|3|)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>完整的分类ID</returns>
        public static string ReadVoteFullFatherID(int id)
        {
            return ReadVoteFatherID(id) + "|" + id.ToString() + "|"; ;
        }
        /// <summary>
        /// 读取文章上级分类ID(|1)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>上级的分类ID</returns>
        private static string ReadVoteFatherID(int id)
        {
            string result = string.Empty;
            int fatherID = ReadVote(id).FatherID;
            if (fatherID > 0)
            {
                result = ReadVoteFatherID(fatherID) + "|" + fatherID;
            }
            return result;
        }
        /// <summary>
        /// 改变列表投票数量
        /// </summary>
        /// <param name="id">投票的主键值</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        public static void ChangeVoteCount(int id, ChangeAction action)
        {
            dal.ChangeVoteCount(id, action);
        }


        /// <summary>
        /// 通过下级表改变列表投票数量
        /// </summary>
        /// <param name="strID">下级表的主键值,以,号分隔</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        public static void ChangeVoteCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeVoteCountByGeneral(strID, action);
        }
       /// <summary>
       /// 获取该类型下所有投票选项总数
       /// </summary>
       /// <param name="VoteID"></param>
       /// <returns></returns>
        public static int GetVoteItemCountByVote(string VoteID)
        {
            return dal.GetVoteItemCountByVote(VoteID);
        }
    }
}
