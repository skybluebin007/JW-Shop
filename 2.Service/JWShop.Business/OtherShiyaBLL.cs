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
    public sealed class OtherShiyaBLL : BaseBLL
    {
        private static readonly IOtherShiya dal = FactoryHelper.Instance<IOtherShiya>(Global.DataProvider, "OtherShiyaDAL");

        public static int Add(OtherShiyaInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(OtherShiyaInfo entity)
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

        public static OtherShiyaInfo Read(int id)
        {
            return dal.Read(id);
        }
        
        public static List<OtherShiyaInfo> ReadList(int userid,int usertype)
        {
            return dal.ReadList(userid, usertype);
        }

        public static List<OtherShiyaInfo> SearchList(OtherShiyaSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<OtherShiyaInfo> SearchList(int currentPage, int pageSize, OtherShiyaSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
    }
}
