using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

namespace JWShop.MssqlDAL
{
    public sealed class PrizeRecordDAL : IPrizeRecord
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(PrizeRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [PrizeRecord](ActivityID,PrizeTime,UserID,PrizeName,Prizelevel,IsPrize,RealName,Company,CellPhone) VALUES(@ActivityID,@PrizeTime,@UserID,@PrizeName,@Prizelevel,@IsPrize,@RealName,@Company,@CellPhone);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }
        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [PrizeRecord] where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void UpdatePrizeRecord(PrizeRecordInfo model)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [PrizeRecord] SET  RealName=@RealName, CellPhone=@CellPhone,[Company]=@Company WHERE ActivityID=@ActivityID AND UserID=@UserID AND IsPrize = 1 AND CellPhone IS NULL AND RealName IS NULL And Company IS NULL";
                conn.Execute(sql, model);
            }
        }
        public int GetCountByActivityId(int activityId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(*) from [PrizeRecord] where ActivityId=@ActivityId";
                return conn.ExecuteScalar<int>(sql, new { ActivityId = activityId});
            }   
          
        }
        /// <summary>
        /// 获取当日（月）用户抽奖记录
        /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>抽奖记录次数int</returns>
        public int GetUserPrizeCountToday(int activityId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {             
                //一天内只能抽一次
                string sql = "select count(*) from [PrizeRecord] where ActivityId=@ActivityId  and UserID=@UserID and datediff(\"day\",prizetime,getdate())=0";
                //一个月内只能抽一次
                //builder.Append("select count(*) from Vshop_PrizeRecord where ActivityId=@ActivityId  and UserID=@UserID and datediff(\"mm\",prizetime,getdate())=0");
                return conn.ExecuteScalar<int>(sql, new { ActivityId = activityId, UserID = userId });
            }
        }
       /// <summary>
        /// 获取抽奖记录
       /// </summary>
       /// <param name="activityId">活动id</param>
       /// <param name="userId">会员id</param>
        /// <returns>抽奖记录次数int</returns>
        public int GetUserPrizeCount(int activityId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {   
                string sql = "select count(*) from [PrizeRecord] where ActivityId=@ActivityId  and UserID=@UserID";               
                return conn.ExecuteScalar<int>(sql, new { ActivityId = activityId, UserID = userId });
            }         
        }
       /// <summary>
       /// 获取最新一次抽奖记录
       /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>抽奖记录次数int</returns>
        public PrizeRecordInfo GetLatestUserPrizeRecord(int activityId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select top 1 *  from [PrizeRecord] where ActivityId=@ActivityId  and UserID=@UserID";               
                var data = conn.Query<PrizeRecordInfo>(sql, new { ActivityId = activityId, UserID = userId }).SingleOrDefault();
                return data ?? new PrizeRecordInfo();
            }      
          
        }
        /// <summary>
        /// 获取用户本次活动的所有抽奖记录
        /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>所有抽奖记录列表</returns>
        public List<PrizeRecordInfo> GetPrizeList(int activityId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from PrizeRecord where ActivityID=@activityID and UserID=@userID";
                return conn.Query<PrizeRecordInfo>(sql, new { activityID = activityId, userID = userId }).ToList();
            }
        }
        public bool HasSignUp(int activityId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(*) from PrizeRecord where ActivityID=@ActivityID and UserID=@UserID";
                return conn.ExecuteScalar<int>(sql, new { ActivityId = activityId, UserID = userId })>0;
            }           
        }
        public List<PrizeRecordInfo> SearchList(PrizeRecordSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [PrizeRecord]";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                sql += " Order by [RecordId] desc";

                return conn.Query<PrizeRecordInfo>(sql).ToList();
            }
        }

        public List<PrizeRecordInfo> SearchList(int currentPage, int pageSize, PrizeRecordSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "[PrizeRecord]";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[RecordId]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<PrizeRecordInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(PrizeRecordSearchInfo recordSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            mssqlCondition.Add("[ActivityID]", recordSearch.ActivityID, ConditionType.Equal);
            if (recordSearch.UserID>0) mssqlCondition.Add("[UserID]", recordSearch.UserID, ConditionType.Equal); ;
            if (recordSearch.IsPrize > 0) mssqlCondition.Add("[IsPrize]", recordSearch.IsPrize, ConditionType.Equal);
            return mssqlCondition;
        }
    }
}
