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
	/// 管理组数据层说明。
	/// </summary>
	public sealed class AdminGroupDAL:IAdminGroup
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(AdminGroupInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO AdminGroup( Name,Power,AdminCount,AddDate,IP,Note) VALUES(@Name,@Power,@AdminCount,@AddDate,@IP,@Note);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(AdminGroupInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE AdminGroup SET Name = @Name, Power = @Power, AdminCount = @AdminCount, AddDate = @AddDate, IP = @IP, Note = @Note
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from AdminGroup where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

		public List<AdminGroupInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from AdminGroup";

                return conn.Query<AdminGroupInfo>(sql).ToList();
            }
		}

        public void ChangeCount(int id, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = action == ChangeAction.Plus
                    ? "UPDATE AdminGroup SET [AdminCount]=[AdminCount]+1 WHERE [Id] = @id"
                    : "UPDATE AdminGroup SET [AdminCount]=[AdminCount]-1 WHERE [Id] = @id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void ChangeCountByGeneral(int[] adminIds, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Execute(
                    "SocoShop_ChangeAdminGroupCountByGeneral",
                    new { strID = string.Join(",", adminIds), action = action.ToString() },
                    commandType: CommandType.StoredProcedure);
            }
        }
	}
}