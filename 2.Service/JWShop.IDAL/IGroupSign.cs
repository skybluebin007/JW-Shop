using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.IDAL
{
   public interface IGroupSign
    {
        int Add(GroupSignInfo entity);
        GroupSignInfo Read(int id);
        /// <summary>
        /// 根据拼团活动id获取参团记录
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        List<GroupSignInfo> ReadListByGroupId(int groupId);
        List<GroupSignInfo> SearchListByGroupId(int groupId, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 根据userId获取参团记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<GroupSignInfo> ReadListByUserId(int userId);
        List<GroupSignInfo> SearchListByUserId(int userId, int pageIndex, int pageSize, ref int count);
    }
}
