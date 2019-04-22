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
using System.Linq;

namespace JWShop.Business
{
    public sealed class ThemeActivityBLL : BaseBLL
    {
        private static readonly IThemeActivity dal = FactoryHelper.Instance<IThemeActivity>(Global.DataProvider, "ThemeActivityDAL");
        public static readonly int TableID = UploadTable.ReadTableID("ThemeActivity");

        public static int Add(ThemeActivityInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(ThemeActivityInfo entity)
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

        public static ThemeActivityInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<ThemeActivityInfo> ReadList()
        {
            return dal.ReadList();
        }

    }
}