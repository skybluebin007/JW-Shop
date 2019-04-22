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
    /// <summary>
    /// 广告位业务逻辑。
    /// </summary>
    public sealed class FlashBLL : BaseBLL
    {
        private static readonly IFlash dal = FactoryHelper.Instance<IFlash>(Global.DataProvider, "FlashDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Flash");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("Flash");
        public static int Add(FlashInfo entity)
        {
            entity.Id = dal.Add(entity);
            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }
        public static void Update(FlashInfo entity)
        {
            CacheHelper.Remove(cacheKey);
            dal.Update(entity);
        }

        public static void Delete(int[] ids)
        {
            CacheHelper.Remove(cacheKey);
            dal.Delete(ids);
        }

        public static void Delete(int id)
        {
            CacheHelper.Remove(cacheKey);
            dal.Delete(id);
        }
        public static FlashInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<FlashInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new FlashInfo();
        }
        public static List<FlashInfo> SearchList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.SearchList());
            }
            return (List<FlashInfo>)CacheHelper.Read(cacheKey);
        }
        public static List<FlashInfo> SearchList(int currentPage, int pageSize, ref int count)
        { return dal.SearchList(currentPage, pageSize, ref count); }
    }
}
