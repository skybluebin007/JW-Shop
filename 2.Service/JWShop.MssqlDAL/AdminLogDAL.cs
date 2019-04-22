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
using System.Linq;
using Dapper;


namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 管理员日志数据层说明。
    /// </summary>
    public sealed class AdminLogDAL : IAdminLog
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(AdminLogInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO AdminLog( GroupId,Action,IP,AddDate,AdminId,AdminName) VALUES(@GroupId,@Action,@IP,@AddDate,@AdminId,@AdminName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(AdminLogInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE AdminLog SET GroupId = @GroupId, Action = @Action, IP = @IP, AddDate = @AddDate, AdminId = @AdminId, AdminName = @AdminName
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids, int adminId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from AdminLog where id in @ids";

                var para = new DynamicParameters();
                if (adminId > 0)
                {
                    sql += " and AdminId = @adminId";
                    para.Add("adminId", adminId);
                }
                para.Add("ids", ids);

                conn.Execute(sql, para);
            }
        }

        public void DeleteByAdminIds(int[] adminIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from AdminLog where AdminId in @adminIds";

                conn.Execute(sql, new { adminIds = adminIds });
            }
        }

        public void DeleteByGroupIds(int[] groupIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from AdminLog where GroupId in @groupIds";

                conn.Execute(sql, new { groupIds = groupIds });
            }
        }

        public AdminLogInfo Read(int id, int adminId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from AdminLog where id = @id";

                var para = new DynamicParameters();
                if (adminId > 0)
                {
                    sql += " and AdminId = @adminId";
                    para.Add("adminId", adminId);
                }
                para.Add("id", id);

                var data = conn.Query<AdminLogInfo>(sql, para).SingleOrDefault();
                return data ?? new AdminLogInfo();
            }
        }

        public List<AdminLogInfo> ReadList(int currentPage, int pageSize, ref int count, int adminId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "AdminLog";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("[AdminId]", adminId, ConditionType.Equal);

                count = pc.Count;
                return conn.Query<AdminLogInfo>(pc).ToList();
            }
        }

        public List<AdminLogInfo> ReadList(int currentPage, int pageSize, ref int count, string logType, DateTime startAddDate, DateTime endAddDate, int adminId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "AdminLog";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("[Action]", logType, ConditionType.Like);
                pc.MssqlCondition.Add("[AddDate]", startAddDate, ConditionType.MoreOrEqual);
                pc.MssqlCondition.Add("[AddDate]", endAddDate, ConditionType.Less);
                pc.MssqlCondition.Add("[AdminId]", adminId, ConditionType.Equal);

                count = pc.Count;
                return conn.Query<AdminLogInfo>(pc).ToList();
            }
        }
    }
}