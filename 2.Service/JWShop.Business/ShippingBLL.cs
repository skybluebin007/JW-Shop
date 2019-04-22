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
    public sealed class ShippingBLL : BaseBLL
    {
        private static readonly IShipping dal = FactoryHelper.Instance<IShipping>(Global.DataProvider, "ShippingDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("Shipping");

        public static int Add(ShippingInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ShippingInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
            CacheHelper.Remove(cacheKey);
        }

        public static ShippingInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ShippingInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ShippingInfo();
        }

        public static List<ShippingInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ShippingInfo>)CacheHelper.Read(cacheKey);
        }

        public static void Move(ChangeAction action, int id)
        {
            dal.Move(action, id);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 从缓存中读取可用的物流列表
        /// </summary>
        /// <returns>物流数据列表</returns>
        public static List<ShippingInfo> ReadShippingIsEnabledCacheList()
        {
            List<ShippingInfo> shippingList = new List<ShippingInfo>();
            foreach (ShippingInfo shipping in ReadList())
            {
                if (shipping.IsEnabled == (int)BoolType.True)
                {
                    shippingList.Add(shipping);
                }
            }
            return shippingList;
        }

    }
}