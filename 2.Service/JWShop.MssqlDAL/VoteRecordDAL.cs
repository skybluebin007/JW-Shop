using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Linq;


namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 投票记录数据层说明。
    /// </summary>
    public sealed class VoteRecordDAL : IVoteRecord
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 增加一条投票记录数据
        /// </summary>
        /// <param name="voteRecord">投票记录模型变量</param>
        public int AddVoteRecord(VoteRecordInfo entity)
        {
              using (var conn = new SqlConnection(connectString))
          {
              string sql = @"INSERT INTO [VoteRecord]([VoteID],[ItemID],[UserIP],[AddDate],[UserID],[UserName]) VALUES(@VoteID,@ItemID,@UserIP,@AddDate,@UserID,@UserName);
                            select SCOPE_IDENTITY()";
              return conn.Query<int>(sql, entity).Single();
          }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voteRecord"></param>
        public VoteRecordInfo ReadVoteHistoryRecord(int voteID, string ip, string itemID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@voteID", SqlDbType.NVarChar),
                new SqlParameter("@ip", SqlDbType.NVarChar),
                new SqlParameter("@itemID", SqlDbType.NVarChar)
			};
            parameters[0].Value = voteID;
            parameters[1].Value = ip;
            parameters[2].Value = itemID;
            VoteRecordInfo voteRecord = new VoteRecordInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadVoteHistoryRecord", parameters))
            {
                if (dr.Read())
                {
                    voteRecord.ID = dr.GetInt32(0);
                    voteRecord.VoteID = dr[1].ToString();
                    voteRecord.ItemID = dr[2].ToString();
                    voteRecord.UserIP = dr[3].ToString();
                    voteRecord.AddDate = dr.GetDateTime(4);
                    voteRecord.UserID = dr.GetInt32(5);
                    voteRecord.UserName = dr[6].ToString();
                }
            }
            return voteRecord;
        }

        /// <summary>
        /// 删除多条投票记录数据
        /// </summary>
        /// <param name="strID">投票记录的主键值,以,号分隔</param>
        public void DeleteVoteRecord(int[] IdS)
        {
            if (IdS.Length > 0)
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "delete from VoteRecord where id in @ids";

                    conn.Execute(sql, new { ids = IdS });
                }
            }          
        }

        /// <summary>
        /// 按分类删除投票记录数据
        /// </summary>
        /// <param name="strItemID">分类ID,以,号分隔</param>
        public void DeleteVoteRecordByItemID(int[] voteIdS)
        {
            if (voteIdS.Length > 0)
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "delete from VoteRecord where [ItemID] in @voteids";

                    conn.Execute(sql, new { voteids = voteIdS });
                }
            }          
        }

        /// <summary>
        /// 按分类删除投票记录数据
        /// </summary>
        /// <param name="strVoteID">分类ID,以,号分隔</param>
        public void DeleteVoteRecordByVoteID(string strVoteID)
        {
            if (!string.IsNullOrEmpty(strVoteID))
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "delete from VoteRecord  WHERE [VoteID] like @voteId";

                    conn.Execute(sql, new { voteId = "|" + strVoteID + "|" });
                }
            }      
        }

        /// <summary>
        /// 读取一条投票记录数据
        /// </summary>
        /// <param name="id">投票记录的主键值</param>
        /// <returns>投票记录数据模型</returns>
        public VoteRecordInfo ReadVoteRecord(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from VoteRecord where id=@id";

                var data = conn.Query<VoteRecordInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new VoteRecordInfo();
            }
        }

        /// <summary>
        /// 准备投票记录模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="voteRecordList">投票记录的数据列表</param>
        public void PrepareVoteRecordModel(SqlDataReader dr, List<VoteRecordInfo> voteRecordList)
        {
            while (dr.Read())
            {
                VoteRecordInfo voteRecord = new VoteRecordInfo();
                voteRecord.ID = dr.GetInt32(0);
                voteRecord.VoteID = dr[1].ToString();
                voteRecord.ItemID = dr[2].ToString();
                voteRecord.UserIP = dr[3].ToString();
                voteRecord.AddDate = dr.GetDateTime(4);
                voteRecord.UserID = dr.GetInt32(5);
                voteRecord.UserName = dr[6].ToString();
                voteRecordList.Add(voteRecord);
            }
        }


        /// <summary>
        /// 获得投票记录数据列表
        /// </summary>
        /// <param name="voteID">分类ID</param>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票记录数据列表</returns>
        public List<VoteRecordInfo> ReadVoteRecordList(int currentPage, int pageSize,VoteRecordSearchInfo searchInfo, ref int count)
        {
            List<VoteRecordInfo> voteRecordList = new List<VoteRecordInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName ="VoteRecord";
            pc.Fields = "[ID],[VoteID],[ItemID],[UserIP],[AddDate],[UserID],[UserName]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[ID]";
            pc.OrderType = OrderType.Desc;
            PrepareCondition(pc.MssqlCondition, searchInfo);
            pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteRecordModel(dr, voteRecordList);
            }
            return voteRecordList;
        }

        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="voteItemSearch"></param>
        public void PrepareCondition(MssqlCondition mssqlCondition, VoteRecordSearchInfo searchInfo)
        {
            mssqlCondition.Add("[VoteID]", searchInfo.VoteID, ConditionType.Like);
            mssqlCondition.Add("[ItemID]",searchInfo.ItemID, ConditionType.Equal);
            mssqlCondition.Add("[UserIP]", searchInfo.UserIP, ConditionType.Equal); 
             mssqlCondition.Add("[UserID]", searchInfo.UserID, ConditionType.Equal);
             mssqlCondition.Add("[UserName]", searchInfo.UserName, ConditionType.Like);
             mssqlCondition.Add("[AddDate]", searchInfo.AddDate, ConditionType.Equal);
        }

        /// <summary>
        /// 是否投过票
        /// </summary>
        /// <param name="VoteItemID">投票项目ID</param>
        /// <param name="IpAddress">投票IP</param>
        /// <returns></returns>
        public bool HasVoted(int VoteItemID,string IpAddress) {
            bool result = false;
           
            SqlParameter[] sps = { new SqlParameter("@voteItemID", VoteItemID), new SqlParameter("@ipAddress", IpAddress) };
            SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "HasVoted", sps);
            if (dr.HasRows) result=true;
            return result;
        }
        /// <summary>
        /// 投过票的记录
        /// </summary>
        /// <param name="VoteItemID">投票项目ID</param>
        /// <param name="IpAddress">投票IP</param>
        /// <returns></returns>
        public List<VoteRecordInfo> HasVotedList(int VoteItemID, string IpAddress)
        {
            List<VoteRecordInfo> voteRecordList = new List<VoteRecordInfo>();
            SqlParameter[] sps = { new SqlParameter("@voteItemID", VoteItemID), new SqlParameter("@ipAddress", IpAddress) };
           using( SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "HasVoted", sps))
           {
               PrepareVoteRecordModel(dr, voteRecordList);
           }
           return voteRecordList;
        }
    }
}