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
    public sealed class ShippingDAL : IShipping
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ShippingInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO Shipping( Name,[Description],IsEnabled,ShippingType,FirstWeight,AgainWeight,OrderId,ShippingCode) VALUES(@Name,@Description,@IsEnabled,@ShippingType,@FirstWeight,@AgainWeight,@OrderId,@ShippingCode);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ShippingInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE Shipping SET Name = @Name, [Description] = @Description, IsEnabled = @IsEnabled, ShippingType = @ShippingType, FirstWeight = @FirstWeight, AgainWeight = @AgainWeight, OrderId = @OrderId,ShippingCode=@ShippingCode
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Shipping where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Shipping where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public ShippingInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Shipping where id=@id";

                var data = conn.Query<ShippingInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ShippingInfo();
            }
        }

        public List<ShippingInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Shipping order by orderId";

                return conn.Query<ShippingInfo>(sql).ToList();
            }
        }

        public void Move(ChangeAction action, int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_Shipping", new
                {
                    action = action.ToString(),
                    id = id
                }, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }

    }
}