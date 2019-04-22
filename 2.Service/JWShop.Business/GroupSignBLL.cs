using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;
using JWShop.Common;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 参团 业务层
    /// </summary>
   public sealed class GroupSignBLL
    {
        private static readonly IGroupSign dal = FactoryHelper.Instance<IGroupSign>(Global.DataProvider, "GroupSignDAL");

        public static int Add(GroupSignInfo entity)
        {
            return dal.Add(entity);
        }
        public static GroupSignInfo Read(int id)
        {
            return dal.Read(id);
        }
        /// <summary>
        /// 根据拼团活动id获取参团记录
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static List<GroupSignInfo> ReadListByGroupId(int groupId)
        {
            return dal.ReadListByGroupId(groupId);
        }
        public static List<GroupSignInfo> SearchListByGroupId(int groupId, int pageIndex, int pageSize, ref int count)
        {
            return dal.SearchListByGroupId(groupId, pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 根据userId获取参团记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<GroupSignInfo> ReadListByUserId(int userId)
        {
            return dal.ReadListByUserId(userId);
        }

        public static List<GroupSignInfo> SearchListByUserId(int userId, int pageIndex, int pageSize, ref int count)
        {
            return dal.SearchListByUserId(userId,pageIndex,pageSize,ref count);
        }
    }
}
