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
    public partial class Coupon : JWShop.Page.AdminBasePage
    {
        //优惠券的类别
        protected int couponKind = -1;
        //优惠券开始状态，默认 ：进行中
        protected int timeperiod = 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadCoupon", PowerCheckType.Single);
                couponKind = RequestHelper.GetQueryString<int>("couponkind");
                //开始状态：未开始、进行中、已结束
                 timeperiod = RequestHelper.GetQueryString<int>("timeperiod")<1?2: RequestHelper.GetQueryString<int>("timeperiod");
                
                CouponSearchInfo searchInfo = new CouponSearchInfo();
                if (couponKind > 0) searchInfo.Type = couponKind;
                if (timeperiod > 0) searchInfo.TimePeriod = timeperiod;
                List<CouponInfo> couponList = CouponBLL.SearchList(CurrentPage, PageSize, searchInfo, ref Count);
                BindControl(couponList, RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteCoupon", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                CouponBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Coupon"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 是否显示修改链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string ShowUpdatehref(object id) {
            string result =string.Empty;
            int _id = 0;
            int.TryParse(id.ToString(), out  _id);
            CouponInfo coupon = CouponBLL.Read(_id);
            if (coupon.Id > 0 && coupon.UseStartDate > DateTime.Now) {
                result = "<a href=\"CouponAdd.aspx?ID=" + id + "\" >修改</a>";
            }
           return result;
        }
        /// <summary>
        /// 是否显示发放优惠券链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string ShowSendhref(object id)
        {
            string result = string.Empty;
            int _id = 0;
            int.TryParse(id.ToString(), out  _id);
            CouponInfo coupon = CouponBLL.Read(_id);
            //通用类别，在有效期内才能发放
            if (coupon.Id > 0 && coupon.UseEndDate >= DateTime.Now && coupon.Type<2)
            {
                result = " <a href=\"javascript:loadPage('CouponSend.aspx?CouponID=" + id + "','发放优惠券','800px','650px')\">发放优惠券</a>";
            }
            return result;
        }
    }
}