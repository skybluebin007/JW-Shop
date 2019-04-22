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

namespace JWShop.Page.Lab
{
    public class CheckOutShippingAjax : AjaxBasePage
    {
        protected List<ShippingInfo> shippingList = new List<ShippingInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "SelectShipping") this.SelectShipping();

            int id = RequestHelper.GetQueryString<int>("id");
            var address = UserAddressBLL.Read(id, base.UserId);

            if (string.IsNullOrEmpty(address.RegionId)) return;

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
                string tempRegionId = address.RegionId;
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

        private void SelectShipping()
        {
            int shippingId = RequestHelper.GetQueryString<int>("shippingId");
            int addressId = RequestHelper.GetQueryString<int>("addressId");

            string checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                ResponseHelper.Write("error|请选择需要购买的商品");
                ResponseHelper.End();
            }

            var address = UserAddressBLL.Read(addressId, base.UserId);

            //计算配送费用
            List<CartInfo> cartList = CartBLL.ReadList(base.UserId).Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count < 1)
            {
                ResponseHelper.Write("error|请选择需要购买的商品");
                ResponseHelper.End();
            }

            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);
            cartList.ForEach(k => k.Product = productList.FirstOrDefault(k2 => k2.Id == k.ProductId) ?? new ProductInfo());

            decimal shippingMoney = 0;
            //首先根据ShopId分组，根据供应商的不同来分别计算运费
            //然后将分拆后的供应商商品，按单个商品独立计算运费（相同商品购买多个则叠加计算）
            ShippingInfo shipping = ShippingBLL.Read(shippingId);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shippingId, address.RegionId);

            var shopIds = cartList.GroupBy(k => k.Product.ShopId).Select(k => k.Key).ToList();
            foreach (var shopId in shopIds)
            {
                var shopCartList = cartList.Where(k => k.Product.ShopId == shopId).ToList();
                foreach (var shopCartSplit in shopCartList)
                {
                    shippingMoney += ShippingRegionBLL.ReadShippingMoney(shipping, shippingRegion, shopCartSplit);
                }
            }

            //decimal shippingMoney = ShippingRegionBLL.ReadShippingMoney(shippingId, address.RegionId, cartList);
            ResponseHelper.Write("ok|" + Math.Round(shippingMoney, 2).ToString());
            ResponseHelper.End();
        }

    }
}