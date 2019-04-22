using System;
using System.Collections.Generic;
using System.Linq;
using SkyCES.EntLib;
using JWShop.IDAL;
using JWShop.Entity;
using JWShop.Common;

namespace JWShop.Business
{
    /// <summary>
    /// 提现记录 业务层
    /// </summary>
    public sealed class WithdrawBLL : BaseBLL
    {
        private static readonly IWithdraw dal = FactoryHelper.Instance<IWithdraw>(Global.DataProvider, "WithdrawDAL");
        public static int Add(WithdrawInfo entity)
        {
            return dal.Add(entity);
        }
        public static WithdrawInfo Read(int id)
        {
            return dal.Read(id);
        }
        //void Delete(int id);
        /// <summary>
        /// 管理员审核提现申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operate">审核操作</param>
        /// <param name="operator_Id">管理员Id</param>
        public static bool Operate(int id, Withdraw_Operate operate, int operator_Id, string note)
        {
           return dal.Operate(id, operate, operator_Id,note);
        }
        public static List<WithdrawInfo> SearchList(WithdrawSearchInfo searchModel)
        {
            return dal.SearchList(searchModel);
        }
        public static List<WithdrawInfo> SearchList(int currentPage, int pageSize, WithdrawSearchInfo searchModel, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchModel, ref count);
        }
        /// <summary>
        /// 统计分销商提现总额
        /// </summary>       
        public static decimal GetSumWithdraw(int distributor_Id)
        {
            return dal.GetSumWithdraw(distributor_Id);
        }
    }
}
