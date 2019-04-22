using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.IDAL;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.Common;

namespace JWShop.Business
{
    /// <summary>
    /// 分销商等级 业务层
    /// </summary>
   public sealed class DistributorGradeBLL:BaseBLL
    {
        private static readonly IDistributorGrade dal = FactoryHelper.Instance<IDistributorGrade>(Global.DataProvider, "DistributorGradeDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("DistributorGrade");

        public static int Add(DistributorGradeInfo entity)
        {
            CacheHelper.Remove(cacheKey);
            return dal.Add(entity);
          
        }
        public static void Update(DistributorGradeInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }
        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }
        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
            CacheHelper.Remove(cacheKey);
        }
        public static DistributorGradeInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<DistributorGradeInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new DistributorGradeInfo();
        }
        public static List<DistributorGradeInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return CacheHelper.Read(cacheKey) as List<DistributorGradeInfo>;
        }
    }
}
