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
    public class CouponAdd:AdminBase
    {
        //优惠券的类别
        protected int couponKind = -1;
        protected CouponInfo entity = new CouponInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            int couponID = RequestHelper.GetQueryString<int>("id");
            if (couponID > 0) entity = CouponBLL.Read(couponID);
            //默认优惠券类别：通用(1)
            couponKind = RequestHelper.GetQueryString<int>("couponkind") < (int)CouponKind.Common || RequestHelper.GetQueryString<int>("couponkind") > (int)CouponKind.BirthdayGet ? (int)CouponKind.Common : RequestHelper.GetQueryString<int>("couponkind");
        }
    }
}
