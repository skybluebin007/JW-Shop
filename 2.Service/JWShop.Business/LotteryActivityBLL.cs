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
   public sealed class LotteryActivityBLL:BaseBLL
    {
       private static readonly ILotteryActivity dal = FactoryHelper.Instance<ILotteryActivity>(Global.DataProvider, "LotteryActivityDAL");
       public static readonly int TableID = UploadTable.ReadTableID("LotteryActivity");
       private static readonly string cacheKey = CacheKey.ReadCacheKey("LotteryActivityList");

        public static int Add(LotteryActivityInfo entity)
        {
            entity.Id = dal.Add(entity);
            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(LotteryActivityInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));
        }

        public static void Delete(int[] ids)
        {
            UploadBLL.DeleteUploadByRecordID(TableID, string.Join(",", ids));
            dal.Delete(ids);
            CacheHelper.Remove(cacheKey);
        }

        public static LotteryActivityInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<LotteryActivityInfo> SearchList(int currentPage, int pageSize, LotteryActivitySearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }


        public static List<LotteryActivityInfo> SearchList(LotteryActivitySearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }
       /// <summary>
        /// 检查key唯一性
        /// </summary>
        /// <param name="activityKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UniqueKey(string activityKey, int id = 0)
        {
            return dal.UniqueKey(activityKey, id);
        }
   
    }
}
