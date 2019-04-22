using System;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class OrderShippingAjax : JWShop.Page.AdminBasePage
    {
        protected List<ShippingInfo> shippingList = new List<ShippingInfo>();
        protected int orderShippingId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();
            orderShippingId = RequestHelper.GetQueryString<int>("ShippingID"); ;
            string regionId = RequestHelper.GetQueryString<string>("RegionID");
            if (!string.IsNullOrEmpty(regionId))
            {
                //取出所有配送方式Id
                List<ShippingInfo> tempShippingList = ShippingBLL.ReadList();
                tempShippingList = tempShippingList.Where(k => k.IsEnabled == (int)BoolType.True).ToList();

                var shippingIds = new List<int>();
                tempShippingList.ForEach(k => shippingIds.Add(k.Id));

                //读取配送区域列表
                List<ShippingRegionInfo> shippingRegionList = ShippingRegionBLL.ReadList(shippingIds.ToArray());

                //查找符合条件的配送方式
                foreach (ShippingInfo shipping in tempShippingList)
                {
                    string tempRegionId = regionId;
                    while (tempRegionId.Length >= 1)
                    {
                        bool isFind = false;
                        foreach (ShippingRegionInfo temp in shippingRegionList)
                        {
                            if (("|" + temp.RegionId + "|").IndexOf("|" + tempRegionId + "|") > -1 && temp.ShippingId == shipping.Id)
                            {
                                isFind = true;
                                shippingList.Add(shipping);
                                break;
                            }
                        }
                        if (isFind)
                        {
                            break;
                        }
                        else
                        {
                            tempRegionId = tempRegionId.Substring(0, tempRegionId.Length - 1);
                            tempRegionId = tempRegionId.Substring(0, tempRegionId.LastIndexOf('|') + 1);
                        }
                    }
                }
            }
        }
    }
}