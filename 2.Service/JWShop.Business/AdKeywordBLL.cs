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
    /// <summary>
    /// 热门搜索关键词业务逻辑。
    /// </summary>
    public sealed class AdKeywordBLL : BaseBLL
    {
        private static readonly IAdKeyword dal = FactoryHelper.Instance<IAdKeyword>(Global.DataProvider, "AdKeywordDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("AdKeyword");

        public static int Add(AdKeywordInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(AdKeywordInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }

        public static AdKeywordInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<AdKeywordInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new AdKeywordInfo();
        }

        public static List<AdKeywordInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<AdKeywordInfo>)CacheHelper.Read(cacheKey);
        }
    }
}