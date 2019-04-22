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
    public sealed class ProductTypeStandardRecordBLL : BaseBLL
    {
        private static readonly IProductTypeStandardRecord dal = FactoryHelper.Instance<IProductTypeStandardRecord>(Global.DataProvider, "ProductTypeStandardRecordDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductTypeStandardRecord");

        public static void Add(ProductTypeStandardRecordInfo entity)
        {
            dal.Add(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Update(ProductTypeStandardRecordInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }
        public static void UpdateSalePrice(ProductTypeStandardRecordInfo entity) {
            dal.UpdateSalePrice(entity);
            CacheHelper.Remove(cacheKey);
        }
        public static void UpdateStorage(ProductTypeStandardRecordInfo entity) {
            dal.UpdateStorage(entity);
            CacheHelper.Remove(cacheKey);
        }
        public static void Delete(int productId)
        {
            dal.Delete(productId);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteByStandardId(int standardId)
        {
            dal.DeleteByStandardId(standardId);
            CacheHelper.Remove(cacheKey);
        }

        public static ProductTypeStandardRecordInfo Read(int productId, string valueList)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(productId, valueList);
            }
            return ((List<ProductTypeStandardRecordInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.ProductId == productId && k.ValueList == valueList) ?? new ProductTypeStandardRecordInfo();
        }

        public static List<ProductTypeStandardRecordInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductTypeStandardRecordInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ProductTypeStandardRecordInfo> ReadList(int productId)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(productId);
            }
            return ((List<ProductTypeStandardRecordInfo>)CacheHelper.Read(cacheKey)).ToList().Where(k => k.ProductId == productId).ToList() ?? new List<ProductTypeStandardRecordInfo>();
        }

        /// <summary>
        /// 按产品删除规格记录数据
        /// </summary>
        /// <param name="strProductID">产品ID,以,号分隔</param>
        public static void DeleteByProductID(string strProductID)
        {
            dal.DeleteByProductID(strProductID);
        } 
        /// <summary>
        /// 按产品ID获得规格记录所有数据
        /// </summary>
        /// <param name="productID">产品ID</param>
        /// <returns>规格记录数据列表</returns>
        public static List<ProductTypeStandardRecordInfo> ReadListByProduct(int productID, int standardType)
        {
            return dal.ReadListByProduct(productID, standardType);
        }

        public static void ChangeOrderCount(int productId, string valueList, int buyCount, ChangeAction action)
        {
            dal.ChangeOrderCount(productId, valueList, buyCount, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeSendCount(int productId, string valueList, int buyCount, ChangeAction action)
        {
            dal.ChangeSendCount(productId, valueList, buyCount, action);
            CacheHelper.Remove(cacheKey);
        }
         /// <summary>
        /// 获取规格总库存
        /// </summary>
        /// <param name="entity"></param>
        public static int GetSumStorageByProduct(int productId) {
            return dal.GetSumStorageByProduct(productId);
        }
    }
}