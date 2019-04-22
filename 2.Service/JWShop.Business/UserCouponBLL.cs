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
    public sealed class UserCouponBLL : BaseBLL
    {
        private static readonly IUserCoupon dal = FactoryHelper.Instance<IUserCoupon>(Global.DataProvider, "UserCouponDAL");

        public static int Add(UserCouponInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(UserCouponInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids, int userId)
        {
            dal.Delete(ids, userId);
        }

        public static void Delete(int[] couponIds)
        {
            dal.Delete(couponIds);
        }

        public static UserCouponInfo Read(int id, int userId)
        {
            return dal.Read(id, userId);
        }

        public static UserCouponInfo ReadLast(int couponId)
        {
            return dal.ReadLast(couponId);
        }

        public static UserCouponInfo Read(string number, string password)
        {
            return dal.Read(number, password);
        }

        public static UserCouponInfo Read(int orderId)
        {
            return dal.Read(orderId);
        }
        /// <summary>
        /// 检查会员是否已领取过优惠券,true:没领过，false：已领
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UniqueUserCatch(int userId = 0, int couponId = 0)
        {
            return dal.UniqueUserCatch(userId, couponId);
        }
        public static List<UserCouponInfo> SearchList(UserCouponSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<UserCouponInfo> SearchList(int currentPage, int pageSize, UserCouponSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
        
        public static List<UserCouponInfo> ReadCanUse(int userId)
        {
            return dal.ReadCanUse(userId);
        }
    }
}