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
    /// <summary>
    /// 网站导航业务逻辑。
    /// </summary>
    public sealed class NavigationBLL
    {
        private static readonly INavigation dal = FactoryHelper.Instance<INavigation>(Global.DataProvider, "NavigationDAL");

        public static void Add(NavigationInfo entity)
        {
            dal.Add(entity);
        }

        public static void Update(NavigationInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static NavigationInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static NavigationInfo Read(NavigationClassType classType, int classId)
        {
            return dal.Read(classType, classId);
        }

        public static List<NavigationInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<NavigationInfo> ReadList(int navigationType)
        {
            return dal.ReadList(navigationType);
        }

        public static List<NavigationInfo> ReadFatherList(int navigationType)
        {
            var entites = dal.ReadList(navigationType);
            return entites.Where(k => k.ParentId == 0).ToList();
        }

        public static List<NavigationInfo> ReadChildList(int classId)
        {
            return dal.ReadChildList(classId);
        }

    }
}