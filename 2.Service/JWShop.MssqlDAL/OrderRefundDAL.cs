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
    public sealed class OrderRefundDAL : IOrderRefund
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OrderRefundInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OrderRefund( RefundNumber,OrderId,OrderNumber,OrderDetailId,RefundCount,RefundBalance,RefundMoney,RefundPayKey,RefundPayName,BatchNo,Status,TmCreate,TmRefund,RefundRemark,Remark,IsCommented,TmComment,CommentContent,UserType,UserId,UserName,OwnerId) VALUES(@RefundNumber,@OrderId,@OrderNumber,@OrderDetailId,@RefundCount,@RefundBalance,@RefundMoney,@RefundPayKey,@RefundPayName,@BatchNo,@Status,@TmCreate,@TmRefund,@RefundRemark,@Remark,@IsCommented,@TmComment,@CommentContent,@UserType,@UserId,@UserName,@OwnerId);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(OrderRefundInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OrderRefund SET RefundNumber = @RefundNumber, OrderId = @OrderId, OrderNumber = @OrderNumber, OrderDetailId = @OrderDetailId, RefundCount = @RefundCount, RefundBalance = @RefundBalance, RefundMoney = @RefundMoney, RefundPayKey = @RefundPayKey, RefundPayName = @RefundPayName, BatchNo = @BatchNo, Status = @Status, TmCreate = @TmCreate, TmRefund = @TmRefund, RefundRemark = @RefundRemark, Remark = @Remark, IsCommented = @IsCommented, TmComment = @TmComment, CommentContent = @CommentContent, UserType = @UserType, UserId = @UserId, UserName = @UserName, OwnerId = @OwnerId
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void UpdateBatchNo(int id, string batchNo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OrderRefund SET BatchNo = @batchNo where Id=@id";

                conn.Execute(sql, new { id = id, batchNo = batchNo });
            }
        }

        public void Comment(int id, string content)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OrderRefund SET IsCommented = 1, TmComment = @TmComment, CommentContent = @CommentContent
                            where Id=@Id and IsCommented = 0";

                conn.Execute(sql, new { TmComment = DateTime.Now, CommentContent = content, Id = id });
            }
        }

        public OrderRefundInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefund where id=@id";

                var data = conn.Query<OrderRefundInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new OrderRefundInfo();
            }
        }

        public OrderRefundInfo ReadByBatchNo(string batchNo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefund where BatchNo=@batchNo";

                var data = conn.Query<OrderRefundInfo>(sql, new { batchNo = batchNo }).SingleOrDefault();
                return data ?? new OrderRefundInfo();
            }
        }

        public List<OrderRefundInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefund";

                return conn.Query<OrderRefundInfo>(sql).ToList();
            }
        }

        public List<OrderRefundInfo> ReadList(int orderId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefund where orderId = @orderId order by Id desc";

                return conn.Query<OrderRefundInfo>(sql, new { orderId = orderId }).ToList();
            }
        }

        public List<OrderRefundInfo> ReadList(int[] orderIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefund where orderId in @orderIds";

                return conn.Query<OrderRefundInfo>(sql, new { orderIds = orderIds }).ToList();
            }
        }

        public List<OrderRefundInfo> ReadListByOwnerId(int ownerId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderRefund where OwnerId = @ownerId order by Id desc";

                return conn.Query<OrderRefundInfo>(sql, new { ownerId = ownerId }).ToList();
            }
        }

        public List<OrderRefundInfo> SearchList(int currentPage, int pageSize, OrderRefundSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "OrderRefund";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<OrderRefundInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(OrderRefundSearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[RefundNumber]", searchInfo.RefundNumber, ConditionType.Like);
            mssqlCondition.Add("[OrderNumber]", searchInfo.OrderNumber, ConditionType.Like);
            mssqlCondition.Add("[Status]", searchInfo.Status, ConditionType.Equal);
            mssqlCondition.Add("[TmCreate]", searchInfo.StartTmCreate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[TmCreate]", searchInfo.EndTmCreate, ConditionType.Less);
            mssqlCondition.Add("[OwnerId]", searchInfo.OwnerId, ConditionType.Equal);

            return mssqlCondition;
        }

    }
}