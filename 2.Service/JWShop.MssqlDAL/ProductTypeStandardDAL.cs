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
    public sealed class ProductTypeStandardDAL : IProductTypeStandard
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductTypeStandardInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductTypeStandard( ProductTypeId,Name,ValueList) VALUES(@ProductTypeId,@Name,@ValueList);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductTypeStandardInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeStandard SET ProductTypeId = @ProductTypeId, Name = @Name, ValueList = @ValueList
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeStandard where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int productTypeId, int[] notInIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeStandard where productTypeId=@productTypeId and id not in @notInIds";

                conn.Execute(sql, new { productTypeId = productTypeId, notInIds = notInIds });
            }
        }

        public void DeleteList(int productTypeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeStandard where productTypeId=@productTypeId";

                conn.Execute(sql, new { productTypeId = productTypeId });
            }
        }

        public ProductTypeStandardInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandard where id=@id";

                var data = conn.Query<ProductTypeStandardInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductTypeStandardInfo();
            }
        }
        public ProductTypeStandardInfo Read(string name, int productTypeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandard where Name=@name and productTypeId=@productTypeId";

                var data = conn.Query<ProductTypeStandardInfo>(sql, new { name = name, productTypeId=productTypeId }).SingleOrDefault();
                return data ?? new ProductTypeStandardInfo();
            }
        }

        public List<ProductTypeStandardInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandard";

                return conn.Query<ProductTypeStandardInfo>(sql).ToList();
            }
        }

        public List<ProductTypeStandardInfo> ReadList(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandard where id in @ids";

                return conn.Query<ProductTypeStandardInfo>(sql, new { ids = ids }).ToList();
            }
        }

        public List<ProductTypeStandardInfo> ReadList(int productTypeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandard where ProductTypeId=@productTypeId";

                return conn.Query<ProductTypeStandardInfo>(sql, new { productTypeId = productTypeId }).ToList();
            }
        }

    }
}