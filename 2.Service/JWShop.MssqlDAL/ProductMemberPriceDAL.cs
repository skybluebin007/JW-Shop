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
    public sealed class ProductMemberPriceDAL : IProductMemberPrice
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductMemberPriceInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductMemberPrice( ProductId,GradeId,Price) VALUES(@ProductId,@GradeId,@Price);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void DeleteByGradeId(int[] gradeIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductMemberPrice where GradeId in @gradeIds";

                conn.Execute(sql, new { gradeIds = gradeIds });
            }
        }

        public void DeleteByProductId(int[] productIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductMemberPrice where ProductId in @productIds";

                conn.Execute(sql, new { productIds = productIds });
            }
        }

        public List<ProductMemberPriceInfo> ReadList(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductMemberPrice where ProductId = @productId";

                return conn.Query<ProductMemberPriceInfo>(sql, new { productId = productId }).ToList();
            }
        }

        public List<ProductMemberPriceInfo> ReadList(int[] productIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductMemberPrice where ProductId in @productIds";

                return conn.Query<ProductMemberPriceInfo>(sql, new { productIds = productIds }).ToList();
            }
        }

        public List<ProductMemberPriceInfo> ReadList(int[] productIds, int gradeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductMemberPrice where ProductId in @productIds and GradeId = @gradeId";

                return conn.Query<ProductMemberPriceInfo>(sql, new { productIds = productIds, gradeId = gradeId }).ToList();
            }
        }
    }
}