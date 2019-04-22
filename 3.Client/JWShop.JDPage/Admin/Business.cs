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
    public class Business : AdminBase
    {
        protected string orderCount = "0";
        protected string orderMoney = "0";
        protected override void PageLoad()
        {
            base.PageLoad();

            //今天  营业额
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.StartPayDate = DateTime.Now.Date;
            orderSearch.EndPayDate = DateTime.Now.Date;
            orderCount = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderCount"].ToString();
            orderMoney = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderMoney"].ToString();
            if (string.IsNullOrEmpty(orderCount)) orderCount = "0";
            if (string.IsNullOrEmpty(orderMoney)) orderMoney = "0";

            topNav = 2;
        }
    }
}
