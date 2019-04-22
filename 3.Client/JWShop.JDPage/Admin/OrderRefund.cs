using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using System.Web.Security;

namespace JWShop.Page.Admin
{
    public class OrderRefund:AdminBase
    {
        protected int pageSize = 6;
        protected int orderStatus = 0;
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();
        protected List<OrderInfo> allOrderList = new List<OrderInfo>();
      
        protected override void PageLoad()
        {
            base.PageLoad();
            OrderRefundSearchInfo searchInfo = new OrderRefundSearchInfo();
            searchInfo.RefundNumber = RequestHelper.GetQueryString<string>("RefundNumber");
            searchInfo.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
            searchInfo.Status = RequestHelper.GetQueryString<int>("Status");
            searchInfo.StartTmCreate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
            searchInfo.EndTmCreate = RequestHelper.GetQueryString<DateTime>("EndAddDate");

            orderRefundList = OrderRefundBLL.SearchList(1, pageSize, searchInfo, ref Count);
            allOrderList = OrderBLL.ReadList();

            topNav = 0;      
        }
    }
}
