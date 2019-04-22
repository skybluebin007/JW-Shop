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
    public sealed class NavMenuBLL : BaseBLL
    {
        private static readonly INavMenu dal = FactoryHelper.Instance<INavMenu>(Global.DataProvider, "NavMenuDAL");

        public static int Add(NavMenuInfo entity)
        {
            entity.Id = dal.Add(entity);
            return entity.Id;
        }

        public static void Update(NavMenuInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static NavMenuInfo Read(int id)
        {
            return dal.Read(id);
        }
         /// <summary>
        /// 修改导航排序
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void ChangeNavMenuOrder(int id, int orderId) {
            dal.ChangeNavMenuOrder(id, orderId);
        }
        public static List<NavMenuInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<NavMenuInfo> ReadList(bool isShow)
        {
            return dal.ReadList(isShow);
        }
    
    }
}
