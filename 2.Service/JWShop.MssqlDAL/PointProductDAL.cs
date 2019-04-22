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
    public sealed class PointProductDAL : IPointProduct
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(PointProductInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO PointProduct( Name,SubTitle,MarketPrice,Point,Photo,Keywords,Summary,Introduction1,Introduction1_Mobile,IsSale,ViewCount,TotalStorageCount,SendCount,AddDate,OrderId,BeginDate,EndDate) VALUES(@Name,@SubTitle,@MarketPrice,@Point,@Photo,@Keywords,@Summary,@Introduction1,@Introduction1_Mobile,@IsSale,@ViewCount,@TotalStorageCount,@SendCount,@AddDate,@OrderId,@BeginDate,@EndDate);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(PointProductInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE PointProduct SET Name = @Name, SubTitle = @SubTitle, MarketPrice = @MarketPrice, Point = @Point, Photo = @Photo, Keywords = @Keywords, Summary = @Summary, Introduction1 = @Introduction1, Introduction1_Mobile = @Introduction1_Mobile, IsSale = @IsSale, ViewCount = @ViewCount, TotalStorageCount = @TotalStorageCount, SendCount = @SendCount, AddDate = @AddDate, OrderId = @OrderId, BeginDate = @BeginDate, EndDate = @EndDate
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from PointProduct where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public PointProductInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from PointProduct where id=@id";

                var data = conn.Query<PointProductInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new PointProductInfo();
            }
        }

        public List<PointProductInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from PointProduct";

                return conn.Query<PointProductInfo>(sql).ToList();
            }
        }

        public List<PointProductInfo> ReadList(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from PointProduct where id in @ids";

                return conn.Query<PointProductInfo>(sql, new { ids = ids }).ToList();
            }
        }

        #region search
        public List<PointProductInfo> SearchList(int currentPage, int pageSize, PointProductSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "PointProduct";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[OrderId],[Id]";
                pc.OrderType = OrderType.Asc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<PointProductInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(PointProductSearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Id]", searchInfo.ProductId, ConditionType.Equal);
            mssqlCondition.Add("[Name]", searchInfo.ProductName, ConditionType.Like);
            mssqlCondition.Add("[Point]", searchInfo.Point, ConditionType.Equal);
            mssqlCondition.Add("[BeginDate]", searchInfo.BeginDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[EndDate]", searchInfo.EndDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[IsSale]", searchInfo.IsSale, ConditionType.Equal);
            mssqlCondition.Add("[BeginDate]", searchInfo.ValidDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[EndDate]", searchInfo.ValidDate, ConditionType.MoreOrEqual);

            return mssqlCondition;
        }
        #endregion

        public void OffSale(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE PointProduct SET [IsSale]=0 WHERE [Id] IN @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void OnSale(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE PointProduct SET [IsSale]=1 WHERE [Id] IN @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void ChangeSendCount(int id, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE PointProduct SET [SendCount] = isnull([SendCount],0) + 1 WHERE [Id] = @id";
                if (action == ChangeAction.Minus)
                {
                    sql = @"UPDATE PointProduct 
                            SET SendCount = CASE WHEN SendCount > 0 THEN SendCount - 1 ELSE 0 END 
                            where Id=@id";
                }

                conn.Execute(sql, new { id = id });
            }
        }
    }
}