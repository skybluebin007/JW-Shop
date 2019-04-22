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
    public sealed class ProductTypeBLL : BaseBLL
    {
        private static readonly IProductType dal = FactoryHelper.Instance<IProductType>(Global.DataProvider, "ProductTypeDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductType");

        public static int Add(ProductTypeInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ProductTypeInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            ProductTypeAttributeBLL.DeleteList(id);
            ProductTypeStandardBLL.DeleteList(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteList(string ids)
        {
            string[] idArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string id in idArr)
            {
                dal.Delete(Convert.ToInt32(id));
                ProductTypeAttributeBLL.DeleteList(Convert.ToInt32(id));
                ProductTypeStandardBLL.DeleteList(Convert.ToInt32(id));
            }
            CacheHelper.Remove(cacheKey);
        }

        public static ProductTypeInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ProductTypeInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ProductTypeInfo();
        }

        public static List<ProductTypeInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductTypeInfo>)CacheHelper.Read(cacheKey);
        }

    }
}