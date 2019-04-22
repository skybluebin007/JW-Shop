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
    public partial class OrderArea : JWShop.Page.AdminBasePage
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
                DataTable dt = OrderBLL.StatisticsOrderArea(orderSearch);
                result += "[";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string cityName = GetProvinceName(dr["RegionID"].ToString());
                        if (cityName == "") cityName = "其他";
                        statusArr = statusArr + "'" + cityName + "',";
                        result += " {value:" + dr["Count"] + ", name:'" + cityName + "'},";
                    }
                    statusArr = statusArr.Substring(0, statusArr.Length - 1);
                    result = result.Substring(0, result.Length - 1);
                }
                result += "]";
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "OrderArea.aspx?Action=search&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 获取省份的名称
        /// </summary>
        protected string GetProvinceName(string regionID)
        {
            string result = string.Empty;
            if (regionID != string.Empty)
            {
                string[] regionIDArray = regionID.Split('|');
                if (regionIDArray.Length >= 4)
                {
                    try
                    {
                        result = RegionBLL.ReadRegionCache(Convert.ToInt32(regionIDArray[2])).RegionName;
                    }
                    catch (Exception ex)
                    { }
                }
            }
            return result;
        }
    }
}
