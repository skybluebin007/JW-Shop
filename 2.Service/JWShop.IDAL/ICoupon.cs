using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface ICoupon
    {
        int Add(CouponInfo entity);
        void Update(CouponInfo entity);
        void Delete(int[] ids);
        CouponInfo Read(int id);
        List<CouponInfo> SearchList(CouponSearchInfo searchInfo);
        /// <summary>
        /// 获取所有在有效期内的优惠券
        /// </summary>
        /// <returns></returns>
        List<CouponInfo> SearchListCanUse();
        List<CouponInfo> SearchList(int currentPage, int pageSize, CouponSearchInfo searchInfo, ref int count);
    }
}