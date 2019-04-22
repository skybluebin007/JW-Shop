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
    public sealed class ProductCollectDAL : IProductCollect
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductCollectInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductCollect( ProductId,Tm,UserId) VALUES(@ProductId,@Tm,@UserId);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Delete(int[] ids, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductCollect where id in @ids and userId=@userId";

                conn.Execute(sql, new { ids = ids, userId = userId });
            }
        }

        public ProductCollectInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductCollect where id=@id";

                var data = conn.Query<ProductCollectInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductCollectInfo();
            }
        }

        public ProductCollectInfo Read(int productId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductCollect where productId=@productId and userId=@userId";

                var data = conn.Query<ProductCollectInfo>(sql, new { productId = productId, userId = userId }).SingleOrDefault();
                return data ?? new ProductCollectInfo();
            }
        }

        public List<ProductCollectInfo> ReadList(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductCollect where productId=@productId";

                return conn.Query<ProductCollectInfo>(sql, new { productId = productId }).ToList();
            }
        }

        public List<ProductCollectInfo> ReadListByUserId(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductCollect where userId=@userId";

                return conn.Query<ProductCollectInfo>(sql, new { userId = userId }).ToList();
            }
        }

        /// <summary>
        /// 获得产品收藏数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>产品收藏数据列表</returns>
        public List<ProductCollectInfo> ReadList(int currentPage, int pageSize, ref int count, int userID)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "ProductCollect";
                pc.Fields = "[ID],[ProductID],[Tm],[UserID],[UserName]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[ID]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("[UserID]", userID, ConditionType.Equal);

                count = pc.Count;
                return conn.Query<ProductCollectInfo>(pc).ToList();
            }
        }

    }
}