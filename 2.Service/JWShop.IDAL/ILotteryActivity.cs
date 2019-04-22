using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface ILotteryActivity
    {
        int Add(LotteryActivityInfo entity);
        void Update(LotteryActivityInfo entity);
        void Delete(int[] ids);
        LotteryActivityInfo Read(int id);

        List<LotteryActivityInfo> SearchList(LotteryActivitySearchInfo searchInfo);
        List<LotteryActivityInfo> SearchList(int currentPage, int pageSize, LotteryActivitySearchInfo searchInfo, ref int count);
        /// <summary>
        /// 检查key唯一性
        /// </summary>
        /// <param name="activityKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UniqueKey(string activityKey, int id = 0);
    }
}
