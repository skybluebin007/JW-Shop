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
    public sealed class OrderActionDAL : IOrderAction
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OrderActionInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OrderAction( OrderId,OrderOperate,StartOrderStatus,EndOrderStatus,Note,IP,Date,AdminID,AdminName) VALUES(@OrderId,@OrderOperate,@StartOrderStatus,@EndOrderStatus,@Note,@IP,@Date,@AdminID,@AdminName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OrderAction where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void DeleteByOrderId(int[] orderIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OrderAction where orderId in @orderIds";

                conn.Execute(sql, new { orderIds = orderIds });
            }
        }

        public OrderActionInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderAction where id=@id";

                var data = conn.Query<OrderActionInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new OrderActionInfo();
            }
        }

        public OrderActionInfo ReadLast(int orderId, int orderStatus)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT top 1 [Id],[OrderId],[OrderOperate],[StartOrderStatus],[EndOrderStatus],[Note],[IP],[Date],[AdminID],[AdminName] FROM OrderAction WHERE [OrderId]=@orderId AND [EndOrderStatus]= @orderStatus AND [OrderOperate]<>8  Order By Id DESC";

                var data = conn.Query<OrderActionInfo>(sql, new { orderId = orderId, orderStatus = orderStatus }).SingleOrDefault();
                return data ?? new OrderActionInfo();
            }
        }

        public List<OrderActionInfo> ReadList(int orderId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderAction where orderId = @orderId";

                return conn.Query<OrderActionInfo>(sql, new { orderId = orderId }).ToList();
            }
        }

        public List<OrderActionInfo> ReadListLastDate(int[] orderIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select OrderId, MAX([Date]) as [Date] from [OrderAction] where OrderId in @orderIds group by OrderId";

                return conn.Query<OrderActionInfo>(sql, new { orderIds = orderIds }).ToList();
            }
        }

    }
}