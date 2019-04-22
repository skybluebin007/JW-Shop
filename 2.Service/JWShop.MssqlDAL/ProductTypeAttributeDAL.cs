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
    public sealed class ProductTypeAttributeDAL : IProductTypeAttribute
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductTypeAttributeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductTypeAttribute(ProductTypeId,Name,InputType,InputValue,OrderId) VALUES(@ProductTypeId,@Name,@InputType,@InputValue,@OrderId);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductTypeAttributeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeAttribute SET ProductTypeId = @ProductTypeId, Name = @Name, InputType = @InputType, InputValue = @InputValue, OrderId = @OrderId
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeAttribute where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int productTypeId, int[] notInIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeAttribute where productTypeId=@productTypeId and id not in @notInIds";

                conn.Execute(sql, new { productTypeId = productTypeId, notInIds = notInIds });
            }
        }

        public void DeleteList(int productTypeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeAttribute where productTypeId=@productTypeId";

                conn.Execute(sql, new { productTypeId = productTypeId });
            }
        }

        public ProductTypeAttributeInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeAttribute where id=@id";

                var data = conn.Query<ProductTypeAttributeInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductTypeAttributeInfo();
            }
        }
        public ProductTypeAttributeInfo Read(string name,int productTypeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeAttribute where Name=@name and ProductTypeId = @ProductTypeId";

                var data = conn.Query<ProductTypeAttributeInfo>(sql, new { name = name, ProductTypeId=productTypeId }).SingleOrDefault();
                return data ?? new ProductTypeAttributeInfo();
            }
        }
        public List<ProductTypeAttributeInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeAttribute order by OrderId";

                return conn.Query<ProductTypeAttributeInfo>(sql).ToList();
            }
        }

        public List<ProductTypeAttributeInfo> ReadList(int productTypeId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeAttribute where ProductTypeId=@productTypeId order by OrderId";

                return conn.Query<ProductTypeAttributeInfo>(sql, new { productTypeId = productTypeId }).ToList();
            }
        }

    }
}