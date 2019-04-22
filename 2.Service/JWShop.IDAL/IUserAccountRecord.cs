using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IUserAccountRecord
    {
        int Add(UserAccountRecordInfo entity);
        List<UserAccountRecordInfo> ReadList(int userId);
        List<UserAccountRecordInfo> ReadList(int currentPage, int pageSize, AccountRecordType accountType, int userId, ref int count);
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
        List<UserAccountRecordInfo> ReadList(int currentPage, int pageSize, AccountRecordType accountType, int userId, int inCome, ref int count);
        int SumPoint(int userId);
        bool HasGiftForLogin(int userId, string note);
        /// <summary>
        /// 在指定的id前剩余的资金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        decimal ReadMoneyLeftBeforID(int id, int userID);
        /// <summary>
        /// 在指定的id前剩余的积分
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        int ReadPointLeftBeforID(int id, int userID);
    }
}