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
    public partial class OrderStatusPage : JWShop.Page.AdminBasePage
    {
        protected string result = string.Empty;
        protected string statusArr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsOrder", PowerCheckType.Single);
                OrderSearchInfo orderSearch = new OrderSearchInfo();
                orderSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                orderSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                DataTable dt = OrderBLL.StatisticsOrderStatus(orderSearch);
                string[] color = { "33FF66", "FF6600", "FFCC33", "CC3399", "CC7036", "349802", "066C93", "336699" };
                int i = 0;
                bool find = false;
                result += "[";
                foreach (EnumInfo temp in EnumHelper.ReadEnumList<JWShop.Entity.OrderStatus>())
                {
                    statusArr = statusArr + "'" + temp.ChineseName + "',";
                    find = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt16(dr["OrderStatus"]) == temp.Value)
                        {
                            result += " {value:" + dr["Count"] + ", name:'" + temp.ChineseName + "'},";
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        result += " {value:0, name:'" + temp.ChineseName + "'},";
                    }
                    i++;
                }
                statusArr = statusArr.Substring(0, statusArr.Length - 1);
                result = result.Substring(0, result.Length - 1);
                result += "]";
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "OrderStatus.aspx?Action=search&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}