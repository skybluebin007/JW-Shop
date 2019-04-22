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
    public sealed class ProductTypeDAL : IProductType
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductTypeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductType(Name,BrandIds) VALUES(@Name,@BrandIds);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductTypeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductType SET Name = @Name, BrandIds = @BrandIds
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductType where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public ProductTypeInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductType where id=@id";

                var data = conn.Query<ProductTypeInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductTypeInfo();
            }
        }

        public List<ProductTypeInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductType";

                return conn.Query<ProductTypeInfo>(sql).ToList();
            }
        }

    }
}