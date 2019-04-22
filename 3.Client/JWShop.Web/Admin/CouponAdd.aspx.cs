using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class CouponAdd : JWShop.Page.AdminBasePage
    {
        //优惠券的类别
        protected int couponKind = -1;

       protected  CouponInfo coupon = new CouponInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int couponID = RequestHelper.GetQueryString<int>("ID");
                //默认优惠券类别：通用(1)
                couponKind = RequestHelper.GetQueryString<int>("couponkind")< (int)CouponKind.Common || RequestHelper.GetQueryString<int>("couponkind")> (int)CouponKind.BirthdayGet ? (int)CouponKind.Common:RequestHelper.GetQueryString<int>("couponkind");
                Type.Text = couponKind.ToString();
                if (couponID != int.MinValue)
                {
                    CheckAdminPower("ReadCoupon", PowerCheckType.Single);
                    coupon = CouponBLL.Read(couponID);
                    Name.Text = coupon.Name;
                    Money.Text = coupon.Money.ToString();
                    UseMinAmount.Text = coupon.UseMinAmount.ToString();
                    UseStartDate.Text = coupon.UseStartDate.ToString("yyyy-MM-dd");
                    UseEndDate.Text = coupon.UseEndDate.ToString("yyyy-MM-dd");
                    Type.Text = coupon.Type.ToString();
                    if (couponKind == (int)CouponKind.Common)
                    {//商家优惠券可以设置图片、总数
                        TotalCount.Text = coupon.TotalCount.ToString();
                        Photo.Text = coupon.Photo;
                    }
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CouponInfo coupon = new CouponInfo();
            coupon.Id = RequestHelper.GetQueryString<int>("ID");
            coupon.Name = Name.Text;
            coupon.Money = Convert.ToDecimal(Money.Text);
            coupon.UseMinAmount = Convert.ToDecimal(UseMinAmount.Text);
            coupon.UseStartDate = Convert.ToDateTime(UseStartDate.Text);
            coupon.UseEndDate = Convert.ToDateTime(UseEndDate.Text).AddDays(1).AddSeconds(-1);
            coupon.Type =Convert.ToInt32(Type.Text);
            if (coupon.UseEndDate < coupon.UseStartDate)
            {
                ScriptHelper.Alert("结束日期不得小于开始日期");
            }
            if (coupon.Type == (int)CouponKind.Common)
            {//商家优惠券可以设置图片、总数
                coupon.TotalCount = Convert.ToInt32(TotalCount.Text) < 0 ? 0 : Convert.ToInt32(TotalCount.Text);              
                coupon.Photo = Photo.Text.Trim();
            }
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (coupon.Id == int.MinValue)
            {
                CheckAdminPower("AddCoupon", PowerCheckType.Single);               
                coupon.Id = CouponBLL.Add(coupon);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Coupon"), coupon.Id);
            }
            else
            {
                CheckAdminPower("UpdateCoupon", PowerCheckType.Single);
                if (coupon.Type == (int)CouponKind.Common)
                {//商家优惠券总数量不能小于已发放量                
                    if (coupon.TotalCount < CouponBLL.Read(coupon.Id).UsedCount) { ScriptHelper.Alert("总数不得小于已发放数量"); }                  
                }
                CouponBLL.Update(coupon);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Coupon"), coupon.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}