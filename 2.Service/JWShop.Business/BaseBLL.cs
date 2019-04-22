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
    public  class BaseBLL
    {
        private static readonly IBase dal = FactoryHelper.Instance<IBase>(Global.DataProvider, "BaseDAL");

        public static int MaxOrderId(string table)
        {
            return dal.MaxOrderId(table);
        }

        public static void UpdatePart(string table, Dictionary<string, object> dict, int id)
        {
            dal.UpdatePart(table, dict, id);
        }

    }
}