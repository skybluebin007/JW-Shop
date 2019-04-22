using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    public sealed class FavorableActivityBLL : BaseBLL
    {
        private static readonly IFavorableActivity dal = FactoryHelper.Instance<IFavorableActivity>(Global.DataProvider, "FavorableActivityDAL");
        public static readonly int TableID = UploadTable.ReadTableID("FavorableActivity");
        
        public static int Add(FavorableActivityInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(FavorableActivityInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static FavorableActivityInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static FavorableActivityInfo Read(DateTime startDate, DateTime endDate, int id = 0)
        {
            return dal.Read(startDate, endDate, id);
        }
        public static List<FavorableActivityInfo> ReadList(DateTime startDate, DateTime endDate) {
            return dal.ReadList(startDate, endDate);
        }
        public static List<FavorableActivityInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, ref count);
        }
        /// <summary>
        /// 根据有效期状态查询
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="timePeriod">状态：未开始 1，进行中 2，已结束 3</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<FavorableActivityInfo> ReadList(int currentPage, int pageSize, ref int count, int timePeriod = 2)
        {
            return dal.ReadList(currentPage, pageSize, ref count, timePeriod);
        }
        /// <summary>
        /// 获取优惠活动类型名称
        /// </summary>
        /// <param name="type">类型ID</param>
        /// <returns></returns>
        public static string FavorableTypeName(int type)
        {
            string result = "订单优惠";
            switch(type){
                case (int)FavorableType.AllOrders:
                    result = "订单优惠";
                    break;
                case (int)FavorableType.ProductClass:
                    result = "商品分类优惠";
                    break;
                default:
                     result = "订单优惠";
                    break;
            }
            return result;
        }
    }
}