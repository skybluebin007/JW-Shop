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
    public sealed class UserAccountRecordBLL : BaseBLL
    {
        private static readonly IUserAccountRecord dal = FactoryHelper.Instance<IUserAccountRecord>(Global.DataProvider, "UserAccountRecordDAL");

        public static int Add(UserAccountRecordInfo entity)
        {
            return dal.Add(entity);
        }

        public static List<UserAccountRecordInfo> ReadList(int userId)
        {
            return dal.ReadList(userId);
        }

        public static List<UserAccountRecordInfo> ReadList(int currentPage, int pageSize, AccountRecordType accountType, int userId, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, accountType, userId, ref count);
        }
        /// <summary>
        /// 积分明细（区分收入、支出）
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="accountType"></param>
        /// <param name="userId"></param>
        /// <param name="inCome">1-收入，0-支出</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<UserAccountRecordInfo> ReadList(int currentPage, int pageSize, AccountRecordType accountType, int userId, int inCome, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, accountType, userId, inCome, ref count);
        }
        public static int SumPoint(int userId)
        {
            return dal.SumPoint(userId);
        }

        public static bool HasGiftForLogin(int userId, string note)
        {
            return dal.HasGiftForLogin(userId, note);
        }

        /// <summary>
        /// 在指定的id前剩余的资金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static decimal ReadMoneyLeftBeforID(int id, int userID)
        {
            return dal.ReadMoneyLeftBeforID(id, userID);
        }
        /// <summary>
        /// 在指定的id前剩余的积分
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int ReadPointLeftBeforID(int id, int userID)
        {
            return dal.ReadPointLeftBeforID(id, userID);
        }
    }
}