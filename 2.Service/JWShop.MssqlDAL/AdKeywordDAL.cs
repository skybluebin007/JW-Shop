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
    public sealed class AdKeywordDAL : IAdKeyword
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(AdKeywordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO AdKeyword( Name,Url,OrderId,Remark,Tm) VALUES(@Name,@Url,@OrderId,@Remark,@Tm);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(AdKeywordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE AdKeyword SET Name = @Name, Url = @Url, OrderId = @OrderId, Remark = @Remark, Tm = @Tm
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from AdKeyword where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public AdKeywordInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from AdKeyword where id=@id";

                var data = conn.Query<AdKeywordInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new AdKeywordInfo();
            }
        }

        public List<AdKeywordInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from AdKeyword order by OrderId";

                return conn.Query<AdKeywordInfo>(sql).ToList();
            }
        }
    }
}