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
    public sealed class ShippingRegionDAL : IShippingRegion
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ShippingRegionInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ShippingRegion( Name,ShippingId,RegionId,FixedMoeny,FirstMoney,AgainMoney,OneMoeny,AnotherMoeny) VALUES(@Name,@ShippingId,@RegionId,@FixedMoeny,@FirstMoney,@AgainMoney,@OneMoeny,@AnotherMoeny);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ShippingRegionInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ShippingRegion SET Name = @Name, ShippingId = @ShippingId, RegionId = @RegionId, FixedMoeny = @FixedMoeny, FirstMoney = @FirstMoney, AgainMoney = @AgainMoney, OneMoeny = @OneMoeny, AnotherMoeny = @AnotherMoeny
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ShippingRegion where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ShippingRegion where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public ShippingRegionInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ShippingRegion where id=@id";

                var data = conn.Query<ShippingRegionInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ShippingRegionInfo();
            }
        }

        public List<ShippingRegionInfo> ReadList(int shippingId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ShippingRegion where shippingId = @shippingId";

                return conn.Query<ShippingRegionInfo>(sql, new { shippingId = shippingId }).ToList();
            }
        }

        public List<ShippingRegionInfo> ReadList(int[] shippingIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ShippingRegion where shippingId in @shippingIds";

                return conn.Query<ShippingRegionInfo>(sql, new { shippingIds = shippingIds }).ToList();
            }
        }

    }
}