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
    public sealed class FavorableActivityGiftBLL : BaseBLL
    {
        private static readonly IFavorableActivityGift dal = FactoryHelper.Instance<IFavorableActivityGift>(Global.DataProvider, "FavorableActivityGiftDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Gift");

        public static int Add(FavorableActivityGiftInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(FavorableActivityGiftInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static FavorableActivityGiftInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<FavorableActivityGiftInfo> SearchList(FavorableActivityGiftSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<FavorableActivityGiftInfo> SearchList(int currentPage, int pageSize, FavorableActivityGiftSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
    }
}