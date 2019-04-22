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
    public sealed class OrderDAL : IOrder
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OrderInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [Order]( ShopId,OrderNumber,IsActivity,OrderStatus,OrderNote,ProductMoney,Balance,FavorableMoney,OtherMoney,CouponMoney,Point,PointMoney,Consignee,RegionId,Address,ZipCode,Tel,Email,Mobile,ShippingId,ShippingDate,ShippingNumber,ShippingMoney,PayKey,PayName,PayDate,IsRefund,FavorableActivityId,GiftId,InvoiceTitle,InvoiceContent,UserMessage,AddDate,IP,UserId,UserName,IsDelete,ActivityPoint,AliPayTradeNo,WxPayTradeNo,IsNoticed,SelfPick,BargainOrderId,Saleid,Shuigongid,Shiyaid,Shuigong_name,Shuigong_tel,Shiya_name,Shiya_tel,Shuigong_stat,Shiya_stat) VALUES(@ShopId,@OrderNumber,@IsActivity,@OrderStatus,@OrderNote,@ProductMoney,@Balance,@FavorableMoney,@OtherMoney,@CouponMoney,@Point,@PointMoney,@Consignee,@RegionId,@Address,@ZipCode,@Tel,@Email,@Mobile,@ShippingId,@ShippingDate,@ShippingNumber,@ShippingMoney,@PayKey,@PayName,@PayDate,@IsRefund,@FavorableActivityId,@GiftId,@InvoiceTitle,@InvoiceContent,@UserMessage,@AddDate,@IP,@UserId,@UserName,@IsDelete,@ActivityPoint,@AliPayTradeNo,@WxPayTradeNo,@IsNoticed,@SelfPick,@BargainOrderId,@Saleid,@Shuigongid,@Shiyaid,@Shuigong_name,@Shuigong_tel,@Shiya_name,@Shiya_tel,@Shuigong_stat,@Shiya_stat);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(OrderInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [Order] SET ShopId = @ShopId, OrderNumber = @OrderNumber, IsActivity = @IsActivity, OrderStatus = @OrderStatus, OrderNote = @OrderNote, ProductMoney = @ProductMoney, Balance = @Balance, FavorableMoney = @FavorableMoney, OtherMoney = @OtherMoney, CouponMoney = @CouponMoney, Point = @Point, PointMoney = @PointMoney, Consignee = @Consignee, RegionId = @RegionId, Address = @Address, ZipCode = @ZipCode, Tel = @Tel, Email = @Email, Mobile = @Mobile, ShippingId = @ShippingId, ShippingDate = @ShippingDate, ShippingNumber = @ShippingNumber, ShippingMoney = @ShippingMoney, PayKey = @PayKey, PayName = @PayName, PayDate = @PayDate, IsRefund = @IsRefund, FavorableActivityId = @FavorableActivityId, GiftId = @GiftId, InvoiceTitle = @InvoiceTitle, InvoiceContent = @InvoiceContent, UserMessage = @UserMessage, AddDate = @AddDate, IP = @IP, UserId = @UserId, UserName = @UserName, IsDelete=@IsDelete, ActivityPoint=@ActivityPoint, AliPayTradeNo = @AliPayTradeNo, WxPayTradeNo = @WxPayTradeNo,IsNoticed=@IsNoticed,BargainOrderId=@BargainOrderId,Saleid=@Saleid,Shuigongid=@Shuigongid,Shiyaid=@Shiyaid,Shuigong_name=@Shuigong_name,Shuigong_tel=@Shuigong_tel,Shiya_name=@Shiya_name,Shiya_tel=@Shiya_tel,Shuigong_stat=@Shuigong_stat,Shiya_stat=@Shiya_stat
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void UpdateIsNoticed(int orderid,int isNoticed)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "";
                if (orderid > 0)
                    sql = @"UPDATE [Order] SET IsNoticed=@isNoticed where Id=@orderid";
                else
                    sql = @"UPDATE [Order] SET IsNoticed=@isNoticed";

                conn.Execute(sql, new { isNoticed= isNoticed, orderid = orderid });
            }
        }

        public void UpdateIsNoticed(int[] orderids, int isNoticed)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "";
                if (orderids.Count() > 0)
                {
                    //string orders = string.Join(",", orderids);
                    sql = @"UPDATE [Order] SET IsNoticed=@isNoticed where Id in @orderids";

                    conn.Execute(sql, new { isNoticed = isNoticed, orderids = orderids });
                }                    

                
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [Order] where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int[] ids, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [Order] where id in @ids and userId=@userId";

                conn.Execute(sql, new { ids = ids, userId = userId });
            }
        }

        public OrderInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where id=@id";

                var data = conn.Query<OrderInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new OrderInfo();
            }
        }

        public OrderInfo Read(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where id=@id and userId=@userId";
                if (userId <= 0) sql = "select * from [Order] where id=@id";

                var data = conn.Query<OrderInfo>(sql, new { id = id, userId = userId }).SingleOrDefault();
                return data ?? new OrderInfo();
            }
        }

        public OrderInfo ReadByShopId(int id, int shopId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where id=@id and shopId=@shopId";

                var data = conn.Query<OrderInfo>(sql, new { id = id, shopId = shopId }).SingleOrDefault();
                return data ?? new OrderInfo();
            }
        }

        public OrderInfo Read(string orderNumber, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where orderNumber=@orderNumber and userId=@userId";

                var data = conn.Query<OrderInfo>(sql, new { orderNumber = orderNumber, userId = userId }).SingleOrDefault();
                return data ?? new OrderInfo();
            }
        }
        public OrderInfo Read(string orderNumber)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where orderNumber=@orderNumber";

                var data = conn.Query<OrderInfo>(sql, new { orderNumber = orderNumber}).SingleOrDefault();
                return data ?? new OrderInfo();
            }
        }

        public List<OrderInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order]";

                return conn.Query<OrderInfo>(sql).ToList();
            }
        }

        public List<OrderInfo> ReadList(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where id in @ids";

                return conn.Query<OrderInfo>(sql, new { ids = ids }).ToList();
            }
        }

        public List<OrderInfo> ReadList(int[] ids, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order] where id in @ids and userId=@userId";

                return conn.Query<OrderInfo>(sql, new { ids = ids, userId = userId }).ToList();
            }
        }

        public int ReadCount(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from [Order] where userId=@userId";

                return conn.ExecuteScalar<int>(sql, new { userId = userId });
            }
        }
        /// <summary>
        /// 待付款订单失效检查
        /// </summary>
        /// <param name="ids"></param>
        public void CheckOrderPayTime(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [Order] SET OrderStatus =3 where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }
        /// <summary>
        /// 待付款订单失效检查--所有订单--存储过程
        /// </summary>       
        public void CheckOrderPayTimeProg()
        {          
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
                new SqlParameter("@payTime",SqlDbType.Int),
                new SqlParameter("@clientIP",SqlDbType.VarChar)
			};
            parameters[0].Value = "CheckOrderPayTime";
            parameters[1].Value = ShopConfig.ReadConfigInfo().OrderPayTime;
            parameters[2].Value = ClientHelper.IP;
            ShopMssqlHelper.ExecuteNonQuery("usp_Order", parameters);
        }

        /// <summary>
        /// 待付款订单失效检查--指定用户订单--存储过程
        /// </summary>       
        public void CheckOrderPayTimeProg(int userId)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
                new SqlParameter("@payTime",SqlDbType.Int),
                new SqlParameter("@theUserID",SqlDbType.Int),
                new SqlParameter("@clientIP",SqlDbType.VarChar)
            };
            parameters[0].Value = "CheckOrderPayTime";
            parameters[1].Value = ShopConfig.ReadConfigInfo().OrderPayTime;
            parameters[2].Value = userId;
            parameters[3].Value = ClientHelper.IP;
            ShopMssqlHelper.ExecuteNonQuery("usp_Order", parameters);
        }

        /// <summary>
        /// 待收货订单自动收货--所有订单--存储过程
        /// </summary>       
        public void CheckOrderRecieveTimeProg()
        {
            SqlParameter[] parameters ={
                new SqlParameter("@type",SqlDbType.VarChar),
                new SqlParameter("@recieveDays",SqlDbType.Int),
                new SqlParameter("@clientIP",SqlDbType.VarChar)
            };
            parameters[0].Value = "CheckOrderRecieveTime";
            parameters[1].Value = ShopConfig.ReadConfigInfo().OrderRecieveShippingDays;
            parameters[2].Value = ClientHelper.IP;
            ShopMssqlHelper.ExecuteNonQuery("usp_Order", parameters);
        }
        /// <summary>
        /// 待收货订单自动收货--指定订单--存储过程
        /// </summary>       
        public void CheckOrderRecieveTimeProg(int userId)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@type",SqlDbType.VarChar),
                new SqlParameter("@recieveDays",SqlDbType.Int),
                new SqlParameter("@theUserID",SqlDbType.Int),
                new SqlParameter("@clientIP",SqlDbType.VarChar)
            };
            parameters[0].Value = "CheckOrderRecieveTime";
            parameters[1].Value = ShopConfig.ReadConfigInfo().OrderRecieveShippingDays;
            parameters[2].Value = userId;
            parameters[3].Value = ClientHelper.IP;
            ShopMssqlHelper.ExecuteNonQuery("usp_Order", parameters);
        }
        #region search
        public List<OrderInfo> SearchList(OrderSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Order]";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<OrderInfo>(sql).ToList();
            }
        }
        public List<OrderInfo> SearchList(int currentPage, int pageSize, OrderSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "[Order]";
                pc.Fields = "[Id], [OrderNumber], [IsActivity], [OrderStatus], [OrderNote], [ProductMoney], [Balance], [FavorableMoney], [OtherMoney], [CouponMoney], [Point], [PointMoney], [Consignee], [RegionId], [Address], [ZipCode], [Tel], [Email], [Mobile], [ShippingId], [ShippingDate], [ShippingNumber], [ShippingMoney], [PayKey], [PayName], [PayDate], [IsRefund], [FavorableActivityId], [GiftId], [InvoiceTitle], [InvoiceContent], [UserMessage], [AddDate], [IP], [UserId], [UserName], [IsDelete], [ActivityPoint], [AliPayTradeNo], [WxPayTradeNo],[SelfPick],[Saleid],[Shuigongid],[Shiyaid],[Shuigong_name],[Shuigong_tel],[Shiya_name],[Shiya_tel],[Shuigong_stat],[Shiya_stat]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<OrderInfo>(pc).ToList();
            }
        }
        /// <summary>
        /// 搜索拼团订单
        /// </summary>     
        public List<OrderInfo> SearchGroupOrderList(int currentPage, int pageSize, OrderSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = @"(select  a.[Id], [OrderNumber], [IsActivity], [OrderStatus], [OrderNote], [ProductMoney], [Balance], [FavorableMoney], [OtherMoney], [CouponMoney], [Point], [PointMoney], [Consignee], [RegionId], [Address], [ZipCode], [Tel], [Email], [Mobile], [ShippingId], [ShippingDate], [ShippingNumber], [ShippingMoney], [PayKey], [PayName], [PayDate], [IsRefund], [FavorableActivityId], [GiftId], [InvoiceTitle], [InvoiceContent], [UserMessage], [AddDate], [IP], [UserId], [UserName], [IsDelete], [ActivityPoint], [AliPayTradeNo], [WxPayTradeNo],[SelfPick],b.[Leader],b.[ProductId],b.[StartTime],b.[EndTime],b.[Quantity],b.[SignCount]
                  from[Order] a
                    inner join
                    [GroupBuy] b
                    on a.[IsActivity] = 2 and a.[FavorableActivityId]=b.[Id]) tmp";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<OrderInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(OrderSearchInfo orderSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[ShopId]", orderSearch.ShopId, ConditionType.Equal);
            mssqlCondition.Add("[OrderNumber]", orderSearch.OrderNumber, ConditionType.Like);
            mssqlCondition.Add("[OrderStatus]", orderSearch.OrderStatus, ConditionType.Equal);
            mssqlCondition.Add("[Consignee]", orderSearch.Consignee, ConditionType.Like);
            mssqlCondition.Add("[RegionId]", orderSearch.RegionId, ConditionType.Like);
            mssqlCondition.Add("[ShippingId]", orderSearch.ShippingId, ConditionType.Equal);
            mssqlCondition.Add("[AddDate]", orderSearch.StartAddDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[AddDate]", orderSearch.EndAddDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[PayDate]", orderSearch.StartPayDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[PayDate]", orderSearch.EndPayDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[UserId]", orderSearch.UserId, ConditionType.Equal);
            mssqlCondition.Add("[UserName]", orderSearch.UserName, ConditionType.Like);
            mssqlCondition.Add("[IsDelete]", orderSearch.IsDelete, ConditionType.Equal);
            mssqlCondition.Add("[IsNoticed]", orderSearch.IsNoticed, ConditionType.Equal);
            mssqlCondition.Add("[Mobile]", orderSearch.Mobile, ConditionType.Like);
            mssqlCondition.Add("[IsActivity]", orderSearch.IsActivity, ConditionType.Equal);
            mssqlCondition.Add("[FavorableActivityId]", orderSearch.FavorableActivityId, ConditionType.Equal);
            mssqlCondition.Add("[OrderStatus]", orderSearch.NotEqualStatus, ConditionType.NoEqual);
            mssqlCondition.Add("[SelfPick]", orderSearch.SelfPick, ConditionType.Equal);

            mssqlCondition.Add("[Saleid]", orderSearch.Saleid, ConditionType.Equal);
            mssqlCondition.Add("[Shuigongid]", orderSearch.Shuigongid, ConditionType.Equal);
            mssqlCondition.Add("[Shiyaid]", orderSearch.Shiyaid, ConditionType.Equal);
            mssqlCondition.Add("[Shuigong_name]", orderSearch.Shuigong_name, ConditionType.Like);
            mssqlCondition.Add("[Shuigong_tel]", orderSearch.Shuigong_tel, ConditionType.Like);
            mssqlCondition.Add("[Shiya_name]", orderSearch.Shiya_name, ConditionType.Like);
            mssqlCondition.Add("[Shiya_tel]", orderSearch.Shiya_tel, ConditionType.Like);
            mssqlCondition.Add("[Shuigong_stat]", orderSearch.Shuigong_stat, ConditionType.Equal);
            mssqlCondition.Add("[Shiya_stat]", orderSearch.Shiya_stat, ConditionType.Equal);

            //进行中
            if (orderSearch.OrderPeriod == (int)OrderPeriod.Going)
            {
                mssqlCondition.Add("  [OrderStatus] in (1,2,4,5)");
            }
            //已完成
            if (orderSearch.OrderPeriod == (int)OrderPeriod.Finished)
            {
                mssqlCondition.Add("  [OrderStatus]=6");
            }
            //已取消
            if (orderSearch.OrderPeriod == (int)OrderPeriod.Closed)
            {
                mssqlCondition.Add("  [OrderStatus]=3");
            }
            //订单号或者手机号
            if (!string.IsNullOrEmpty(orderSearch.SearchKey))
            {
                mssqlCondition.Add("  ([OrderNumber] like '%" + orderSearch.SearchKey + "%' or [Mobile] like '%" + orderSearch.SearchKey + "%')");
            }
            //订单日期
            if (orderSearch.OrderDate > DateTime.MinValue)
            {
                mssqlCondition.Add("[AddDate]", orderSearch.OrderDate, ConditionType.Equal);
            }

            #region groupbuy拼团订单联合搜索
            if (orderSearch.ProductId > 0) mssqlCondition.Add("[ProductId]", orderSearch.ProductId, ConditionType.Equal);
            if (orderSearch.Leader > 0) mssqlCondition.Add("[Leader]", orderSearch.Leader, ConditionType.Equal);
            if (orderSearch.NotLeader > 0) mssqlCondition.Add("[Leader]", orderSearch.NotLeader, ConditionType.NoEqual);
            mssqlCondition.Add("[StartTime]", orderSearch.StartTime, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[EndTime]", orderSearch.EndTime, ConditionType.LessOrEqual);

            //拼团失败(逾期未拼满)
            if (orderSearch.Status == (int)GroupBuyStatus.Fail)
            {
                mssqlCondition.Add("  [EndTime]<getdate() and [SignCount]<[Quantity]");
            }
            //拼团成功(有效期内已拼满)
            if (orderSearch.Status == (int)GroupBuyStatus.Success)
            {
                mssqlCondition.Add("   [StartTime]<=getdate() and [EndTime]>=getdate() and [SignCount]>=[Quantity]");
            }
            //拼团进行中（在有效期内未拼满）
            if (orderSearch.Status == (int)GroupBuyStatus.Going)
            {
                mssqlCondition.Add("  [StartTime]<=getdate() and [EndTime]>=getdate() and [SignCount]<[Quantity]");
            }
            #endregion
            return mssqlCondition;
        }
        #endregion

        #region 统计
        public DataTable StatisticsOrderStatus(OrderSearchInfo orderSearch)
        {
            string condition = PrepareCondition(orderSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = "StatisticsOrderStatus";
            parameters[1].Value = condition;
            return ShopMssqlHelper.ExecuteDataTable("usp_Order", parameters);
        }
        public DataTable StatisticsOrderCount(OrderSearchInfo orderSearch, DateType dateType)
        {
            string condition = PrepareCondition(orderSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@condition",SqlDbType.NVarChar),
                new SqlParameter("@dateType",SqlDbType.Int)
			};
            parameters[0].Value = "StatisticsOrderCount";
            parameters[1].Value = condition;
            parameters[2].Value = (int)dateType;
            return ShopMssqlHelper.ExecuteDataTable("usp_Order", parameters);
        }
        public DataTable StatisticsOrderArea(OrderSearchInfo orderSearch)
        {
            string condition = PrepareCondition(orderSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = "StatisticsOrderArea";
            parameters[1].Value = condition;
            return ShopMssqlHelper.ExecuteDataTable("usp_Order", parameters);
        }
        public DataTable StatisticsSaleTotal(OrderSearchInfo orderSearch, DateType dateType)
        {
            string condition = PrepareCondition(orderSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@condition",SqlDbType.NVarChar),
                new SqlParameter("@dateType",SqlDbType.Int)
			};
            parameters[0].Value = "StatisticsSaleTotal";
            parameters[1].Value = condition;
            parameters[2].Value = (int)dateType;
            return ShopMssqlHelper.ExecuteDataTable("usp_Order", parameters);
        }
        /// <summary>
        /// 统计销售汇总
        /// </summary>
        /// <param name="orderSearch"></param>
        /// <returns></returns>
        public DataTable StatisticsAllTotal(OrderSearchInfo orderSearch)
        {
            string condition = PrepareCondition(orderSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = condition;
            return ShopMssqlHelper.ExecuteDataTable("StatisticsAllTotal", parameters);
        }
        /// <summary>
        /// 统计已付款汇总
        /// </summary>
        /// <param name="orderSearch"></param>
        /// <returns></returns>
        public DataTable StatisticsPaidTotal(OrderSearchInfo orderSearch)
        {
            string condition = PrepareCondition(orderSearch).ToString();
            SqlParameter[] parameters ={
                new SqlParameter("@condition",SqlDbType.NVarChar)
            };
            parameters[0].Value = condition;
            return ShopMssqlHelper.ExecuteDataTable("StatisticsPaidTotal", parameters);
        }
        public DataTable StatisticsSaleStop(string productIds)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@productIds",SqlDbType.NVarChar)
			};
            parameters[0].Value = "StatisticsSaleStop";
            parameters[1].Value = productIds;
            return ShopMssqlHelper.ExecuteDataTable("usp_Order", parameters);
        }
        #endregion

        public List<OrderInfo> ReadPreNextOrder(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT [Id] FROM
                            (
	                            (SELECT TOP 1 [Id] FROM [Order] WHERE [Id]>@id ORDER BY Id ASC)
	                             UNION ALL 
	                            (SELECT TOP 1 [Id] FROM [Order] WHERE [Id]<@id ORDER BY Id DESC)
                            ) AS TEMP ";

                return conn.Query<OrderInfo>(sql, new { id = id }).ToList();
            }
        }
        /// <summary>
        /// 启用不限库存后，获取商品单日销量
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public int GetProductOrderCountDaily(int productId,int productStandType, DateTime date, string standardValueList=null)
        {
            int result = 0;
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"select a.Id,a.ordernumber,a.adddate,a.orderStatus,b.ProductId,b.buycount from [Order] a join [OrderDetail] b on  a.id = b.orderid  where a.orderStatus <> 3 and a.orderstatus <> 7 and a.isrefund = 0";
                if (productStandType != (int)ProductStandardType.Single)
                {
                    #region 没有规格或者产品组规格 
                    sql += @"  and b.ProductId=@ProductId and datediff(day,@Date,a.adddate)=0";                
                   
                    #endregion
                }
                else
                {
                    #region 单产品规格
                    if(standardValueList!=null && !string.IsNullOrEmpty(standardValueList.Trim()))
                        sql += @"  and b.ProductId=@ProductId and datediff(day,@Date,a.adddate)=0 and [StandardValueList]=@StandardValueList";
                    else
                        sql += @"  and b.ProductId=@ProductId and datediff(day,@Date,a.adddate)=0";
                    #endregion
                }
                string executeSql = string.Format("select ISNULL(Sum([BuyCount]),0) from ({0}) tmp ", sql);
                result = conn.ExecuteScalar<int>(executeSql, new { ProductId = productId, Date = date, StandardValueList=standardValueList });               
                return result;
            }
        }
    }
}