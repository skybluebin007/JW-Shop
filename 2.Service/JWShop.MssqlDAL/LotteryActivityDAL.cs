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
   public sealed class LotteryActivityDAL:ILotteryActivity
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(LotteryActivityInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [LotteryActivity]([ActivityName],[ActivityType],[StartTime],[EndTime],[ActivityDesc],[ActivityKey],[ActivityPic],[PrizeSetting],[MaxNum],[GradeIds],[MinValue],[InvitationCode],[OpenTime],[IsOpened]) VALUES(@ActivityName,@ActivityType,@StartTime,@EndTime,@ActivityDesc,@ActivityKey,@ActivityPic,@PrizeSetting,@MaxNum,@GradeIds,@MinValue,@InvitationCode,@OpenTime,@IsOpened);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(LotteryActivityInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [LotteryActivity] SET [ActivityName]=@ActivityName,[ActivityType]=@ActivityType,[StartTime]=@StartTime,[EndTime]=@EndTime,[ActivityDesc]=@ActivityDesc,[ActivityKey]=@ActivityKey,[ActivityPic]=@ActivityPic,[PrizeSetting]=@PrizeSetting,[MaxNum]=@MaxNum,[GradeIds]=@GradeIds,[MinValue]=@MinValue,[InvitationCode]=@InvitationCode,[OpenTime]=@OpenTime,[IsOpened]=@IsOpened  
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [LotteryActivity] where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public LotteryActivityInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [LotteryActivity] where id=@id";

                var data = conn.Query<LotteryActivityInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new LotteryActivityInfo();
            }
        }

        public List<LotteryActivityInfo> SearchList(LotteryActivitySearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [LotteryActivity]";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                sql += " Order by [OrderId] desc,[RealDate] desc,[Id] desc";

                return conn.Query<LotteryActivityInfo>(sql).ToList();
            }
        }

        public List<LotteryActivityInfo> SearchList(int currentPage, int pageSize, LotteryActivitySearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "[LotteryActivity]";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<LotteryActivityInfo>(pc).ToList();
            }
        }


        public MssqlCondition PrepareCondition(LotteryActivitySearchInfo activitySearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[ActivityName]", activitySearch.ActivityName, ConditionType.Like);
            mssqlCondition.Add("[ActivityType]", activitySearch.ActivityType, ConditionType.Equal);
            mssqlCondition.Add("[ActivityKey]", activitySearch.ActivityKey, ConditionType.Like); ;
            mssqlCondition.Add("[ActivityDesc]",activitySearch.ActivityDesc, ConditionType.Like);
            mssqlCondition.Add("[StartTime]",activitySearch.StartTime, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[EndTime]", activitySearch.EndTime, ConditionType.LessOrEqual);
            mssqlCondition.Add("[IsOpened]", activitySearch.IsOpened, ConditionType.Equal);
         
         
            return mssqlCondition;
        }
        /// <summary>
        /// 检查key唯一性
        /// </summary>
        /// <param name="activityKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UniqueKey(string activityKey, int id = 0)
        {

            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from LotteryActivity where ActivityKey = @ActivityKey and id<>@Id";

                return conn.ExecuteScalar<int>(sql, new { ActivityKey = activityKey, Id = id }) < 1;
            }

        }

    }
}
