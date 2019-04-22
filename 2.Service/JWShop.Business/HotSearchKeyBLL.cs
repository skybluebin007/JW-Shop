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
     public sealed class HotSearchKeyBLL
    {
         private static readonly IHotSearchKey dal = FactoryHelper.Instance<IHotSearchKey>(Global.DataProvider, "HotSearchKeyDAL");
         private static readonly string cacheKey = CacheKey.ReadCacheKey("HotSearchKey");
         public static int Add(HotSearchKeyInfo entity)
         {
             entity.Id = dal.Add(entity);
             CacheHelper.Remove(cacheKey);
             return entity.Id;
         }

         public static void Update(HotSearchKeyInfo entity)
         {
             dal.Update(entity);
             CacheHelper.Remove(cacheKey);
         }

         public static void Delete(int id)
         {
             dal.Delete(id);
             CacheHelper.Remove(cacheKey);
         }

         public static HotSearchKeyInfo Read(int id)
         {
             if (CacheHelper.Read(cacheKey) == null)
             {
                 return dal.Read(id);
             }
             return ((List<HotSearchKeyInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new HotSearchKeyInfo();
         }
         public static HotSearchKeyInfo Read(string name)
         {
             if (CacheHelper.Read(cacheKey) == null)
             {
                 return dal.Read(name);
             }
             return ((List<HotSearchKeyInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Name == name) ?? new HotSearchKeyInfo();
         }
         public static List<HotSearchKeyInfo> ReadList()
         {
             if (CacheHelper.Read(cacheKey) == null)
             {
                 CacheHelper.Write(cacheKey, dal.ReadList());
             }
             return (List<HotSearchKeyInfo>)CacheHelper.Read(cacheKey);
         }
         public static List<HotSearchKeyInfo> ReadList(string name)
         {
             if (CacheHelper.Read(cacheKey) == null)
             {
                 CacheHelper.Write(cacheKey, dal.ReadList());
             }
             return ((List<HotSearchKeyInfo>)CacheHelper.Read(cacheKey)).Where(hk=>hk.Name.IndexOf(name)>-1).OrderByDescending(hk=>hk.SearchTimes).OrderBy(hk=>hk.Id).ToList();
         }
    }
}
