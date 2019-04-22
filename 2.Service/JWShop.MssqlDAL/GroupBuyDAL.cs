using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using SkyCES.EntLib;
using JWShop.IDAL;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 拼团 数据层
    /// </summary>
    public sealed class GroupBuyDAL : IGroupBuy
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(GroupBuyInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"insert into [GroupBuy]([Leader],[ProductId],[StartTime],[EndTime],[Quantity],[SignCount]) values (@Leader,@ProductId,@StartTime,@EndTime,@Quantity,@SignCount);
                                select SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }
        }
        public GroupBuyInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"select a.*,b.username as GroupUserName,b.photo as GroupUserAvatar,d.ProductName,d.ProductPrice,e.Photo as ProductPhoto,c.Id as GroupOrderId,c.OrderStatus as GroupOrderStatus,c.IsRefund 
                                from [GroupBuy] a 
                                inner join
                                [usr] b on a.[Leader] = b.[Id]
                                inner join 
                                [Order] c on a.[Id]=c.[FavorableActivityId]  and a.[Leader]=c.[UserId]  and c.[IsActivity]=2
                                inner join 
                                [orderdetail] d on d.[OrderId]=c.[Id]
                                inner join
                                [Product] e on e.[Id]=a.[ProductId] and a.id=@id";
                return conn.Query<GroupBuyInfo>(sql, new { id = id }).SingleOrDefault() ?? new GroupBuyInfo();
            }
        }
        /// <summary>
        /// 增加参团人数（有人参团,+1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int PlusSignCount(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update [GroupBuy] set [SignCount]=[SignCount]+1 where id=@id";
                return conn.Execute(sql, new { id = id });
            }
        }
        /// <summary>
        /// 减少参团人数（-1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int MinusSignCount(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update [GroupBuy] set [SignCount]=[SignCount]-1 where id=@id";
                return conn.Execute(sql, new { id = id });
            }
        }
        #region search
        public List<GroupBuyInfo> ReadList(int[] ids)
        {
            using(var conn=new SqlConnection(connectString))
            {
                string sql = "select * from [GroupBuy] where id in @ids";
                return conn.Query<GroupBuyInfo>(sql, new { ids = ids }).ToList();
            }
        }
        public List<GroupBuyInfo> SearchList(GroupBuySearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"select * from (select a.*,b.username as GroupUserName,b.photo as GroupUserAvatar,d.ProductName,d.ProductPrice,e.Photo as ProductPhoto,c.Id as GroupOrderId,c.OrderStatus as GroupOrderStatus,c.IsRefund 
                                from [GroupBuy] a 
                                inner join
                                [usr] b on a.[Leader] = b.[Id]
                                inner join 
                                [Order] c on a.[Id]=c.[FavorableActivityId]  and a.[Leader]=c.[UserId]  and c.[IsActivity]=2
                                inner join 
                                [orderdetail] d on d.[OrderId]=c.[Id]
                                inner join
                                [Product] e on e.[Id]=a.[ProductId]) tmp";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += "  where " + condition;
                }
                sql += "  Order By Id desc";
                return conn.Query<GroupBuyInfo>(sql).ToList();
            }
        }
        public List<GroupBuyInfo> SearchList(int currentPage, int pageSize, GroupBuySearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = @"(select a.*,b.username as GroupUserName,b.photo as GroupUserAvatar,d.ProductName,d.ProductPrice,e.Photo as ProductPhoto,c.Id as GroupOrderId,c.OrderStatus as GroupOrderStatus,c.IsRefund 
                                from [GroupBuy] a 
                                inner join
                                [usr] b on a.[Leader] = b.[Id]
                                inner join 
                                [Order] c on a.[Id]=c.[FavorableActivityId]  and a.[Leader]=c.[UserId]  and c.[IsActivity]=2
                                inner join 
                                [orderdetail] d on d.[OrderId]=c.[Id]
                                inner join
                                [Product] e on e.[Id]=a.[ProductId]) tmp";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<GroupBuyInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(GroupBuySearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            if (searchInfo.ProductId > 0) mssqlCondition.Add("[ProductId]", searchInfo.ProductId, ConditionType.Equal);
            if (searchInfo.Leader > 0) mssqlCondition.Add("[Leader]", searchInfo.Leader, ConditionType.Equal);
            if (searchInfo.NotLeader > 0) mssqlCondition.Add("[Leader]", searchInfo.NotLeader, ConditionType.NoEqual);
            mssqlCondition.Add("[StartTime]", searchInfo.StartTime, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[EndTime]", searchInfo.EndTime, ConditionType.LessOrEqual);

            //拼团失败(逾期未拼满)
            if (searchInfo.Status == (int)GroupBuyStatus.Fail)
            {
                mssqlCondition.Add("  [EndTime]<getdate() and [SignCount]<[Quantity]");
            }
            //拼团成功(有效期内已拼满)
            if (searchInfo.Status == (int)GroupBuyStatus.Success)
            {
                mssqlCondition.Add("   [StartTime]<=getdate() and [EndTime]>=getdate() and [SignCount]>=[Quantity]");
            }
            //拼团进行中（在有效期内未拼满）
            if (searchInfo.Status == (int)GroupBuyStatus.Going)
            {
                mssqlCondition.Add("  [StartTime]<=getdate() and [EndTime]>=getdate() and [SignCount]<[Quantity]");
            }
            return mssqlCondition;
        }
        #endregion
    }
}
