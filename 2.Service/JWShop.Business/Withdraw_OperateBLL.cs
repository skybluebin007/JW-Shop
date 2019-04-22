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
    /// 提现申请 审核操作 业务层
    /// </summary>
    public sealed class Withdraw_OperateBLL : BaseBLL
    {
        private static readonly IWithdraw_Operate dal = FactoryHelper.Instance<IWithdraw_Operate>(Global.DataProvider, "Withdraw_OperateDAL");
        /// <summary>
        /// 根据提现申请Id获取相关操作记录
        /// </summary>
        /// <param name="withdraw_Id"></param>
        /// <returns></returns>
        public static List<WithdrawOperateInfo> ReadListByWithdraw(int withdraw_Id)
        {
            return dal.ReadListByWithdraw(withdraw_Id);
        }
    }
}
