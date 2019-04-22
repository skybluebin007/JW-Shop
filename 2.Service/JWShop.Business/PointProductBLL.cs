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
    public sealed class PointProductBLL : BaseBLL
    {
        private static readonly IPointProduct dal = FactoryHelper.Instance<IPointProduct>(Global.DataProvider, "PointProductDAL");

        public static int Add(PointProductInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(PointProductInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static PointProductInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<PointProductInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<PointProductInfo> ReadList(int[] ids)
        {
            return dal.ReadList(ids);
        }

        public static List<PointProductInfo> SearchList(int currentPage, int pageSize, PointProductSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }

        public static void OffSale(int[] ids)
        {
            dal.OffSale(ids);
        }

        public static void OnSale(int[] ids)
        {
            dal.OnSale(ids);
        }

        public static void ChangeSendCount(int id, ChangeAction action)
        {
            dal.ChangeSendCount(id, action);
        }
    }
}