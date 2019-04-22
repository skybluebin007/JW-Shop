using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.Common;

namespace JWShop.Web.Admin
{
    public partial class GroupSign :JWShop.Page.AdminBasePage
    {
        protected GroupBuyInfo group = new GroupBuyInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckAdminPower("ReadGroupSign", PowerCheckType.Single);
                int groupId = RequestHelper.GetQueryString<int>("GroupId");
                group = GroupBuyBLL.Read(groupId);
                var dataList = GroupSignBLL.SearchListByGroupId(groupId,1,group.Quantity,ref Count);
                dataList.ForEach(k => k.UserName = HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
                BindControl(dataList, RecordList);
            }
        }
        /// <summary>
        /// 判断是否待退款
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Quantity"></param>
        /// <param name="SignCount"></param>
        /// <param name="GroupOrderStatus"></param>
        /// <param name="IsRefund"></param>
        /// <returns></returns>
        protected string NeedRefund(object StartTime,object EndTime,object Quantity, object SignCount,object orderId,object GroupOrderStatus,object IsRefund)
        {
            string result = string.Empty;
            try
            {
                if(Convert.ToDateTime(EndTime) < DateTime.Now && Convert.ToInt32(Quantity) > Convert.ToInt32(SignCount) && Convert.ToInt32(GroupOrderStatus) == 2 && Convert.ToInt32(IsRefund) == 0)
                {
                    result = "<td><a id=\"refund_" + orderId + "\" href=\"javascript: GroupBuyOrderRefund(" + orderId + ")\">退款</a></td>";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        /// <summary>
        /// 判断是否待审核
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Quantity"></param>
        /// <param name="SignCount"></param>
        /// <param name="GroupOrderStatus"></param>
        /// <param name="IsRefund"></param>
        /// <returns></returns>
        protected string NeedCheck(object StartTime, object EndTime, object Quantity, object SignCount, object orderId, object GroupOrderStatus, object IsRefund)
        {
            string result = string.Empty;
            try
            {
                if(Convert.ToDateTime(StartTime) <= DateTime.Now && Convert.ToDateTime(EndTime) >= DateTime.Now && Convert.ToInt32(SignCount) >= Convert.ToInt32(Quantity) && Convert.ToInt32(GroupOrderStatus) == 2)
                {
                    result = "<td><a href=\"/admin/orderdetail.aspx?Id=" + orderId + "\">审核</a></td>";
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }
    }
}