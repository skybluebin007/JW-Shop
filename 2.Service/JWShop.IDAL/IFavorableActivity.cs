using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IFavorableActivity
    {
        int Add(FavorableActivityInfo entity);
        void Update(FavorableActivityInfo entity);
        void Delete(int[] ids);
        FavorableActivityInfo Read(int id);
        FavorableActivityInfo Read(DateTime startDate, DateTime endDate, int id = 0);
        List<FavorableActivityInfo> ReadList(DateTime startDate, DateTime endDate);
        List<FavorableActivityInfo> ReadList(int currentPage, int pageSize, ref int count);
        /// <summary>
        /// 根据有效期状态查询
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="timePeriod">状态：未开始 1，进行中 2，已结束 3</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<FavorableActivityInfo> ReadList(int currentPage, int pageSize, ref int count, int timePeriod = 2);
    }
}