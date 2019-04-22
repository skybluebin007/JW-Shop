using System;
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
   public sealed class PhotoSizeBLL : BaseBLL
    {
       private static readonly IPhotoSize dal = FactoryHelper.Instance<IPhotoSize>(Global.DataProvider, "PhotoSizeDaL");
       private static readonly string cacheKey = CacheKey.ReadCacheKey("PhotoSize");
       public static int Add(PhotoSizeInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }
       public static void Update(PhotoSizeInfo entity)
        {
            CacheHelper.Remove(cacheKey);
           dal.Update(entity); }

        public static void Delete(int[] ids)
        {
            CacheHelper.Remove(cacheKey);
            dal.Delete(ids); }

        public static void Delete(int id)
        {
            CacheHelper.Remove(cacheKey);
            dal.Delete(id); }
        public static PhotoSizeInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<PhotoSizeInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(a => a.Id == id) ?? new PhotoSizeInfo();
        }
        public static List<PhotoSizeInfo> SearchList(int type)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.SearchList(type);
            }
            return ((List<PhotoSizeInfo>)CacheHelper.Read(cacheKey)).Where(a => a.Type == type).ToList() ?? new List<PhotoSizeInfo>();
        }
        public List<PhotoSizeInfo> SearchAllList() {
            if (CacheHelper.Read(cacheKey) == null)
            {
              CacheHelper.Write(cacheKey,  dal.SearchAllList());
            }
            return (List<PhotoSizeInfo>)CacheHelper.Read(cacheKey);
        }
        public static List<PhotoSizeInfo> SearchList(int type,int currentPage, int pageSize, ref int count)
        { return dal.SearchList(type,currentPage, pageSize, ref count); }

       public static string ReadPhotoType(int type){
           string result = string.Empty;
           switch (type) { 
               case (int)PhotoType.Article:
                   result = "文章封面图"; 
                   break;
               case (int)PhotoType.Product:
                   result = "产品封面图";
                   break;
               case (int)PhotoType.ProductPhoto:
                   result = "产品图集";
                   break;
           }
           return result;
       
       }
    }
}
