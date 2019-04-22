using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;

namespace JWShop.Page.Admin
{
   public class OrderDetail:AdminBase
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("id");
            order = OrderBLL.Read(id);
            order.OrderDetailList = OrderDetailBLL.ReadList(id);

            //正在处理中的退款订单或商品
            orderRefundList = OrderRefundBLL.ReadListValid(id);   
            
            if(order.OrderStatus==2 || order.OrderStatus == 4)
            {
                topNav = 0;
            }       
            else
            {
                topNav = 1;
            }
        }
    }
}
