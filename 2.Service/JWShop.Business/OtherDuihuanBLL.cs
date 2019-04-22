using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 业务逻辑。
    /// </summary>
    public sealed class OtherDuihuanBLL : BaseBLL
    {
        private static readonly IOtherDuihuan dal = FactoryHelper.Instance<IOtherDuihuan>(Global.DataProvider, "OtherDuihuanDAL");

        public static int Add(OtherDuihuanInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(OtherDuihuanInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static OtherDuihuanInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static OtherDuihuanInfo Read_User(int userid)
        {
            return dal.Read_User(userid);
        }

        public static List<OtherDuihuanInfo> ReadList(int userid)
        {
            return dal.ReadList(userid);
        }

        public static List<OtherDuihuanInfo> SearchList(OtherDuihuanSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<OtherDuihuanInfo> SearchList(int currentPage, int pageSize, OtherDuihuanSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
    }
}
