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
    public sealed class UserRechargeBLL : BaseBLL
    {
        private static readonly IUserRecharge dal = FactoryHelper.Instance<IUserRecharge>(Global.DataProvider, "UserRechargeDAL");

        public static int Add(UserRechargeInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(UserRechargeInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id, int userId)
        {
            dal.Delete(id, userId);
        }

        public static UserRechargeInfo Read(int id, int userId)
        {
            return dal.Read(id, userId);
        }

        public static UserRechargeInfo Read(string number, int userId)
        {
            return dal.Read(number, userId);
        }
        public static UserRechargeInfo Read(string number) {
            return dal.Read(number);
        }
        public static List<UserRechargeInfo> ReadList(int userId)
        {
            return dal.ReadList(userId);
        }

        public static List<UserRechargeInfo> SearchList(int currentPage, int pageSize, UserRechargeSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }

    }
}