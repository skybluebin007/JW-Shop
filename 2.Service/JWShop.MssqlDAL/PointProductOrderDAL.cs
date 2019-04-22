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
    public sealed class PointProductOrderDAL : IPointProductOrder
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(PointProductOrderInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO PointProductOrder( OrderNumber,OrderStatus,OrderNote,Point,ProductId,ProductName,BuyCount,Consignee,RegionId,Address,Tel,ShippingName,ShippingNumber,ShippingDate,AddDate,IP,UserId,UserName) VALUES(@OrderNumber,@OrderStatus,@OrderNote,@Point,@ProductId,@ProductName,@BuyCount,@Consignee,@RegionId,@Address,@Tel,@ShippingName,@ShippingNumber,@ShippingDate,@AddDate,@IP,@UserId,@UserName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(PointProductOrderInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE PointProductOrder SET OrderNumber = @OrderNumber, OrderStatus = @OrderStatus, OrderNote = @OrderNote, Point = @Point, ProductId = @ProductId, ProductName = @ProductName, BuyCount = @BuyCount, Consignee = @Consignee, RegionId = @RegionId, Address = @Address, Tel = @Tel, ShippingName = @ShippingName, ShippingNumber = @ShippingNumber, ShippingDate = @ShippingDate, AddDate = @AddDate, IP = @IP, UserId = @UserId, UserName = @UserName
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from PointProductOrder where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public PointProductOrderInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from PointProductOrder where id=@id";

                var data = conn.Query<PointProductOrderInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new PointProductOrderInfo();
            }
        }

        public List<PointProductOrderInfo> ReadList(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from PointProductOrder where userId=@userId";

                return conn.Query<PointProductOrderInfo>(sql, new { userId = userId }).ToList();
            }
        }

        public List<PointProductOrderInfo> SearchList(int currentPage, int pageSize, PointProductOrderSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "PointProductOrder";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Asc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<PointProductOrderInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(PointProductOrderSearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[OrderNumber]", searchInfo.OrderNumber, ConditionType.Like);
            mssqlCondition.Add("[OrderStatus]", searchInfo.OrderStatus, ConditionType.Equal);
            mssqlCondition.Add("[ProductName]", searchInfo.ProductName, ConditionType.Like);
            mssqlCondition.Add("[UserId]", searchInfo.UserId, ConditionType.Equal);
            mssqlCondition.Add("[UserName]", searchInfo.UserName, ConditionType.Like);
            mssqlCondition.Add("[AddDate]", searchInfo.StartAddDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[AddDate]", searchInfo.EndAddDate, ConditionType.LessOrEqual);

            return mssqlCondition;
        }
    }
}