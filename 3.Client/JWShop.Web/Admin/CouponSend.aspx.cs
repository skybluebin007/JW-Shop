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
    public partial class CouponSend : JWShop.Page.AdminBasePage
    {
        protected CouponInfo coupon = new CouponInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadCoupon", PowerCheckType.Single);
                int couponID = RequestHelper.GetQueryString<int>("CouponID");
                coupon = CouponBLL.Read(couponID);
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("SendCoupon", PowerCheckType.Single);
            int sendCount = 0;
            int.TryParse(SendCount.Text.Trim(),out sendCount);
            int couponID = RequestHelper.GetQueryString<int>("CouponID");
            string sendUser = RequestHelper.GetForm<string>("RelationUser");
            if (sendCount + sendUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
            {
                CouponInfo coupon = CouponBLL.Read(couponID);
                UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponID);
                int startNumber = 0;
                if (userCoupon.Id > 0)
                {
                    string tempNumber = userCoupon.Number.Substring(3, 5);
                    while (tempNumber.Substring(0, 1) == "0")
                    {
                        tempNumber = tempNumber.Substring(1);
                    }
                    startNumber = Convert.ToInt32(tempNumber);
                }
                //如果线上、线下发放数量超过了优惠券剩余量
                if (coupon.UsedCount + sendCount + sendUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length > coupon.TotalCount)
                {
                    ScriptHelper.Alert("优惠券剩余量不足");
                }
                else
                {
                    CreateOfflineCoupon(couponID, sendCount, ref startNumber);
                    SeandUserCoupon(couponID, sendUser, ref startNumber);
                    //优惠券发放量增加
                    coupon.UsedCount += sendCount + sendUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[UsedCount]", coupon.UsedCount);
                    CouponBLL.UpdatePart("[Coupon]", dict, coupon.Id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("SendCoupon"), coupon.Id);
                string alertMessage = ShopLanguage.ReadLanguage("SendOK");
                ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
            }
            else
            {
                //ScriptHelper.Alert("请选择在线发放用户或输入线下发放数量");
                ScriptHelper.Alert("请选择在线发放用户");
            }
        }

        /// <summary>
        /// 生成线下优惠券
        /// </summary>
        private void CreateOfflineCoupon(int couponID, int sendCount, ref int startNumber)
        {
            if (sendCount > 0)
            {
                for (int i = 0; i < sendCount; i++)
                {
                    startNumber++;
                    UserCouponInfo userCoupon = new UserCouponInfo();
                    userCoupon.CouponId = couponID;
                    userCoupon.GetType = (int)CouponType.Offline;
                    userCoupon.Number = ShopCommon.CreateCouponNo(couponID, startNumber);
                    userCoupon.Password = ShopCommon.CreateCouponPassword(startNumber);
                    userCoupon.IsUse = (int)BoolType.False;
                    userCoupon.OrderId = 0;
                    userCoupon.UserId = 0;
                    userCoupon.UserName = string.Empty;
                    UserCouponBLL.Add(userCoupon);
                }
            }
        }

        /// <summary>
        /// 生成按用户发放的优惠券
        /// </summary>
        private void SeandUserCoupon(int couponID, string sendUser, ref int startNumber)
        {
            if (sendUser != string.Empty)
            {
                foreach (string user in sendUser.Split( new char[]{ ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    startNumber++;
                    int userID = Convert.ToInt32(user.Split('|')[0]);
                    string userName = user.Split('|')[1];
                    UserCouponInfo userCoupon = new UserCouponInfo();
                    userCoupon.CouponId = couponID;
                    userCoupon.GetType = (int)CouponType.Online;
                    userCoupon.Number = ShopCommon.CreateCouponNo(couponID, startNumber);
                    userCoupon.Password = ShopCommon.CreateCouponPassword(startNumber);
                    userCoupon.IsUse = (int)BoolType.False;
                    userCoupon.OrderId = 0;
                    userCoupon.UserId = userID;
                    userCoupon.UserName = userName;
                    UserCouponBLL.Add(userCoupon);
                }
            }
        }
    }
}