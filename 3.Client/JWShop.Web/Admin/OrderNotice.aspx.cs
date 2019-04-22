using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Business;
using JWShop.Entity;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class OrderNotice : JWShop.Page.AdminBasePage
    {
        protected DataTable dt = new DataTable();

        protected string userCount = "0";
        protected int notNoticed = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            dt = ProductBLL.NoHandlerStatistics();

            var orderlist = OrderBLL.SearchList(new OrderSearchInfo() { IsNoticed = 0, OrderStatus = (int)OrderStatus.WaitCheck });
            notNoticed = orderlist.Count;
            if (orderlist.Count > 0)
            {
                int[] orderids = orderlist.Select(k => k.Id).ToArray();
                OrderBLL.UpdateIsNoticed(orderids, 1);
            }
        }
    }
}