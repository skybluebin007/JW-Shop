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
    public sealed class OrderRefundActionDAL : IOrderRefundAction
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OrderRefundActionInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OrderRefundAction( OrderRefundId,Status,Remark,Tm,UserType,UserId,UserName) VALUES(@OrderRefundId,@Status,@Remark,@Tm,@UserType,@UserId,@UserName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public List<OrderRefundActionInfo> ReadList(int orderRefundId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefundAction where orderRefundId = @orderRefundId order by Id desc";

                return conn.Query<OrderRefundActionInfo>(sql, new { orderRefundId = orderRefundId }).ToList();
            }
        }

    }
}