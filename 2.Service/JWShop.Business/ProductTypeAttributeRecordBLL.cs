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
    public sealed class ProductTypeAttributeRecordBLL : BaseBLL
    {
        private static readonly IProductTypeAttributeRecord dal = FactoryHelper.Instance<IProductTypeAttributeRecord>(Global.DataProvider, "ProductTypeAttributeRecordDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductTypeAttributeRecord");

        public static void Add(ProductTypeAttributeRecordInfo entity)
        {
            dal.Add(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Update(ProductTypeAttributeRecordInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int productId)
        {
            dal.Delete(productId);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteByAttr(int attrId)
        {
            dal.DeleteByAttr(attrId);
            CacheHelper.Remove(cacheKey);
        }        

        public static List<ProductTypeAttributeRecordInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductTypeAttributeRecordInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ProductTypeAttributeRecordInfo> ReadList(int productId)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(productId);
            }
            return ((List<ProductTypeAttributeRecordInfo>)CacheHelper.Read(cacheKey)).ToList().Where(k => k.ProductId == productId).ToList() ?? new List<ProductTypeAttributeRecordInfo>();
        }

        public static List<ProductTypeAttributeRecordInfo> ReadListByAttribute(int attributeId)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList();
            }
            return (List<ProductTypeAttributeRecordInfo>)dal.ReadList().Where(k => k.AttributeId == attributeId).ToList() ?? new List<ProductTypeAttributeRecordInfo>();
        }

    }
}