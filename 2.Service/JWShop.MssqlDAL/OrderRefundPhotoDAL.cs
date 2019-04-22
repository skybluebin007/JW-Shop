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
    public sealed class OrderRefundPhotoDAL : IOrderRefundPhoto
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OrderRefundPhotoInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OrderRefundPhoto( OrderRefundId,ImageUrl,Remark) VALUES(@OrderRefundId,@ImageUrl,@Remark);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OrderRefundPhoto where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public OrderRefundPhotoInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefundPhoto where id=@id";

                var data = conn.Query<OrderRefundPhotoInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new OrderRefundPhotoInfo();
            }
        }

        public List<OrderRefundPhotoInfo> ReadList(int orderRefundId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefundPhoto where orderRefundId = @orderRefundId";

                return conn.Query<OrderRefundPhotoInfo>(sql, new { orderRefundId = orderRefundId }).ToList();
            }
        }

    }
}