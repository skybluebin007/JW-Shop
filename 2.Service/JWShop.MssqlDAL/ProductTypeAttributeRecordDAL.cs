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
    public sealed class ProductTypeAttributeRecordDAL : IProductTypeAttributeRecord
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public void Add(ProductTypeAttributeRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductTypeAttributeRecord( ProductId,AttributeId,Value) VALUES(@ProductId,@AttributeId,@Value)";

                conn.Execute(sql, entity);
            }
        }

        public void Update(ProductTypeAttributeRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeAttributeRecord SET ProductId = @ProductId, AttributeId = @AttributeId, Value = @Value
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeAttributeRecord where productId=@productId";

                conn.Execute(sql, new { productId = productId });
            }
        }

        public void DeleteByAttr(int attrId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeAttributeRecord where attributeId=@attrId";

                conn.Execute(sql, new {attrId = attrId });
            }
        }

        public List<ProductTypeAttributeRecordInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeAttributeRecord";

                return conn.Query<ProductTypeAttributeRecordInfo>(sql).ToList();
            }
        }

        public List<ProductTypeAttributeRecordInfo> ReadList(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select a.*,b.Name as AttributeName from ProductTypeAttributeRecord a, ProductTypeAttribute b where a.AttributeId=b.Id and a.ProductId=@productId";

                return conn.Query<ProductTypeAttributeRecordInfo>(sql, new { productId = productId }).ToList();
            }
        }

    }
}