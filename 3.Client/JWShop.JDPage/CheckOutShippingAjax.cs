﻿using System;
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
        protected List<ShippingInfo> shippingList = new List<ShippingInfo>();
        protected UserAddressInfo address = new UserAddressInfo();
        protected List<FavorableActivityInfo> favorableActivityList = new List<FavorableActivityInfo>();
       
        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "SelectShipping") this.SelectShipping();

            int id = RequestHelper.GetQueryString<int>("id");
            address = UserAddressBLL.Read(id, base.UserId);

            if (string.IsNullOrEmpty(address.RegionId)) return;

            //取出所有配送方式Id
            List<ShippingInfo> tempShippingList = ShippingBLL.ReadList();
            tempShippingList = tempShippingList.Where(k => k.IsEnabled == (int)BoolType.True).ToList();

            var shippingIds = new List<int>();
            tempShippingList.ForEach(k => shippingIds.Add(k.Id));

            //读取配送区域列表
            List<ShippingRegionInfo> shippingRegionList = ShippingRegionBLL.ReadList(shippingIds.ToArray());
            #region 读取购物车结算金额
            string checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                ResponseHelper.Write("error|请选择需要购买的商品");
                ResponseHelper.End();
            }
            //计算配送费用
            List<CartInfo> cartList = CartBLL.ReadList(base.UserId).Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count < 1)
            {
                ResponseHelper.Write("error|请选择需要购买的商品");
                ResponseHelper.End();
            }
            int count = 0;
            //购物车结算金额
            decimal cartTotalPrice = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);
            cartList.ForEach(k => k.Product = productList.FirstOrDefault(k2 => k2.Id == k.ProductId) ?? new ProductInfo());
            foreach (var cart in cartList)
            {
                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cartTotalPrice += ProductBLL.GetCurrentPrice(standardRecord.SalePrice,base.GradeID)*cart.BuyCount;
                }
                else
                {
                    cartTotalPrice += ProductBLL.GetCurrentPrice(cart.Product.SalePrice,base.GradeID)*cart.BuyCount;
                }
            }
            #endregion
            #region 获取符合条件（时间段，用户等级，金额限制）的整站订单优惠活动列表，默认使用第一个
            favorableActivityList = FavorableActivityBLL.ReadList(DateTime.Now, DateTime.Now).Where<FavorableActivityInfo>(f => f.Type == (int)FavorableType.AllOrders && ("," + f.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1 && cartTotalPrice >= f.OrderProductMoney).ToList();           
            #endregion
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
            int favorId = RequestHelper.GetQueryString<int>("favorId");
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
            //购物车结算金额
            decimal cartTotalPrice = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);
            cartList.ForEach(k => k.Product = productList.FirstOrDefault(k2 => k2.Id == k.ProductId) ?? new ProductInfo());
            foreach (var cart in cartList)
            {
                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cartTotalPrice+= ProductBLL.GetCurrentPrice(standardRecord.SalePrice,base.GradeID)*cart.BuyCount;
                }
                else
                {
                    cartTotalPrice += ProductBLL.GetCurrentPrice(cart.Product.SalePrice,base.GradeID)*cart.BuyCount;  
                }
            }

            ShippingInfo shipping = ShippingBLL.Read(shippingId);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shippingId, address.RegionId);

            decimal shippingMoney = ShippingRegionBLL.ReadShippingMoney(shippingId, address.RegionId, cartList);
            decimal favorableMoney = 0;
            #region 计算优惠费用--原始方式           
            //FavorableActivityInfo favorableActivity = FavorableActivityBLL.Read(DateTime.Now, DateTime.Now, 0);
            //if (favorableActivity.Id > 0)
            //{
            //    if (("," + favorableActivity.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1 && Sessions.ProductTotalPrice >= favorableActivity.OrderProductMoney)
            //    {
            //        switch (favorableActivity.ReduceWay)
            //        {
            //            case (int)FavorableMoney.Money:
            //                favorableMoney += favorableActivity.ReduceMoney;
            //                break;
            //            case (int)FavorableMoney.Discount:
            //                favorableMoney += Sessions.ProductTotalPrice * (100 - favorableActivity.ReduceDiscount) / 100;
            //                break;
            //            default:
            //                break;
            //        }
            //        if (favorableActivity.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(address.RegionId, favorableActivity.RegionId))
            //        {
            //            favorableMoney += shippingMoney;
            //        }
            //    }
            //}
            #endregion
            #region 计算优惠费用
            if (favorId > 0) {
                var theFavorable = FavorableActivityBLL.Read(favorId);
                if (("," + theFavorable.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1 && cartTotalPrice >= theFavorable.OrderProductMoney)
                {
                    switch (theFavorable.ReduceWay)
                    {
                        case (int)FavorableMoney.Money:
                            favorableMoney += theFavorable.ReduceMoney;
                            break;
                        case (int)FavorableMoney.Discount:
                            favorableMoney += cartTotalPrice * (100 - theFavorable.ReduceDiscount) / 100;
                            break;
                        default:
                            break;
                    }
                    if (theFavorable.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(address.RegionId, theFavorable.RegionId))
                    {
                        favorableMoney += shippingMoney;
                    }
                }
            }
            #endregion
            ResponseHelper.Write("ok|" + Math.Round(shippingMoney, 2).ToString()+"|"+Math.Round(favorableMoney,2));
            ResponseHelper.End();
        }

    }
}