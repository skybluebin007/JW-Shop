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

namespace JWShop.Web.Admin
{
    public partial class SaleTotalData : JWShop.Page.AdminBasePage
    {
        protected int year = 0;
        protected int month = 0;
        protected int days = 0;
        protected Dictionary<int, int> orderCountDic = new Dictionary<int, int>();
        protected Dictionary<int, decimal> orderMoneyDic = new Dictionary<int, decimal>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string date = RequestHelper.GetQueryString<string>("Date");
            year = Convert.ToInt32(date.Split('|')[0]);
            month = Convert.ToInt32(date.Split('|')[1]);
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            DateType dateType = DateType.Day;
            if (month == int.MinValue)
            {
                dateType = DateType.Month;
                orderSearch.StartAddDate = Convert.ToDateTime(year + "-01-01");
                orderSearch.EndAddDate = Convert.ToDateTime(year + "-01-01").AddYears(1);
            }
            else
            {
                days = ShopCommon.CountMonthDays(year, month);
                orderSearch.StartAddDate = Convert.ToDateTime(year + "-" + month + "-01");
                orderSearch.EndAddDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1);
            }
            DataTable dt = OrderBLL.StatisticsSaleTotal(orderSearch, dateType);
            foreach (DataRow dr in dt.Rows)
            {
                orderCountDic.Add(Convert.ToInt32(dr[0]), Convert.ToInt32(dr[1]));
                orderMoneyDic.Add(Convert.ToInt32(dr[0]), Convert.ToDecimal(dr[2]));
            }
        }
    }
}