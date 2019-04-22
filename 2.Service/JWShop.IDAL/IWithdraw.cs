using JWShop.Entity;
using System.Collections.Generic;

namespace JWShop.IDAL
{
    /// <summary>
    /// 提现记录 接口层
    /// </summary>
   public interface IWithdraw
    {
        int Add(WithdrawInfo entity);
        WithdrawInfo Read(int id);
        void Delete(int id);
        bool Operate(int id, Withdraw_Operate operate,int operator_Id, string note);
        List<WithdrawInfo> SearchList(WithdrawSearchInfo searchModel);
        List<WithdrawInfo> SearchList(int currentPage,int pageSize, WithdrawSearchInfo searchModel,ref int count);
        /// <summary>
        /// 统计分销商提现总额
        /// </summary>       
        decimal GetSumWithdraw(int distributor_Id);
    }
}
