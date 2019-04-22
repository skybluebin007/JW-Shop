using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page
{
    public class CouponDetail : CommonBasePage
    {
        protected CouponInfo coupon = new CouponInfo();
        protected bool isOutOfDate = false;
        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("Id");
            coupon = CouponBLL.Read(id);
            //如果已经结束或者暂未开始
            if (coupon.UseEndDate.Date < DateTime.Now.Date || coupon.UseStartDate.Date > DateTime.Now.Date) isOutOfDate = true;
         

            Title = coupon.Name + " - 优惠券领取";
        }
    }
}
