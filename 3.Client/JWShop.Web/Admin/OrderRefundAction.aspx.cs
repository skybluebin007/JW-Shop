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
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class OrderRefundAction : JWShop.Page.AdminBasePage
    {
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckAdminPower("ReadOrderRefundAction", PowerCheckType.Single);

                int orderId = RequestHelper.GetQueryString<int>("orderId");
                int orderDetailId = RequestHelper.GetQueryString<int>("orderDetailId");
                if (orderId > 0)
                {
                    orderRefundList = OrderRefundBLL.ReadList(orderId);
                    if (orderDetailId > 0)
                    {
                        orderRefundList = orderRefundList.Where(k => k.OrderDetailId == orderDetailId).ToList();
                    }
                }
            }
        }

    }
}