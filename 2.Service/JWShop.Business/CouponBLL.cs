using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    public sealed class CouponBLL : BaseBLL
    {
        private static readonly ICoupon dal = FactoryHelper.Instance<ICoupon>(Global.DataProvider, "CouponDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Coupon");
        public static int Add(CouponInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(CouponInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids)
        {
            UserCouponBLL.Delete(ids);
            dal.Delete(ids);
        }

        public static CouponInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<CouponInfo> SearchList(CouponSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }
        /// <summary>
        /// 获取所有在有效期内的优惠券
        /// </summary>
        /// <returns></returns>
        public static List<CouponInfo> SearchListCanUse()
        {
            return dal.SearchListCanUse();
        }
        public static List<CouponInfo> SearchList(int currentPage, int pageSize, CouponSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }

        /// <summary>
        /// 根据优惠券列表，优惠券ID读取优惠券
        /// </summary>
        /// <param name="bookingCouponList"></param>
        /// <returns></returns>
        public static CouponInfo ReadCouponByCouponList(List<CouponInfo> couponList, int couponID)
        {
            CouponInfo coupon = new CouponInfo();
            foreach (CouponInfo temp in couponList)
            {
                if (temp.Id == couponID)
                {
                    coupon = temp;
                }
            }
            return coupon;
        }
    }
}