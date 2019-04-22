using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class SaleTotal : JWShop.Page.AdminBasePage
    {
        protected string queryString = string.Empty;
        protected DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsSale", PowerCheckType.Single);
                ShopCommon.BindYearMonth(Year, Month);
                int year = RequestHelper.GetQueryString<int>("Year");
                year = (year == int.MinValue) ? DateTime.Now.Year : year;
                int month = RequestHelper.GetQueryString<int>("Month");
                Year.Text = year.ToString();
                Month.Text = month.ToString();
                queryString = "?Date=" + year.ToString() + "|" + month.ToString();

                //SaleTotalData(queryString, ref dt);
            }
        }        

        protected void SaleTotalData(string date, ref DataTable dt)
        {
            int days = 0;
            int year = Convert.ToInt32(date.Split('|')[0]);
            int month = Convert.ToInt32(date.Split('|')[1]);
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
            dt = OrderBLL.StatisticsSaleTotal(orderSearch, dateType);


            string jsonStr = "[";
            string countStr = string.Empty;
            string moneyStr = string.Empty;
            if (month == int.MinValue)
            {
                for (int i = 1; i <= 12; i++)
                {
                    int countNum = 0;
                    int moneyNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                            moneyNum = Convert.ToInt32(dr[2]);
                        }
                    }
                    countStr += countNum + ",";
                    moneyStr += moneyNum + ",";
                }
            }
            else
            {
                for (int i = 1; i <= days; i++)
                {
                    int countNum = 0;
                    int moneyNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                            moneyNum = Convert.ToInt32(dr[2]);
                        }
                    }
                    countStr += countNum + ",";
                    moneyStr += moneyNum + ",";
                }
            }
            countStr = countStr.Substring(0, countStr.Length - 1);
            countStr = "{name:'订单量',type:'line',data:[" + countStr + "]}";
            moneyStr = moneyStr.Substring(0, moneyStr.Length - 1);
            moneyStr = "{name:'销售额',type:'line',data:[" + moneyStr + "]}";
            jsonStr = countStr + "," + moneyStr + "]";
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "SaleTotal.aspx?Action=search&";
            URL += "Year=" + Year.Text + "&";
            URL += "Month=" + Month.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}