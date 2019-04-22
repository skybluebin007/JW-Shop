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
    public sealed class UserAccountRecordDAL : IUserAccountRecord
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserAccountRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO UsrAccountRecord( RecordType,Money,Point,Date,IP,Note,UserId,UserName) VALUES(@RecordType,@Money,@Point,@Date,@IP,@Note,@UserId,@UserName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public List<UserAccountRecordInfo> ReadList(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrAccountRecord where userId=@userId order by Id desc";

                return conn.Query<UserAccountRecordInfo>(sql, new { userId = userId }).ToList();
            }
        }

        public List<UserAccountRecordInfo> ReadList(int currentPage, int pageSize, AccountRecordType accountType, int userId, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "UsrAccountRecord";
                pc.Fields = "[Id], [RecordType], [Money], [Point], [Date], [IP], [Note], [UserId], [UserName]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;

                if ((int)accountType > 0) pc.MssqlCondition.Add("[RecordType]", (int)accountType, ConditionType.Equal);
                if (userId > 0) pc.MssqlCondition.Add("[UserId]", userId, ConditionType.Equal);

                count = pc.Count;
                return conn.Query<UserAccountRecordInfo>(pc).ToList();
            }
        }
        /// <summary>
        /// 积分明细（区分收入、支出）
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="accountType"></param>
        /// <param name="userId"></param>
        /// <param name="inCome">1-收入，0-支出</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UserAccountRecordInfo> ReadList(int currentPage, int pageSize, AccountRecordType accountType, int userId, int inCome,ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "UsrAccountRecord";
                pc.Fields = "[Id], [RecordType], [Money], [Point], [Date], [IP], [Note], [UserId], [UserName]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;

                if ((int)accountType > 0) pc.MssqlCondition.Add("[RecordType]", (int)accountType, ConditionType.Equal);
                if (userId > 0) pc.MssqlCondition.Add("[UserId]", userId, ConditionType.Equal);
                if (inCome > 0)
                {//查询收入
                    pc.MssqlCondition.Add("[Point]", 0, ConditionType.More);
                }
                else
                {//查询支出
                    pc.MssqlCondition.Add("[Point]", 0, ConditionType.Less);
                }
                count = pc.Count;
                return conn.Query<UserAccountRecordInfo>(pc).ToList();
            }
        }

        public int SumPoint(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select sum(point) from UsrAccountRecord where userId=@userId";

                return conn.ExecuteScalar<int>(sql, new { userId = userId });
            }
        }

        public bool HasGiftForLogin(int userId, string note)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from UsrAccountRecord where userId=@userId and note=@note and convert(varchar,[Date],112)=@tm";

                return conn.ExecuteScalar<int>(sql, new { userId = userId, note = note, tm = DateTime.Now.ToString("yyyyMMdd") }) > 0;
            }
        }

        /// <summary>
        /// 在指定的id前剩余的资金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public decimal ReadMoneyLeftBeforID(int id, int userID)
        {
            decimal moneyLeft = 0;
            SqlParameter[] parameters ={
				new SqlParameter("@id", SqlDbType.Int),
				new SqlParameter("@userID",SqlDbType.Int)
			};
            parameters[0].Value = id;
            parameters[1].Value = userID;
            Object oj = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "ReadMoneyLeftBeforID", parameters);
            if (oj != DBNull.Value)
            {
                moneyLeft = Convert.ToDecimal(oj);
            }
            return moneyLeft;
        }
        /// <summary>
        /// 在指定的id前剩余的积分
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int ReadPointLeftBeforID(int id, int userID)
        {
            int pointLeft = 0;
            SqlParameter[] parameters ={
				new SqlParameter("@id", SqlDbType.Int),
				new SqlParameter("@userID",SqlDbType.Int)
			};
            parameters[0].Value = id;
            parameters[1].Value = userID;
            Object oj = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "ReadPointLeftBeforID", parameters);
            if (oj != DBNull.Value)
            {
                pointLeft = Convert.ToInt32(oj);
            }
            return pointLeft;
        }
    }
}