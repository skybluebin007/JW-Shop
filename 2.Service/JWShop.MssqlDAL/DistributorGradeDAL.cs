using System;
using System.Collections.Generic;
using JWShop.IDAL;
using JWShop.Entity;
using System.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 分销商等级 数据层
    /// </summary>
    public sealed class DistributorGradeDAL : IDistributorGrade
    {
        private readonly string connectString=ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(DistributorGradeInfo entity)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [Distributor_Grade]([Title],[Min_Amount],[Max_Amount],[Discount]) VALUES(@Title,@Min_Amount,@Max_Amount,@Discount);
                                SELECT SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }           
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM [Distributor_Grade] WHERE [Id]=@Id";
                conn.Execute(sql, new { Id = id });
            }
        }
        public void Delete(int[] ids)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM [Distributor_Grade] WHERE [Id] IN @Ids";
                conn.Execute(sql, new { Ids = ids });
            }
        }
        public DistributorGradeInfo Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [Distributor_Grade] WHERE [Id]=@Id";
              return  conn.Query<DistributorGradeInfo>(sql, new { Id = id }).SingleOrDefault()??new DistributorGradeInfo();
            }
        }

        public List<DistributorGradeInfo> ReadList()
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [Distributor_Grade]";
                return conn.Query<DistributorGradeInfo>(sql).ToList();
            }
        }

        public void Update(DistributorGradeInfo entity)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [Distributor_Grade] SET [Title]=@Title,[Min_Amount]=@Min_Amount,[Max_Amount]=@Max_Amount,[Discount]=@Discount WHERE [Id]=@Id";
                 conn.Execute(sql, entity);
            }
        }
    }
}
