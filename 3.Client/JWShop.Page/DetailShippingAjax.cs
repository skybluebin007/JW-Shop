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
    public class DetailShippingAjax : AjaxBasePage
    {
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "SelectShipping") this.SelectShipping();

            string idList = CookiesHelper.ReadCookieValue("usr_region");
            if (string.IsNullOrEmpty(idList))
            {
                string city = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("city"));
                if (!string.IsNullOrEmpty(city))
                {
                    idList = RegionBLL.ReadRegionIdList(city);
                    CookiesHelper.AddCookie("usr_region", idList, 1, TimeType.Year);
                }
            }

            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.ClassID = idList;
            singleUnlimitClass.FunctionName = "readShippingMoney()";
        }

        private void SelectShipping()
        {
            int productId = RequestHelper.GetQueryString<int>("productId");

            if (productId < 1)
            {
                ResponseHelper.Write("-1");
                ResponseHelper.End();
            }

            SingleUnlimitClass singleUnlimitClass2 = new SingleUnlimitClass();
            string regionId = singleUnlimitClass2.ClassID;
            
            //当前产品
            var product = ProductBLL.Read(productId);
            //所有可用的配送方式
            var shippingList = ShippingBLL.ReadList().Where(k => k.IsEnabled == (int)BoolType.True).ToList();
            
            //无可用的配送方式，返回-10
            if (shippingList.Count < 1)
            {
                ResponseHelper.Write("-10");
                ResponseHelper.End();
            }

            var shippingIds = new List<int>();
            shippingList.ForEach(k => shippingIds.Add(k.Id));

            //读取配送区域列表
            List<ShippingRegionInfo> shippingRegionList = ShippingRegionBLL.ReadList(shippingIds.ToArray());

            //该区域没有物流配送，返回-11
            if (shippingRegionList.Count < 1)
            {
                ResponseHelper.Write("-11");
                ResponseHelper.End();
            }

            //比较匹配出来的配送数据，取最高价（只计算单件商品的价格）
            List<decimal> listShippingMoney = new List<decimal>();
            foreach (ShippingInfo shipping in shippingList)
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

                            decimal shippingMoney = 0;
                            switch (shipping.ShippingType)
                            {
                                case (int)ShippingType.Fixed:
                                    shippingMoney = temp.FixedMoeny;
                                    listShippingMoney.Add(shippingMoney);
                                    break;
                                case (int)ShippingType.Weight:
                                    if (product.Weight <= shipping.FirstWeight)
                                    {
                                        shippingMoney = temp.FirstMoney;
                                    }
                                    else
                                    {
                                        shippingMoney = temp.FirstMoney + Math.Ceiling((product.Weight - shipping.FirstWeight) / shipping.AgainWeight) * temp.AgainMoney;
                                    }
                                    listShippingMoney.Add(shippingMoney);
                                    break;
                                case (int)ShippingType.ProductCount:
                                    int productCount = 1;
                                    shippingMoney = temp.OneMoeny + (productCount - 1) * temp.AnotherMoeny;
                                    listShippingMoney.Add(shippingMoney);
                                    break;
                                default:
                                    break;
                            }
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

            CookiesHelper.AddCookie("usr_region", regionId, 1, TimeType.Year);
            //该区域没有物流配送，返回-11
            if (listShippingMoney.Count < 1)
            {
                ResponseHelper.Write("-11");
                ResponseHelper.End();
            }
            else
            {
                ResponseHelper.Write(listShippingMoney.Max().ToString());
                ResponseHelper.End();
            }
        }

    }
}