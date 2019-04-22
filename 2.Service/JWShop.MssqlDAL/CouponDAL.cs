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
    public sealed class CouponDAL : ICoupon
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(CouponInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                long id = conn.Insert<CouponInfo>(entity);
                return Convert.ToInt32(id);
            }
        }

        public void Update(CouponInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                //conn.Update<CouponInfo>(entity);
                string sql = @"update [Coupon] set [Name]=@Name,[Money]=@Money,[UseMinAmount]=@UseMinAmount,[UseStartDate]=@UseStartDate,[UseEndDate]=@UseEndDate,[Type]=@Type,[Photo]=@Photo,[TotalCount]=@TotalCount where Id=@Id";
                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Coupon where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public CouponInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                return conn.Get<CouponInfo>(id) ?? new CouponInfo();
            }
        }

        public List<CouponInfo> SearchList(CouponSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Coupon where 1=1 ";

                var para = new DynamicParameters();
                if (searchInfo.InCouponIds.Length > 0)
                {
                    sql += " and Id in @ids";
                    para.Add("ids", searchInfo.InCouponIds);
                }
                if (!string.IsNullOrEmpty(searchInfo.Name))
                {
                    sql += " and Name like @name";
                    para.Add("name", "%" + searchInfo.Name + "%");
                }

                return conn.Query<CouponInfo>(sql, para).ToList();
            }
        }
        /// <summary>
        /// 获取所有在有效期内的优惠券
        /// </summary>
        /// <returns></returns>
        public List<CouponInfo> SearchListCanUse()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Coupon where [UseStartDate]<=getdate() and [UseEndDate]>=getdate() ";
                return conn.Query<CouponInfo>(sql).ToList();
            }
        }
        public List<CouponInfo> SearchList(int currentPage, int pageSize, CouponSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "Coupon";
                pc.Fields = " * ";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.MssqlCondition = PrepareCondition(searchInfo);
                
                count = pc.Count;
                return conn.Query<CouponInfo>(pc).ToList();
            }
        }

        public MssqlCondition PrepareCondition(CouponSearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Name]", searchInfo.Name, ConditionType.Like);
            mssqlCondition.Add("[Id]", string.Join(",", searchInfo.InCouponIds), ConditionType.In);
            if(searchInfo.Type>0) mssqlCondition.Add("[Type]", searchInfo.Type, ConditionType.Equal);
            if (searchInfo.CanUse == 1) mssqlCondition.Add("      DATEDIFF(day,getdate(),[UseStartDate])<=0  and  DATEDIFF(day,getdate(),[UseEndDate])>=0");
            //未开始
            if (searchInfo.TimePeriod == 1) mssqlCondition.Add(" DATEDIFF ( day  , getdate(), [UseStartDate] )>0");
            //进行中
            if (searchInfo.TimePeriod == 2) mssqlCondition.Add("  DATEDIFF(day,getdate(),[UseStartDate])<=0  and  DATEDIFF(day,getdate(),[UseEndDate])>=0");
            //已结束
            if (searchInfo.TimePeriod == 3) mssqlCondition.Add("  DATEDIFF(day,getdate(),[UseEndDate])<0");
            return mssqlCondition;
        }

    }
}