using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Mobile
{
   public  class OrderPoint:UserBasePage
    {
        protected List<PointProductOrderInfo> orderList = new List<PointProductOrderInfo>();
        protected CommonPagerClass pager = new CommonPagerClass();
        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "ReceiveShipping") this.ReceiveShipping();
             int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int count = 0, pageSize = 10;

            orderList = PointProductOrderBLL.ReadList(base.UserId);
            orderList = PointProductOrderBLL.SearchList(currentPage, pageSize, new PointProductOrderSearchInfo { UserId = base.UserId }, ref count);         
            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
            Title = "兑换记录";
        }

        private void ReceiveShipping()
        {
            int id = RequestHelper.GetQueryString<int>("orderId");
            var pointProductOrder = PointProductOrderBLL.Read(id);
            if (pointProductOrder.OrderStatus == (int)PointProductOrderStatus.HasShipping)
            {
                pointProductOrder.OrderStatus = (int)PointProductOrderStatus.ReceiveShipping;
                PointProductOrderBLL.Update(pointProductOrder);
            }

            ResponseHelper.End();
        }
    }
}
