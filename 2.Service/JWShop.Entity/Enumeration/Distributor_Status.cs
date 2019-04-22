using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 分销商状态
    /// </summary>
    public enum Distributor_Status
    {
        /// <summary>
        /// 冻结
        /// </summary>
        [Enum("冻结")]
        Frozen=-1,
        /// <summary>
        /// 待审核
        /// </summary>
        [Enum("待审核")]
        WaitCheck=0,
        /// <summary>
        /// 正常
        /// </summary>
        [Enum("正常")]
        Normal=1,
    }

    /// <summary>
    /// 提现状态
    /// </summary>
   public enum Withdraw_Status
    {
        /// <summary>
        /// 拒绝
        /// </summary>
        [Enum("拒绝")]
        Reject=-1,
        /// <summary>
        /// 申请
        /// </summary>
        [Enum("待审核")]
        Apply=1,
        /// <summary>
        /// 完成
        /// </summary>
        [Enum("完成")]
        Complete=2,
    }

    /// <summary>
    /// 提现 审核操作
    /// </summary>
    public enum Withdraw_Operate
    {
        /// <summary>
        /// 拒绝
        /// </summary>
        [Enum("拒绝")]
        Reject = -1,
        /// <summary>
        /// 申请
        /// </summary>
        [Enum("申请")]
        Apply = 0,
        /// <summary>
        /// 申请
        /// </summary>
        [Enum("通过")]
        Pass = 1,
    }
}
