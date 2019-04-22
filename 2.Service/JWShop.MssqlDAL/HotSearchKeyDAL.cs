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
    public sealed class HotSearchKeyDAL : IHotSearchKey
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(HotSearchKeyInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [HotSearchKey](Name,[SearchTimes],[AddCol1],[AddCol2]) VALUES(@Name,@SearchTimes,@AddCol1,@AddCol2);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(HotSearchKeyInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [HotSearchKey] SET Name = @Name, SearchTimes = @SearchTimes, AddCol1 = @AddCol1, AddCol2 = @AddCol2 where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [HotSearchKey] where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public HotSearchKeyInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [HotSearchKey] where id=@id";

                var data = conn.Query<HotSearchKeyInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new HotSearchKeyInfo();
            }
        }
        public HotSearchKeyInfo Read(string name)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [HotSearchKey] where [Name]=@name";

                var data = conn.Query<HotSearchKeyInfo>(sql, new { name = name }).SingleOrDefault();
                return data ?? new HotSearchKeyInfo();
            }
        }
        public List<HotSearchKeyInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [HotSearchKey] order by [SearchTimes] desc,[Id] asc";

                return conn.Query<HotSearchKeyInfo>(sql).ToList();
            }
        }
       
    }
}
