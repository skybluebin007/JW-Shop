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
    public sealed class UserAddressBLL : BaseBLL
    {
        private static readonly IUserAddress dal = FactoryHelper.Instance<IUserAddress>(Global.DataProvider, "UserAddressDAL");

        public static int Add(UserAddressInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(UserAddressInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id, int userId)
        {
            dal.Delete(id, userId);
        }

        public static void Delete(string ids, int userId)
        {
            int[] idArr = Array.ConvertAll<string, int>(ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), i => Convert.ToInt32(i));
            foreach (int d in idArr)
            {
                dal.Delete(d, userId);
            }
        }

        public static UserAddressInfo Read(int id, int userId)
        {
            return dal.Read(id, userId);
        }

        public static List<UserAddressInfo> ReadList(int userId)
        {
            return dal.ReadList(userId);
        }

        public static void SetDefault(int id, int userId)
        {
            dal.SetDefault(id, userId);
        }

    }
}