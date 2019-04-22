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
    public sealed class ProductTypeAttributeBLL : BaseBLL
    {
        private static readonly IProductTypeAttribute dal = FactoryHelper.Instance<IProductTypeAttribute>(Global.DataProvider, "ProductTypeAttributeDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductTypeAttribute");

        public static int Add(ProductTypeAttributeInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ProductTypeAttributeInfo entity)
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

        public static ProductTypeAttributeInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ProductTypeAttributeInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ProductTypeAttributeInfo();
        }
        public static ProductTypeAttributeInfo Read(string name, int productTypeId)
        {
            if (CacheHelper.Read(cacheKey) == null) {
                return dal.Read(name, productTypeId);
            }
            return ((List<ProductTypeAttributeInfo>)CacheHelper.Read(cacheKey)).Where(K => K.Name == name).Where(K=>K.ProductTypeId==productTypeId).FirstOrDefault() ?? new ProductTypeAttributeInfo();
        }
        public static List<ProductTypeAttributeInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductTypeAttributeInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ProductTypeAttributeInfo> ReadList(int productTypeId)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(productTypeId);
            }
            return ((List<ProductTypeAttributeInfo>)CacheHelper.Read(cacheKey)).Where(k => k.ProductTypeId == productTypeId).ToList() ?? new List<ProductTypeAttributeInfo>();
        }

        /// <summary>
        /// 合并属性和属性记录，得到完整的商品属性
        /// </summary>
        public static List<ProductTypeAttributeInfo> JoinAttribute(int productTypeId, int productId)
        {
            var attributeRecordList = ProductTypeAttributeRecordBLL.ReadList(productId);
            var attributeList = ProductTypeAttributeBLL.ReadList(productTypeId);
            List<ProductTypeAttributeInfo> result = new List<ProductTypeAttributeInfo>();
            foreach (var attribute in attributeList)
            {
                bool isFind = false;
                foreach (var attributeRecord in attributeRecordList)
                {
                    if (attribute.Id == attributeRecord.AttributeId)
                    {
                        ProductTypeAttributeInfo temp = new ProductTypeAttributeInfo();
                        temp = (ProductTypeAttributeInfo)ServerHelper.CopyClass(attribute);
                        temp.AttributeRecord = attributeRecord;
                        isFind = true;
                        result.Add(temp);
                        break;
                    }
                }
                if (!isFind)
                {
                    result.Add(attribute);
                }
            }
            return result;
        }

    }
}