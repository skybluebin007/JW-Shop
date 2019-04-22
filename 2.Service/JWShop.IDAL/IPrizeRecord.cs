using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IPrizeRecord
    {
        int Add(PrizeRecordInfo entity);
        void UpdatePrizeRecord(PrizeRecordInfo entity);
        void Delete(int[] ids);
        int GetCountByActivityId(int activityId);
        /// <summary>
        /// 获取当日（月）用户抽奖记录
        /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>抽奖记录次数int</returns>
        int GetUserPrizeCountToday(int activityId, int userId);
        /// <summary>
        /// 获取抽奖记录
        /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>抽奖记录次数int</returns>
        int GetUserPrizeCount(int activityId, int userId);
        /// <summary>
        /// 获取最新一次抽奖记录
        /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>抽奖记录次数int</returns>
        PrizeRecordInfo GetLatestUserPrizeRecord(int activityId, int userId);
        /// <summary>
        /// 获取用户本次活动的所有抽奖记录
        /// </summary>
        /// <param name="activityId">活动id</param>
        /// <param name="userId">会员id</param>
        /// <returns>所有抽奖记录列表</returns>
        List<PrizeRecordInfo> GetPrizeList(int activityId, int userId);
        bool HasSignUp(int activityId, int userId);
        List<PrizeRecordInfo> SearchList(PrizeRecordSearchInfo searchInfo);
        List<PrizeRecordInfo> SearchList(int currentPage, int pageSize, PrizeRecordSearchInfo searchInfo, ref int count);
    }
}
