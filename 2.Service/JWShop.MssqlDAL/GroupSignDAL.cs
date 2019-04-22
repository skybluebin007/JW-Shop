using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using JWShop.IDAL;
using JWShop.Entity;
using System.Configuration;
using Dapper;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
   /// <summary>
   /// 参与拼团  数据层
   /// </summary>
  public sealed  class GroupSignDAL:IGroupSign
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(GroupSignInfo entity)
        {
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"insert into [GroupSign]([GroupId],[UserId],[OrderId],[SignTime]) values (@GroupId,@UserId,@OrderId,@SignTime);
                                select SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }
        }
        public  GroupSignInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [GroupSign] where id=@id";
                return conn.Query<GroupSignInfo>(sql, new { id = id }).SingleOrDefault() ?? new GroupSignInfo();
            }
        }
        /// <summary>
        /// 根据拼团活动id获取参团记录
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<GroupSignInfo> ReadListByGroupId(int groupId)
        {
            using(var conn=new SqlConnection(connectString))
            {
                string sql = @"select a.*,b.username,b.photo as UserAvatar from GroupSign a
                                inner join
                                [usr] b on a.[UserId] = b.[Id] and a.GroupId=@groupId order by a.[Id] asc";
                return conn.Query<GroupSignInfo>(sql, new { groupId = groupId }).ToList();
            }
        }
        public List<GroupSignInfo> SearchListByGroupId(int groupId, int pageIndex, int pageSize, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = @"( select a.*,b.Leader,b.StartTime,b.EndTime,b.Quantity,b.SignCount,c.OrderNumber as GroupOrderNumber,c.OrderStatus as GroupOrderStatus,c.IsRefund,b.ProductId,d.ProductName,d.ProductPrice,e.Photo as ProductPhoto,f.username,f.photo as UserAvatar from GroupSign a 
                                 inner join
                                 [GroupBuy] b on a.[GroupId]=b.[Id] 
                                 inner join
                                 [Order] c on a.[OrderId]=c.[Id] and c.[IsActivity]=2
                                inner join 
                                [orderdetail] d on d.[OrderId]=a.[OrderId]
                                inner join
                                [Product] e on e.[Id]=b.[ProductId]
                                inner join
                                [usr] f on a.[UserId]=f.[Id]) tmp";
                pc.Fields = "*";
                pc.CurrentPage = pageIndex;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("  [GroupId]=" + groupId);

                count = pc.Count;
                return conn.Query<GroupSignInfo>(pc).ToList();
            }
        }
        /// <summary>
        /// 根据userId获取参团记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GroupSignInfo> ReadListByUserId(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [GroupSign] where UserId=@userId";
                return conn.Query<GroupSignInfo>(sql,new { userId = userId }).ToList();
            }
        }
       
        public List<GroupSignInfo> SearchListByUserId(int userId,int pageIndex,int pageSize,ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = @"( select a.*,b.Leader,b.StartTime,b.EndTime,b.Quantity,b.SignCount,c.OrderNumber as GroupOrderNumber,c.OrderStatus as GroupOrderStatus,c.IsRefund,b.ProductId,d.ProductName,d.ProductPrice,e.Photo as ProductPhoto from GroupSign a 
                                 inner join
                                 [GroupBuy] b on a.[GroupId]=b.[Id] 
                                 inner join
                                 [Order] c on a.[OrderId]=c.[Id] and c.[IsActivity]=2
                                inner join 
                                [orderdetail] d on d.[OrderId]=a.[OrderId]
                                inner join
                                [Product] e on e.[Id]=b.[ProductId]) tmp";
                pc.Fields = "*";
                pc.CurrentPage = pageIndex;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("  [UserId]="+userId);

                count = pc.Count;
                return conn.Query<GroupSignInfo>(pc).ToList();
            }
        }
    }
}
