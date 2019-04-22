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
    /// Banner图片业务逻辑。
    /// </summary>
    public sealed class AdImageBLL : BaseBLL
    {
        private static readonly IAdImage dal = FactoryHelper.Instance<IAdImage>(Global.DataProvider, "AdImageDAL");
        public static readonly int TableID = UploadTable.ReadTableID("AdImage");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("AdImage");

        public static int Add(AdImageInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(AdImageInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }
        public static void DeleteByAdType(int adType) {
            dal.DeleteByAdType(adType);
            CacheHelper.Remove(cacheKey);
        }
        public static AdImageInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<AdImageInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new AdImageInfo();
        }

        public static List<AdImageInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<AdImageInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<AdImageInfo> ReadList(int adType)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.ReadList(adType);
            }
            return ((List<AdImageInfo>)CacheHelper.Read(cacheKey)).Where(k => k.AdType == adType).ToList();
        }       

        public static List<AdImageInfo> ReadList(int adType,int count)
        {
            List<AdImageInfo> adList = new List<AdImageInfo>();
            if (CacheHelper.Read(cacheKey) == null)
            {
                adList = (List<AdImageInfo>)dal.ReadList(adType).Take(count).ToList();
            }
            else
            {
                adList = ((List<AdImageInfo>)CacheHelper.Read(cacheKey)).Where(k => k.AdType == adType).Take(count).ToList();
            }
            return adList;
        }
        public static List<AdImageInfo> ReadList(int adType,int classID,string showType, int count)
        {
            List<AdImageInfo> adList = new List<AdImageInfo>();
            if (CacheHelper.Read(cacheKey) == null)
            {
                adList = (List<AdImageInfo>)dal.ReadList(adType).Where(k => k.SubTitle == showType && k.ClassId == classID).Take(count).ToList();
            }
            else
            {
                adList = ((List<AdImageInfo>)CacheHelper.Read(cacheKey)).Where(k => k.AdType == adType && k.ClassId == classID && k.SubTitle == showType).Take(count).ToList();
            }
            return adList;
        }
    }
}