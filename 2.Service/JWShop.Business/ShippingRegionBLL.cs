using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class ShippingRegionBLL : BaseBLL
    {
        private static readonly IShippingRegion dal = FactoryHelper.Instance<IShippingRegion>(Global.DataProvider, "ShippingRegionDAL");

        public static int Add(ShippingRegionInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(ShippingRegionInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static ShippingRegionInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<ShippingRegionInfo> ReadList(int shippingId)
        {
            return dal.ReadList(shippingId);
        }

        public static List<ShippingRegionInfo> ReadList(int[] shippingIds)
        {
            return dal.ReadList(shippingIds);
        }

        /// <summary>
        /// 搜索匹配的配送地区
        /// </summary>
        /// <param name="shippingID"></param>
        /// <param name="regionID"></param>
        /// <returns></returns>
        public static ShippingRegionInfo SearchShippingRegion(int shippingId, string regionId)
        {
            List<ShippingRegionInfo> shippingRegionList = ShippingRegionBLL.ReadList(shippingId);
            ShippingRegionInfo shippingRegion = new ShippingRegionInfo();
            if (!string.IsNullOrEmpty(regionId))
            {
                while (regionId.Length >= 1)
                {
                    foreach (ShippingRegionInfo temp in shippingRegionList)
                    {
                        if (("|" + temp.RegionId + "|").IndexOf("|" + regionId + "|") > -1)
                        {
                            shippingRegion = temp;
                            break;
                        }
                    }
                    if (shippingRegion.Id > 0)
                    {
                        break;
                    }
                    else
                    {
                        regionId = regionId.Substring(0, regionId.Length - 1);
                        regionId = regionId.Substring(0, regionId.LastIndexOf('|') + 1);
                    }
                }
            }
            return shippingRegion;
        }
        /// <summary>
        /// 地区1是否在地区2的范围内
        /// </summary>
        /// <param name="regionId1"></param>
        /// <param name="regionId2"></param>
        /// <returns></returns>
        public static bool IsRegionIn(string regionId1, string regionId2)
        {
            bool result = false;
            regionId2 = "|" + regionId2 + "|";
            if (regionId1 != string.Empty)
            {
                while (regionId1.Length >= 1)
                {
                    if (regionId2.IndexOf("|" + regionId1 + "|") > -1)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        regionId1 = regionId1.Substring(0, regionId1.Length - 1);
                        regionId1 = regionId1.Substring(0, regionId1.LastIndexOf('|') + 1);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据配送方式，配送地址，读取邮费价格
        /// </summary>
        /// <returns></returns>
        public static decimal ReadShippingMoney(int shippingId, string regionId, List<CartInfo> cartList)
        {
            decimal shippingMoney = 0;
            ShippingInfo shipping = ShippingBLL.Read(shippingId);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shippingId, regionId);
            switch (shipping.ShippingType)
            {
                case (int)ShippingType.Fixed:
                    shippingMoney = shippingRegion.FixedMoeny;
                    break;
                case (int)ShippingType.Weight:
                    decimal cartProductWeight = cartList.Sum(k => k.BuyCount * k.Product.Weight);
                    if (cartProductWeight <= shipping.FirstWeight)
                    {
                        shippingMoney = shippingRegion.FirstMoney;
                    }
                    else
                    {
                        shippingMoney = shippingRegion.FirstMoney + Math.Ceiling((cartProductWeight - shipping.FirstWeight) / shipping.AgainWeight) * shippingRegion.AgainMoney;
                    }
                    break;
                case (int)ShippingType.ProductCount:
                    int cartProductCount = cartList.Sum(k => k.BuyCount);
                    shippingMoney = shippingRegion.OneMoeny + (cartProductCount - 1) * shippingRegion.AnotherMoeny;
                    break;
                default:
                    break;
            }

            return shippingMoney;
        }
        /// <summary>
        /// 根据配送方式，配送地址，读取邮费价格【按单个商品独立计算运费】
        /// 如买了2件商品，一件商品买了2个，一件商品买了1个，则需计算两次，第一次重量叠加计算（相同商品购买多个则叠加计算）
        /// </summary>
        /// <returns></returns>
        public static decimal ReadShippingMoney(ShippingInfo shipping, ShippingRegionInfo shippingRegion, CartInfo cart)
        {
            decimal shippingMoney = 0;
            switch (shipping.ShippingType)
            {
                case (int)ShippingType.Fixed:
                    shippingMoney = shippingRegion.FixedMoeny;
                    break;
                case (int)ShippingType.Weight:
                    decimal cartProductWeight = cart.BuyCount * cart.Product.Weight;
                    if (cartProductWeight <= shipping.FirstWeight)
                    {
                        shippingMoney = shippingRegion.FirstMoney;
                    }
                    else
                    {
                        shippingMoney = shippingRegion.FirstMoney + Math.Ceiling((cartProductWeight - shipping.FirstWeight) / shipping.AgainWeight) * shippingRegion.AgainMoney;
                    }
                    break;
                case (int)ShippingType.ProductCount:
                    int cartProductCount = cart.BuyCount;
                    shippingMoney = shippingRegion.OneMoeny + (cartProductCount - 1) * shippingRegion.AnotherMoeny;
                    break;
                default:
                    break;
            }

            return shippingMoney;
        }
        /// <summary>
        ///  买1件商品，根据配送方式，配送地址，读取邮费价格【按单个商品独立计算运费】
        /// </summary>
        /// <param name="shipping"></param>
        /// <param name="shippingRegion"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public static decimal ReadShippingMoney(int shippingId, string regionId, ProductInfo product)
        {
            decimal shippingMoney = 0;
            ShippingInfo shipping = ShippingBLL.Read(shippingId);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shippingId, regionId);
            switch (shipping.ShippingType)
            {
                case (int)ShippingType.Fixed:
                    shippingMoney = shippingRegion.FixedMoeny;
                    break;
                case (int)ShippingType.Weight:
                    decimal cartProductWeight = 1 * product.Weight;
                    if (cartProductWeight <= shipping.FirstWeight)
                    {
                        shippingMoney = shippingRegion.FirstMoney;
                    }
                    else
                    {
                        shippingMoney = shippingRegion.FirstMoney + Math.Ceiling((cartProductWeight - shipping.FirstWeight) / shipping.AgainWeight) * shippingRegion.AgainMoney;
                    }
                    break;
                case (int)ShippingType.ProductCount:
                    int cartProductCount = 1;
                    shippingMoney = shippingRegion.OneMoeny + (cartProductCount - 1) * shippingRegion.AnotherMoeny;
                    break;
                default:
                    break;
            }

            return shippingMoney;
        }
    }
}