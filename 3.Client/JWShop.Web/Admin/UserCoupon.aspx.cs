using System;
using System.Text;
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
    public partial class UserCoupon : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadUserCoupon", PowerCheckType.Single);
                UserCouponSearchInfo userCouponSearch = new UserCouponSearchInfo();
                userCouponSearch.CouponId = RequestHelper.GetQueryString<int>("CouponID");
                userCouponSearch.GetType = RequestHelper.GetQueryString<int>("GetType");
                userCouponSearch.Number = RequestHelper.GetQueryString<string>("Number");
                userCouponSearch.IsUse = RequestHelper.GetQueryString<int>("IsUse");
                 userCouponSearch.UserId = RequestHelper.GetQueryString<int>("UserID");
                userCouponSearch.IsTimeOut = -1;//所有
                GetType.Text = RequestHelper.GetQueryString<string>("GetType");
                Number.Text = RequestHelper.GetQueryString<string>("Number");
                IsUse.Text = RequestHelper.GetQueryString<string>("IsUse");
                PageSize = 10;
                BindControl(UserCouponBLL.SearchList(CurrentPage, PageSize, userCouponSearch, ref Count), RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteUserCoupon", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                UserCouponBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)), 0);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("UserCoupon"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            UserCouponSearchInfo userCouponSearch = new UserCouponSearchInfo();
            userCouponSearch.CouponId = RequestHelper.GetQueryString<int>("CouponID");
            userCouponSearch.UserId = RequestHelper.GetQueryString<int>("UserID");
            userCouponSearch.GetType = RequestHelper.GetQueryString<int>("GetType");
            userCouponSearch.Number = RequestHelper.GetQueryString<string>("Number");
            userCouponSearch.IsUse = RequestHelper.GetQueryString<int>("IsUse");
            List<UserCouponInfo> userCouponList = UserCouponBLL.SearchList(userCouponSearch);
            StringBuilder sb = new StringBuilder();
            sb.Append("ID\t优惠券编号\t密码\t获取类型\t所属用户\t是否使用\n");
            foreach (UserCouponInfo userCoupon in userCouponList)
            {
                sb.Append(userCoupon.Id.ToString() + "\t#" + userCoupon.Number + "\t" + userCoupon.Password + "\t" + userCoupon.GetType + "\t" + userCoupon.UserName + "\t" + ShopCommon.GetBoolString(userCoupon.IsUse) + "\n");
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=userCoupon.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/ms-excel";
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserCoupon.aspx?Action=search&";
            URL += "UserID=" + RequestHelper.GetQueryString<int>("UserID") + "&";
            URL += "CouponID=" + RequestHelper.GetQueryString<int>("CouponID") + "&";
            URL += "GetType=" + GetType.Text + "&";
            URL += "Number=" + Number.Text + "&";
            URL += "IsUse=" + IsUse.Text;
            ResponseHelper.Redirect(URL);
        }

        /// <summary>
        /// 显示订单信息
        /// </summary>
        protected string ReadOrderLink(int orderID)
        {
            string result = string.Empty;
            if (orderID > 0)
            {
                result = "<a href=\"OrderDetail.aspx?ID=" + orderID + "\" target=\"_blank\" >查看订单</a>";
            }
            else
            {
                result = "未消费";
            }
            return result;
        }
    }
}