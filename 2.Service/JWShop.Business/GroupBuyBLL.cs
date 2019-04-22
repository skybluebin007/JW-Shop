using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using JWShop.Common;

namespace JWShop.Business
{/// <summary>
/// 拼团 业务层
/// </summary>
  public sealed  class GroupBuyBLL:BaseBLL
    {
        private static readonly IGroupBuy dal = FactoryHelper.Instance<IGroupBuy>(Global.DataProvider, "GroupBuyDAL");

        public static int Add(GroupBuyInfo entity)
        {
            return dal.Add(entity);
        }
        public static GroupBuyInfo Read(int id)
        {
            return dal.Read(id);
        }
        /// <summary>
        /// 增加参团人数（有人参团,+1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int PlusSignCount(int id)
        {
            return dal.PlusSignCount(id);
        }
        /// <summary>
        /// 减少参团人数（-1）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int MinusSignCount(int id)
        {
            return dal.MinusSignCount(id);
        }
        #region search

        public static List<GroupBuyInfo> ReadList(int[] ids)
        {
            return dal.ReadList(ids);
        }
        public static List<GroupBuyInfo> SearchList(GroupBuySearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }
        public static List<GroupBuyInfo> SearchList(int currentPage, int pageSize, GroupBuySearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
     
        #endregion
    }
}
