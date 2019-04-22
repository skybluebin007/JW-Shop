using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class ProductTypeStandardBLL : BaseBLL
    {
        private static readonly IProductTypeStandard dal = FactoryHelper.Instance<IProductTypeStandard>(Global.DataProvider, "ProductTypeStandardDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductTypeStandard");

        public static int Add(ProductTypeStandardInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ProductTypeStandardInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int productTypeId, int[] notInIds)
        {
            dal.Delete(productTypeId, notInIds);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteList(int productTypeId)
        {
            dal.DeleteList(productTypeId);
            CacheHelper.Remove(cacheKey);
        }

        public static ProductTypeStandardInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ProductTypeStandardInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ProductTypeStandardInfo();
        }
        public static ProductTypeStandardInfo Read(string name, int productTypeId) {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(name,productTypeId);
            }
            return ((List<ProductTypeStandardInfo>)CacheHelper.Read(cacheKey)).Where(k => k.Name == name).Where(k=>k.ProductTypeId==productTypeId).FirstOrDefault() ?? new ProductTypeStandardInfo();
        }
        public static List<ProductTypeStandardInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductTypeStandardInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ProductTypeStandardInfo> ReadList(int[] ids)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(ids);
            }
            return ((List<ProductTypeStandardInfo>)CacheHelper.Read(cacheKey)).ToList().Where(k => ids.Contains(k.Id)).ToList() ?? new List<ProductTypeStandardInfo>();
        }

        public static List<ProductTypeStandardInfo> ReadList(int productTypeId)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(productTypeId);
            }
            return ((List<ProductTypeStandardInfo>)CacheHelper.Read(cacheKey)).ToList().Where(k => k.ProductTypeId == productTypeId).ToList() ?? new List<ProductTypeStandardInfo>();
        }

    }
}