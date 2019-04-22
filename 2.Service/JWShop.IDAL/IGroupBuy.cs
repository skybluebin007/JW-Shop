using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 拼团 接口层
    /// </summary>
   public interface IGroupBuy
    {
        int Add(GroupBuyInfo entity);
        GroupBuyInfo Read(int id);
        /// <summary>
        /// 增加参团人数（有人参团,+1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int PlusSignCount(int id);
        /// <summary>
        /// 减少参团人数（-1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MinusSignCount(int id);
        List<GroupBuyInfo> ReadList(int[] ids);
        List<GroupBuyInfo> SearchList(GroupBuySearchInfo searchInfo);
        List<GroupBuyInfo> SearchList(int currentPage, int pageSize, GroupBuySearchInfo searchInfo, ref int count);
    }
}
