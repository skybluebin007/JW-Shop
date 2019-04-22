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
    public class Order : AdminBase
    {     
        protected int pageSize = 6;
        protected int orderStatus = 0;
        protected int orderPeriod = 0;
        protected string searchKey = string.Empty;
        protected DateTime orderDate = DateTime.Now;
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            //检查待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime();
            //订单自动收货
            OrderBLL.CheckOrderRecieveTimeProg();

            OrderSearchInfo orderSearch = new OrderSearchInfo();
            //orderDate =string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("orderDate"))?orderDate: RequestHelper.GetQueryString<DateTime>("orderDate")<=DateTime.MinValue?orderDate: RequestHelper.GetQueryString<DateTime>("orderDate");
            searchKey = RequestHelper.GetQueryString<string>("searchKey");
            orderPeriod = RequestHelper.GetQueryString<int>("orderPeriod");
            orderStatus = RequestHelper.GetQueryString<int>("OrderStatus");
            orderSearch.OrderPeriod = orderPeriod;
            orderSearch.SearchKey = searchKey;
            //orderSearch.OrderDate = orderDate;
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

            topNav = 1;
        }
    }
}
