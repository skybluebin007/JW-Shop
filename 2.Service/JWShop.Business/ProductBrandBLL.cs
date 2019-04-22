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
    public sealed class ProductBrandBLL : BaseBLL
    {
        private static readonly IProductBrand dal = FactoryHelper.Instance<IProductBrand>(Global.DataProvider, "ProductBrandDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductBrand");
        public static readonly int TableID = UploadTable.ReadTableID("ProductBrand");

        public static int Add(ProductBrandInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ProductBrandInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteList(string ids)
        {
            string[] idArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string id in idArr)
            {
                dal.Delete(Convert.ToInt32(id));
            }
            CacheHelper.Remove(cacheKey);
        }

        public static ProductBrandInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ProductBrandInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ProductBrandInfo();
        }
        public static ProductBrandInfo Read(string name) {
            if (CacheHelper.Read(cacheKey) == null) {
                return dal.Read(name);
            }
            return ((List<ProductBrandInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(K => K.Name == name) ?? new ProductBrandInfo();
        }
        public static List<ProductBrandInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductBrandInfo>)CacheHelper.Read(cacheKey);
        }


        public static List<ProductBrandInfo> ReadList(int productClassID)
        {
            var _pro = ProductBLL.SearchList(new ProductSearchInfo { ClassId = "|" + productClassID + "|" });
            //var _class = ProductClassBLL.Read(productClassID);
            //var _type = ProductTypeBLL.Read(_class.ProductTypeId);
            List<ProductBrandInfo> _band = new List<ProductBrandInfo>();
            //if (!string.IsNullOrEmpty(_type.BrandIds))
            //{
            //    _band = ProductBrandBLL.ReadList(Array.ConvertAll<string, int>(_type.BrandIds.Split(';'), k => Convert.ToInt32(k)));
            //}
            _band = ProductBrandBLL.ReadList(_pro.Select(k => k.BrandId).Distinct().ToArray());
            return _band;
        }


        public static List<ProductBrandInfo> ReadList(int[] ids)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(ids);
            }
            return ((List<ProductBrandInfo>)CacheHelper.Read(cacheKey)).Where(k => ids.Contains(k.Id)).ToList() ?? new List<ProductBrandInfo>();
        }
      public static List<ProductBrandInfo> SearchList(ProductBrandSearchInfo brandSearch)
        {
          return dal.SearchList(brandSearch);
        }
      public static List<ProductBrandInfo> SearchList(int currentPage, int pageSize, ProductBrandSearchInfo searchInfo, ref int count) {
          return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
      }
        public static void Move(int id, ChangeAction action)
        {
            dal.Move(id, action);
            CacheHelper.Remove(cacheKey);
        }

    }
}