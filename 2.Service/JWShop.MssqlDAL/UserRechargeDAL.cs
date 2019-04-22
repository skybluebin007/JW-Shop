using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class UserRechargeDAL : IUserRecharge
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserRechargeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO UsrRecharge( Number,Money,PayKey,PayName,RechargeDate,RechargeIP,IsFinish,UserId) VALUES(@Number,@Money,@PayKey,@PayName,@RechargeDate,@RechargeIP,@IsFinish,@UserId);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(UserRechargeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE UsrRecharge SET Number = @Number, Money = @Money, PayKey = @PayKey, PayName = @PayName, RechargeDate = @RechargeDate, RechargeIP = @RechargeIP, IsFinish = @IsFinish, UserId = @UserId
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from UsrRecharge where id=@id and userId=@userId";

                conn.Execute(sql, new { id = id });
            }
        }

        public UserRechargeInfo Read(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrRecharge where id=@id and userId=@userId";

                var data = conn.Query<UserRechargeInfo>(sql, new { id = id, userId = userId }).SingleOrDefault();
                return data ?? new UserRechargeInfo();
            }
        }

        public UserRechargeInfo Read(string number, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrRecharge where number=@number and userId=@userId";

                var data = conn.Query<UserRechargeInfo>(sql, new { number = number, userId = userId }).SingleOrDefault();
                return data ?? new UserRechargeInfo();
            }
        }
        public UserRechargeInfo Read(string number)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrRecharge where number=@number";

                var data = conn.Query<UserRechargeInfo>(sql, new { number = number}).SingleOrDefault();
                return data ?? new UserRechargeInfo();
            }
        }
        public List<UserRechargeInfo> ReadList(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrRecharge where userId = @userId order by Id desc";

                return conn.Query<UserRechargeInfo>(sql, new { userId = userId }).ToList();
            }
        }

        #region Search
        public List<UserRechargeInfo> SearchList(int currentPage, int pageSize, UserRechargeSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "UsrRecharge";
                pc.Fields = "[Id], [Number], [Money], [PayKey], [PayName], [RechargeDate], [RechargeIP], [IsFinish], [UserId] ";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<UserRechargeInfo>(pc).ToList();
            }
        }

        private MssqlCondition PrepareCondition(UserRechargeSearchInfo userRechargeSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Number]", userRechargeSearch.Number, ConditionType.Like);
            mssqlCondition.Add("[RechargeDate]", userRechargeSearch.StartRechargeDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[RechargeDate]", userRechargeSearch.EndRechargeDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[IsFinish]", userRechargeSearch.IsFinish, ConditionType.Equal);
            mssqlCondition.Add("[UserId]", userRechargeSearch.InUserId, ConditionType.In);

            return mssqlCondition;
        }
        #endregion

    }
}