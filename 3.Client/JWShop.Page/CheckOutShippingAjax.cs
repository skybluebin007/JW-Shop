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

namespace JWShop.Page
{
    public class CheckOutShippingAjax : AjaxBasePage
    {
        /// <summary>
        /// 物流方式列表
        /// </summary>
        protected List<ShippingInfo> shippingList = new List<ShippingInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            string regionID = RequestHelper.GetQueryString<string>("RegionID");
            if (regionID != string.Empty)
            {
                //取出所有配送方式ID
                List<ShippingInfo> tempShippingList = ShippingBLL.ReadShippingIsEnabledCacheList();
                string strShippingID = string.Empty;
                foreach (ShippingInfo shipping in tempShippingList)
                {
                    if (strShippingID == string.Empty)
                    {
                        strShippingID = shipping.Id.ToString();
                    }
                    else
                    {
                        strShippingID += "," + shipping.Id.ToString();
                    }
                }
                //读取配送区域列表
                List<ShippingRegionInfo> shippingRegionList = ShippingRegionBLL.ReadList(Array.ConvertAll<string,int>(strShippingID.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries),k=>Convert.ToInt32(k)));
                //查找符合条件的配送方式
                foreach (ShippingInfo shipping in tempShippingList)
                {
                    string tempRegionID = regionID;
                    while (tempRegionID.Length >= 1)
                    {
                        bool isFind = false;
                        foreach (ShippingRegionInfo temp in shippingRegionList)
                        {
                            if (("|" + temp.RegionId + "|").IndexOf("|" + tempRegionID + "|") > -1 && temp.ShippingId == shipping.Id)
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
                            tempRegionID = tempRegionID.Substring(0, tempRegionID.Length - 1);
                            tempRegionID = tempRegionID.Substring(0, tempRegionID.LastIndexOf('|') + 1);
                        }
                    }
                }
            }
        }
    }
}