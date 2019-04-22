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
    public sealed class PointProductOrderBLL : BaseBLL
    {
        private static readonly IPointProductOrder dal = FactoryHelper.Instance<IPointProductOrder>(Global.DataProvider, "PointProductOrderDAL");

        public static int Add(PointProductOrderInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(PointProductOrderInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static PointProductOrderInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<PointProductOrderInfo> ReadList(int userId)
        {
            return dal.ReadList(userId);
        }

        public static List<PointProductOrderInfo> SearchList(int currentPage, int pageSize, PointProductOrderSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
    }
}