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
    public sealed class OrderDetailDAL : IOrderDetail
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OrderDetailInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OrderDetail( OrderId,ProductId,ProductName,StandardValueList,ProductWeight,SendPoint,ProductPrice,BidPrice,ActivityPoint,BuyCount,ParentId,RandNumber,GiftPackId,RefundCount) VALUES(@OrderId,@ProductId,@ProductName,@StandardValueList,@ProductWeight,@SendPoint,@ProductPrice,@BidPrice,@ActivityPoint,@BuyCount,@ParentId,@RandNumber,@GiftPackId,@RefundCount);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(OrderDetailInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OrderDetail SET OrderId = @OrderId, ProductId = @ProductId, ProductName = @ProductName, StandardValueList = @StandardValueList, ProductWeight = @ProductWeight, SendPoint = @SendPoint, ProductPrice = @ProductPrice, BidPrice = @BidPrice, ActivityPoint = @ActivityPoint, BuyCount = @BuyCount, ParentId = @ParentId, RandNumber = @RandNumber, GiftPackId = @GiftPackId, RefundCount = @RefundCount
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OrderDetail where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OrderDetail where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void DeleteByOrderId(int[] orderIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OrderDetail where orderId in @orderIds";

                conn.Execute(sql, new { orderIds = orderIds });
            }
        }

        public OrderDetailInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderDetail where id=@id";

                var data = conn.Query<OrderDetailInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new OrderDetailInfo();
            }
        }

        public List<OrderDetailInfo> ReadList(int orderId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderDetail where orderId = @orderId";

                return conn.Query<OrderDetailInfo>(sql, new { orderId = orderId }).ToList();
            }
        }

        public int GetOrderCount(int productId, string standardValueList)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = string.Empty;
                if (!string.IsNullOrEmpty(standardValueList.Trim()))
                {
                    sql = "select ISNULL(Sum([BuyCount]),0) from [Order] a inner join  OrderDetail b on a.id=b.OrderID where OrderStatus!=3 and OrderStatus!=7 and productId = @productId and standardValueList=@standardValueList";
                }
                else
                {
                    sql = "select ISNULL(Sum([BuyCount]),0) from [Order] a inner join  OrderDetail b on a.id=b.OrderID where OrderStatus!=3 and OrderStatus!=7 and productId = @productId and (standardValueList=@standardValueList or standardValueList is null)";
                }
                return conn.Query<int>(sql, new {productId=productId,standardValueList=standardValueList}).Single();
            }
        }

        public List<OrderDetailInfo> ReadListByProductId(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OrderDetail where productId=@productId";

                return conn.Query<OrderDetailInfo>(sql, new { productId = productId }).ToList();
            }
        }

        public void ChangeBuyCount(int id, int buyCount)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OrderDetail SET buyCount = @buyCount where id=@id";

                conn.Execute(sql, new { buyCount = buyCount, id = id });
            }
        }

        public void ChangeRefundCount(int id, int refundCount, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = action == ChangeAction.Plus
                    ? @"UPDATE OrderDetail SET RefundCount = ISNULL(RefundCount,0) + @refundCount where id=@id"
                    : @"UPDATE OrderDetail SET RefundCount = RefundCount - @refundCount where id=@id";

                conn.Execute(sql, new { refundCount = refundCount, id = id });
            }
        }

        public DataTable StatisticsSaleDetail(int currentPage, int pageSize, OrderSearchInfo orderSearch, ProductSearchInfo productSearch, ref int count)
        {
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "View_SaleDetail";
            pc.Fields = "[Id],[Name],[ClassId],[BrandId],[BuyCount],[Money],[OrderNumber],[AddDate],[UserName]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[AddDate],[Id]";
            pc.OrderType = OrderType.Desc;
            pc.MssqlCondition = PrepareCondition(orderSearch, productSearch);
            
            count = pc.Count;
            return pc.ExecuteDataTable();
        }
        public MssqlCondition PrepareCondition(OrderSearchInfo orderSearch, ProductSearchInfo productSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[OrderNumber]", orderSearch.OrderNumber, ConditionType.Like);
            mssqlCondition.Add("[AddDate]", orderSearch.StartAddDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[AddDate]", orderSearch.EndAddDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[UserName]", orderSearch.UserName, ConditionType.Like);
            mssqlCondition.Add("[Name]", productSearch.Name, ConditionType.Like);
            mssqlCondition.Add("[BrandId]", productSearch.BrandId, ConditionType.Equal);
            mssqlCondition.Add("[ClassId]", productSearch.ClassId, ConditionType.Like);
            mssqlCondition.Add("[ProductId]", productSearch.InProductId, ConditionType.In);

            return mssqlCondition;
        }

    }
}