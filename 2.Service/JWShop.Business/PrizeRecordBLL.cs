using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;


namespace JWShop.Business
{
  public sealed  class PrizeRecordBLL:BaseBLL
    {
      private static readonly IPrizeRecord dal = FactoryHelper.Instance<IPrizeRecord>(Global.DataProvider, "PrizeRecordDAL");
      public static int Add(PrizeRecordInfo entity) {
          return dal.Add(entity);
      }
      public static void UpdatePrizeRecord(PrizeRecordInfo entity) {
          dal.UpdatePrizeRecord(entity);
      }
      public static void Delete(int[] ids) {
          dal.Delete(ids);
      }
      public static int GetCountByActivityId(int activityId) {
          return dal.GetCountByActivityId(activityId);
      }
      /// <summary>
      /// 获取当日（月）用户抽奖记录
      /// </summary>
      /// <param name="activityId">活动id</param>
      /// <param name="userId">会员id</param>
      /// <returns>抽奖记录次数int</returns>
      public static int GetUserPrizeCountToday(int activityId, int userId) {
          return dal.GetUserPrizeCountToday(activityId, userId);
      }
      /// <summary>
      /// 获取抽奖记录
      /// </summary>
      /// <param name="activityId">活动id</param>
      /// <param name="userId">会员id</param>
      /// <returns>抽奖记录次数int</returns>
      public static int GetUserPrizeCount(int activityId, int userId) {
          return dal.GetUserPrizeCount(activityId, userId);
      }
      /// <summary>
      /// 获取最新一次抽奖记录
      /// </summary>
      /// <param name="activityId">活动id</param>
      /// <param name="userId">会员id</param>
      /// <returns>抽奖记录次数int</returns>
      public static PrizeRecordInfo GetLatestUserPrizeRecord(int activityId, int userId) {
          return dal.GetLatestUserPrizeRecord(activityId, userId);
      }
      /// <summary>
      /// 获取用户本次活动的所有抽奖记录
      /// </summary>
      /// <param name="activityId">活动id</param>
      /// <param name="userId">会员id</param>
      /// <returns>所有抽奖记录列表</returns>
      public static List<PrizeRecordInfo> GetPrizeList(int activityId, int userId) {
          return dal.GetPrizeList(activityId, userId);
      }
      public static bool HasSignUp(int activityId, int userId) {
          return dal.HasSignUp(activityId, userId);
      }
      public static List<PrizeRecordInfo> SearchList(PrizeRecordSearchInfo searchInfo) {
          return dal.SearchList(searchInfo);
      }
      public static List<PrizeRecordInfo> SearchList(int currentPage, int pageSize, PrizeRecordSearchInfo searchInfo, ref int count) { 
      return dal.SearchList(currentPage,pageSize,searchInfo,ref count);
      }
    }
}
