using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IUserCoupon
    {
        int Add(UserCouponInfo entity);
        void Update(UserCouponInfo entity);
        void Delete(int[] ids, int userId);
        void Delete(int[] couponIds);
        UserCouponInfo Read(int id, int userId);
        /// <summary>
        /// 读取最后一条用户优惠券数据
        /// </summary>
        UserCouponInfo ReadLast(int couponId);
        /// <summary>
        /// 通过卡号和密码读取一条用户优惠券数据
        /// </summary>
        UserCouponInfo Read(string number, string password);
        /// <summary>
        /// 通过订单读取一条用户优惠券数据
        /// </summary>
        UserCouponInfo Read(int orderId);
        /// <summary>
        /// 检查会员是否已领取过优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UniqueUserCatch(int userId = 0, int couponId = 0);
        List<UserCouponInfo> SearchList(UserCouponSearchInfo searchInfo);
        List<UserCouponInfo> SearchList(int currentPage, int pageSize, UserCouponSearchInfo searchInfo, ref int count);

        /// <summary>
        /// 读取订单中可用的的优惠券列表
        /// </summary>
        List<UserCouponInfo> ReadCanUse(int userId);
    }
}