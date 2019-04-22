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
    public sealed class ProductCommentDAL : IProductComment
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductCommentInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductComment( ProductId,Title,Content,UserIP,PostDate,Support,Against,Status,Rank,ReplyCount,AdminReplyContent,AdminReplyDate,UserId,UserName,OrderId,BuyDate) VALUES(@ProductId,@Title,@Content,@UserIP,@PostDate,@Support,@Against,@Status,@Rank,@ReplyCount,@AdminReplyContent,@AdminReplyDate,@UserId,@UserName,@OrderId,@BuyDate);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductCommentInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductComment SET ProductId = @ProductId, Title = @Title, Content = @Content, UserIP = @UserIP, PostDate = @PostDate, Support = @Support, Against = @Against, Status = @Status, Rank = @Rank, ReplyCount = @ReplyCount, AdminReplyContent = @AdminReplyContent, AdminReplyDate = @AdminReplyDate, UserId = @UserId, UserName = @UserName, OrderId = @OrderId, BuyDate = @BuyDate
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductComment where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductComment where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public ProductCommentInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductComment where id=@id";

                var data = conn.Query<ProductCommentInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductCommentInfo();
            }
        }

        public List<ProductCommentInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductComment";

                return conn.Query<ProductCommentInfo>(sql).ToList();
            }
        }

        #region search
        public List<ProductCommentInfo> SearchList(int currentPage, int pageSize, ProductCommentSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "ProductComment";
                pc.Fields = "[Id],[ProductId],[Title],[Content],[UserIP],[PostDate],[Support],[Against],[Status],[Rank],[ReplyCount],[AdminReplyContent],[AdminReplyDate],[UserId],[UserName],[OrderId],[BuyDate]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<ProductCommentInfo>(pc).ToList();
            }
        }
        public List<ProductCommentInfo> SearchInnerProductList(int currentPage, int pageSize, ProductCommentSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = " ProductComment INNER JOIN Product ON ProductComment.[ProductId]=Product.[Id] ";
                pc.Fields = "ProductComment.[Id],[ProductId],[Title],[Content],[UserIP],[PostDate],[Support],[Against],[Status],[Rank],[ReplyCount],[AdminReplyContent],[AdminReplyDate],[UserId],[UserName],[Name]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "ProductComment.[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<ProductCommentInfo>(pc).ToList();
            }
        }
        /// <summary>
        /// 搜索产品评论数据列表
        /// </summary>
        /// <param name="productCommentSearch">ProductCommentSearchInfo模型变量</param>
        /// <returns>产品评论数据列表</returns>
        public List<ProductCommentInfo> SearchProductCommentList(ProductCommentSearchInfo productCommentSearch)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductComment";

                string condition = PrepareCondition(productCommentSearch).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                sql += " order by id desc";
                return conn.Query<ProductCommentInfo>(sql).ToList();
            }
        }

        public MssqlCondition PrepareCondition(ProductCommentSearchInfo productCommentSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Name]", productCommentSearch.ProductName, ConditionType.Like);
            mssqlCondition.Add("[Title]", productCommentSearch.Title, ConditionType.Like);
            mssqlCondition.Add("[Content]", productCommentSearch.Content, ConditionType.Like);
            mssqlCondition.Add("[UserIP]", productCommentSearch.UserIP, ConditionType.Like);
            mssqlCondition.Add("[PostDate]", productCommentSearch.StartPostDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[PostDate]", productCommentSearch.EndPostDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[UserId]", productCommentSearch.UserId, ConditionType.Equal);
            mssqlCondition.Add("[Status]", productCommentSearch.Status, ConditionType.Equal);
            mssqlCondition.Add("[ProductId]", productCommentSearch.ProductId, ConditionType.Equal);
            mssqlCondition.Add("[OrderID]", productCommentSearch.OrderID, ConditionType.Equal);
            mssqlCondition.Add("[Rank]", productCommentSearch.Rank, ConditionType.In);
            return mssqlCondition;
        }
        #endregion

        public bool HasCommented(int orderId, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from ProductComment where userId=@userId and orderId=@orderId";

                return conn.ExecuteScalar<int>(sql, new { userId = userId, orderId = orderId }) > 0;
            }
        }

        public void ChangeStatus(int[] ids, int status)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE ProductComment SET [Status]=@status where id in @ids";

                conn.Execute(sql, new { ids = ids, status = status });
            }
        }

    }
}