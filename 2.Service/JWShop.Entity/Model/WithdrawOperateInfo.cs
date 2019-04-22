using System;
using Dapper.Contrib.Extensions;
namespace JWShop.Entity
{
    /// <summary>
    /// 提现审核操作记录 实体模型
    /// </summary>
    public sealed class WithdrawOperateInfo
    {
        public int Id { get; set; }
        public int Withdraw_Id { get; set; }
        /// <summary>
        /// 操作者 Id
        /// </summary>
        public int Operator_Id { get; set; }
        /// <summary>
        /// 提现操作：   1--审核通过，线下提现完成（本次流程结束）
        ///2--审核拒绝（本次流程结束）
        /// </summary>
        public int Operate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 操作者  名称
        /// </summary>
        [Computed]
        public string Operator_Name { get; set; }
    }
}
