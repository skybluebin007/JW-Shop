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
    public class Index : AdminBase
    {     
        protected int pageSize =6;
        protected int orderStatus = 0;
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            //检查待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime();
            //订单自动收货
            OrderBLL.CheckOrderRecieveTimeProg();
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
            orderStatus = RequestHelper.GetQueryString<int>("OrderStatus")<=0?2: RequestHelper.GetQueryString<int>("OrderStatus");
            //如果查找已删除订单
            if (orderStatus == (int)Entity.OrderStatus.HasDelete)
            {
                orderSearch.IsDelete = (int)BoolType.True;//已删除
            }
            else
            {
                orderSearch.OrderStatus = orderStatus;
                orderSearch.IsDelete = (int)BoolType.False;//未删除
            }
            orderSearch.Consignee = RequestHelper.GetQueryString<string>("Consignee");
            orderSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
            orderSearch.EndAddDate = RequestHelper.GetQueryString<DateTime>("EndAddDate");

            orderList = OrderBLL.SearchList(1, pageSize, orderSearch, ref Count);

            if(orderStatus==2 || orderStatus == 4)
            {
                topNav = 0;
            }
        }
    }
}
