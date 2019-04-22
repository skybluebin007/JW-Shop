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
    /// 管理组业务逻辑。
    /// </summary>
    public sealed class AdminGroupBLL : BaseBLL
    {
        private static readonly IAdminGroup dal = FactoryHelper.Instance<IAdminGroup>(Global.DataProvider, "AdminGroupDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("AdminGroup");

        public static int Add(AdminGroupInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(AdminGroupInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int[] ids)
        {
            AdminBLL.DeleteByGroupIds(ids);
            dal.Delete(ids);
            CacheHelper.Remove(cacheKey);
        }

        public static AdminGroupInfo Read(int id)
        {
            return ReadList().FirstOrDefault(k => k.Id == id) ?? new AdminGroupInfo();
        }

        public static List<AdminGroupInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<AdminGroupInfo>)CacheHelper.Read(cacheKey);
        }

        public static void ChangeCount(int id, ChangeAction action)
        {
            dal.ChangeCount(id, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeCountByGeneral(int[] adminIds, ChangeAction action)
        {
            dal.ChangeCountByGeneral(adminIds, action);
            CacheHelper.Remove(cacheKey);
        }

    }
}