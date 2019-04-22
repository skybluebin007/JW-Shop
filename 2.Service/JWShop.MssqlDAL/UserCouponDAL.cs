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
using Dapper.Contrib.Extensions;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class UserCouponDAL : IUserCoupon
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserCouponInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO UsrCoupon( CouponId,GetType,Number,Password,IsUse,OrderId,UserId,UserName) VALUES(@CouponId,@GetType,@Number,@Password,@IsUse,@OrderId,@UserId,@UserName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(UserCouponInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE UsrCoupon SET CouponId = @CouponId, GetType = @GetType, Number = @Number, Password = @Password, IsUse = @IsUse, OrderId = @OrderId, UserId = @UserId, UserName = @UserName
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from UsrCoupon where id in @ids";

                var para = new DynamicParameters();
                if (userId > 0)
                {
                    sql += " and UserId = @userId";
                    para.Add("userId", userId);
                }
                para.Add("ids", ids);

                conn.Execute(sql, para);
            }
        }

        public void Delete(int[] couponIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from UsrCoupon where CouponId in @couponIds";

                conn.Execute(sql, new { couponIds = couponIds });
            }
        }

        public UserCouponInfo Read(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrCoupon where id=@id";

                var para = new DynamicParameters();
                if (userId > 0)
                {
                    sql += " and UserId = @userId";
                    para.Add("userId", userId);
                }
                para.Add("id", id);

                var data = conn.Query<UserCouponInfo>(sql, para).SingleOrDefault();
                return data ?? new UserCouponInfo();
            }
        }

        public UserCouponInfo ReadLast(int couponId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT TOP 1 * FROM UsrCoupon WHERE [CouponId]=@couponId ORDER BY Id DESC";

                var data = conn.Query<UserCouponInfo>(sql, new { couponId = couponId }).SingleOrDefault();
                return data ?? new UserCouponInfo();
            }
        }

        public UserCouponInfo Read(string number, string password)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrCoupon where Number = @number and Password=@password";

                var data = conn.Query<UserCouponInfo>(sql, new { number = number, password = password }).SingleOrDefault();
                return data ?? new UserCouponInfo();
            }
        }

        public UserCouponInfo Read(int orderId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"	SELECT UsrCoupon.[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName],[Money],[UseMinAmount] 
		                        FROM UsrCoupon INNER JOIN  Coupon 
		                        ON UsrCoupon.[CouponId]=Coupon.[Id] 
		                        WHERE [OrderId]=@orderId";

                var data = conn.Query<UserCouponInfo>(sql, new { orderId = orderId }).SingleOrDefault();
                return data ?? new UserCouponInfo();
            }
        }

        public List<UserCouponInfo> ReadCanUse(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"	SELECT UsrCoupon.[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName],[Money],[UseMinAmount]
		                        FROM UsrCoupon INNER JOIN Coupon 
		                        ON UsrCoupon.[CouponId]=Coupon.[Id] 
		                        WHERE [IsUse]=0 AND [UserId]=@userId AND [UseStartDate]<=getdate() AND [UseEndDate]>=getdate()";

                return conn.Query<UserCouponInfo>(sql, new { userId = userId }).ToList();
            }
        }
        /// <summary>
        /// 检查会员是否已领取过优惠券(包含用户自己领取3，新人领券4，或者生日礼券自动赠送6)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UniqueUserCatch(int userId = 0, int couponId = 0)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from UsrCoupon where UserId = @userId and CouponId=@couponId and (GetType=3 or GetType=4 or GetType=6)";

                return conn.ExecuteScalar<int>(sql, new { userId = userId, couponId = couponId }) < 1;
            }
        }
        public List<UserCouponInfo> SearchList(UserCouponSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrCoupon";
                if (searchInfo.IsTimeOut == (int)BoolType.True)//已过期
                {
                    sql = @"select * from	(SELECT UsrCoupon.[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName],[Money],[UseMinAmount]
		                        FROM UsrCoupon INNER JOIN Coupon 
		                        ON UsrCoupon.[CouponId]=Coupon.[Id] 
		                        AND [UseEndDate]<getdate()) temp";
                }
                if (searchInfo.IsTimeOut == (int)BoolType.False)//未过期
                {
                    sql = @"select * from	(SELECT UsrCoupon.[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName],[Money],[UseMinAmount]
		                        FROM UsrCoupon INNER JOIN Coupon 
		                        ON UsrCoupon.[CouponId]=Coupon.[Id] 
		                        AND [UseStartDate]<=getdate() AND [UseEndDate]>=getdate()) temp";
                }
              
                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<UserCouponInfo>(sql).ToList();
            }
        }
        //public List<UserCouponInfo> SearchList(int currentPage, int pageSize, UserCouponSearchInfo searchInfo, ref int count)
        //{
        //    using (var conn = new SqlConnection(connectString))
        //    {
        //        ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
        //        pc.TableName = "UsrCoupon";
        //        pc.Fields = "[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName]";
        //        pc.CurrentPage = currentPage;
        //        pc.PageSize = pageSize;
        //        pc.OrderField = "[Id]";
        //        pc.MssqlCondition = PrepareCondition(searchInfo);

        //        count = pc.Count;
        //        return conn.Query<UserCouponInfo>(pc).ToList();
        //    }
        //}
        public List<UserCouponInfo> SearchList(int currentPage, int pageSize, UserCouponSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                if (searchInfo.IsTimeOut ==(int)BoolType.True)//已过期
                {
                    pc.TableName = @"	(SELECT UsrCoupon.[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName],[Money],[UseMinAmount]
		                        FROM UsrCoupon INNER JOIN Coupon 
		                        ON UsrCoupon.[CouponId]=Coupon.[Id] 
		                        AND [UseEndDate]<getdate()) temp";
                }
              if (searchInfo.IsTimeOut == (int)BoolType.False)//未过期
                {
                    pc.TableName = @"	(SELECT UsrCoupon.[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName],[Money],[UseMinAmount]
		                        FROM UsrCoupon INNER JOIN Coupon 
		                        ON UsrCoupon.[CouponId]=Coupon.[Id] 
		                        AND [UseStartDate]<=getdate() AND [UseEndDate]>=getdate()) temp";
                }
              if (searchInfo.IsTimeOut == -1)//所有
              {
                  pc.TableName = "UsrCoupon";
              } 
                pc.Fields = "[Id],[CouponId],[GetType],[Number],[Password],[IsUse],[OrderId],[UserId],[UserName]";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<UserCouponInfo>(pc).ToList();
            }
        }

        public MssqlCondition PrepareCondition(UserCouponSearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[CouponId]", searchInfo.CouponId, ConditionType.Equal);
            mssqlCondition.Add("[GetType]", searchInfo.GetType, ConditionType.Equal);
            mssqlCondition.Add("[Number]", searchInfo.Number, ConditionType.Like);
            mssqlCondition.Add("[IsUse]", searchInfo.IsUse, ConditionType.Equal);
            mssqlCondition.Add("[UserId]", searchInfo.UserId, ConditionType.Equal);
          
            return mssqlCondition;
        }

    }
}