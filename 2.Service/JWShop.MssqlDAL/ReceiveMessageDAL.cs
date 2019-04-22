using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class ReceiveMessageDAL : IReceiveMessage
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        public int Add(ReceiveMessageInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [ReceiveMessage]([Title],[Content],[Date],[IsRead],[IsAdmin],[FromUserID],[FromUserName],[UserID],[UserName]) VALUES(@Title,@Content,@Date,@IsRead,@IsAdmin,@FromUserID,@FromUserName,@UserID,@UserName);
                            select SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ReceiveMessageInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [ReceiveMessage] SET [Title]=@Title,[Content]=@Content,[Date]=@Date,[IsRead]=@IsRead,[IsAdmin]=@IsAdmin,[FromUserID]=@FromUserID,[FromUserName]=@FromUserName,[UserID]=@UserID,[UserName]=@UserName  where [ID]=@Id";
                conn.Execute(sql, entity);
            }
        }
   
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [ReceiveMessage] where [ID]=@id";

                conn.Execute(sql, new { id = id });
            }
        }
        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [ReceiveMessage] where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }
        public ReceiveMessageInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [ReceiveMessage] where [ID]=@id";

                var data = conn.Query<ReceiveMessageInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ReceiveMessageInfo();
            }
        }

        public List<ReceiveMessageInfo> SearchList(ReceiveMessageSearchInfo searchEntity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [ReceiveMessage]";
                string condition = PrepareCondition(searchEntity).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                sql += " Order by [ID] desc";

                return conn.Query<ReceiveMessageInfo>(sql).ToList();
            }
        }

        public List<ReceiveMessageInfo> SearchList(int currentPage, int pageSize,ReceiveMessageSearchInfo searchEntity, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "ReceiveMessage";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchEntity);

                count = pc.Count;
                return conn.Query<ReceiveMessageInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(ReceiveMessageSearchInfo searchEntity)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Title]", searchEntity.Title, ConditionType.Like);
            mssqlCondition.Add("[UserID]", searchEntity.UserID, ConditionType.Equal);
            mssqlCondition.Add("[Date]", searchEntity.StartDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[Date]", searchEntity.EndDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[IsRead]", searchEntity.IsRead, ConditionType.Equal);
            return mssqlCondition;
        }
    }
}
