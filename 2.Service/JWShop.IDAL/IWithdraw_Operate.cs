using JWShop.Entity;
using System.Collections.Generic;

namespace JWShop.IDAL
{
    /// <summary>
    /// 审核提现申请 操作记录  接口层
    /// </summary>
   public interface IWithdraw_Operate
    {
        /// <summary>
        /// 根据提现申请Id获取相关操作记录
        /// </summary>
        /// <param name="withdraw_Id"></param>
        /// <returns></returns>
      List<WithdrawOperateInfo>  ReadListByWithdraw(int withdraw_Id);
    }
}
