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
using System.Linq;

namespace JWShop.Page
{
    public class OrderRefund : UserBasePage
    {
        protected List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            orderRefundList = OrderRefundBLL.ReadListByOwnerId(base.UserId);

            Title = "退款记录";
        }
    }
}