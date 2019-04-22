using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;

namespace JWShop.Page.Admin
{
    public class Coupon : AdminBase
    {
        //优惠券的类别
        protected int couponKind = -1;
        protected int pageSize = 1;
        protected int timePeriod = -1;
      protected  List<CouponInfo> couponList = new List<CouponInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            CheckAdminPower("ReadCoupon", PowerCheckType.Single);
            couponKind = RequestHelper.GetQueryString<int>("couponkind");
            //开始状态：未开始、进行中、已结束。默认: 进行中
            timePeriod = RequestHelper.GetQueryString<int>("timeperiod")>0? RequestHelper.GetQueryString<int>("timeperiod"):2;
           
            CouponSearchInfo searchInfo = new CouponSearchInfo();
            if (couponKind > 0) searchInfo.Type = couponKind;
            if (timePeriod > 0) searchInfo.TimePeriod = timePeriod;
            couponList = CouponBLL.SearchList(1, pageSize, searchInfo, ref Count);
        }
    }
}
