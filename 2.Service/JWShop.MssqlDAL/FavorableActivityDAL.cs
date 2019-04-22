using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class FavorableActivityDAL : IFavorableActivity
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(FavorableActivityInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO FavorableActivity( Name,Photo,Content,StartDate,EndDate,UserGrade,OrderProductMoney,RegionId,ShippingWay,ReduceWay,ReduceMoney,ReduceDiscount,GiftId,[Type],[ClassIds]) VALUES(@Name,@Photo,@Content,@StartDate,@EndDate,@UserGrade,@OrderProductMoney,@RegionId,@ShippingWay,@ReduceWay,@ReduceMoney,@ReduceDiscount,@GiftId,@Type,@ClassIds);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(FavorableActivityInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE FavorableActivity SET Name = @Name, Photo = @Photo, Content = @Content, StartDate = @StartDate, EndDate = @EndDate, UserGrade = @UserGrade, OrderProductMoney = @OrderProductMoney, RegionId = @RegionId, ShippingWay = @ShippingWay, ReduceWay = @ReduceWay, ReduceMoney = @ReduceMoney, ReduceDiscount = @ReduceDiscount, GiftId = @GiftId,[Type]=@Type,[ClassIds]=@ClassIds
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from FavorableActivity where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public FavorableActivityInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from FavorableActivity where Id=@id";

                var data = conn.Query<FavorableActivityInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new FavorableActivityInfo();
            }
        }

        public FavorableActivityInfo Read(DateTime startDate, DateTime endDate, int id = 0)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select top 1 * from FavorableActivity where ([StartDate]<=@startDate AND [EndDate]>@startDate) OR ([StartDate]>=@startDate AND [StartDate]<@endDate)";

                var para = new DynamicParameters();
                if (id > 0)
                {
                    sql = "select top 1 * from FavorableActivity where (([StartDate]<=@startDate AND [EndDate]>@startDate) OR ([StartDate]>=@startDate AND [StartDate]<@endDate)) and Id <> @id";
                    para.Add("id", id);
                }
                para.Add("startDate", startDate);
                para.Add("endDate", endDate);

                var data = conn.Query<FavorableActivityInfo>(sql, para).SingleOrDefault();
                return data ?? new FavorableActivityInfo();
            }
        }
        /// <summary>
        /// 读取一段时间内的优惠活动列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<FavorableActivityInfo> ReadList(DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from FavorableActivity where ([StartDate]<=@startDate AND [EndDate]>@startDate) OR ([StartDate]>=@startDate AND [StartDate]<@endDate)";

                var para = new DynamicParameters();
                para.Add("startDate", startDate);
                para.Add("endDate", endDate);

                return  conn.Query<FavorableActivityInfo>(sql, para).ToList();
               
            }
        }

        public List<FavorableActivityInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "FavorableActivity";
                pc.Fields = "[Id], [Name], [Photo], [Content], [StartDate], [EndDate], [UserGrade], [OrderProductMoney], [RegionId], [ShippingWay], [ReduceWay], [ReduceMoney], [ReduceDiscount], [GiftId],[Type] ";

                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";

                count = pc.Count;
                return conn.Query<FavorableActivityInfo>(pc).ToList();
            }
        }
        /// <summary>
        /// 根据有效期状态查询
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="timePeriod">状态：未开始 1，进行中 2，已结束 3</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<FavorableActivityInfo> ReadList(int currentPage, int pageSize, ref int count, int timePeriod=2)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "FavorableActivity";
                pc.Fields = "[Id], [Name], [Photo], [Content], [StartDate], [EndDate], [UserGrade], [OrderProductMoney], [RegionId], [ShippingWay], [ReduceWay], [ReduceMoney], [ReduceDiscount], [GiftId],[Type] ";

                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                //未开始
                if (timePeriod == 1)
                {
                    pc.MssqlCondition.Add(" DATEDIFF ( day  , getdate(), [StartDate] )>0");
                }
                //进行中
                if (timePeriod == 2)
                {
                    pc.MssqlCondition.Add(" DATEDIFF(day,getdate(),[StartDate])<=0  and  DATEDIFF(day,getdate(),[EndDate])>=0");
                }
                //已结束
                if (timePeriod == 3)
                {
                    pc.MssqlCondition.Add(" DATEDIFF(day,getdate(),[EndDate])<0");
                }
                count = pc.Count;
                return conn.Query<FavorableActivityInfo>(pc).ToList();
            }
        }

    }
}