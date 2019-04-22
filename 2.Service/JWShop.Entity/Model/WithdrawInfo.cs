using Dapper.Contrib.Extensions;
using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 提现记录 实体模型
    /// </summary>
    public sealed class WithdrawInfo
    {
        public int Id { get; set; }
        public int Distributor_Id { get; set; }
        /// <summary>
        /// 提现数值
        /// </summary>
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 状态 
        /// 1   --待审核（提出申请）
        ///-1 --拒绝         => 结束
        ///2  --提现完成  =>结束
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 分销商用户名
        /// </summary>
        [Computed]
        public string UserName { get; set; }
        /// <summary>
        /// 分销商真实姓名
        /// </summary>
        [Computed]
        public string RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Computed]
        public string Mobile { get; set; }

        /// <summary>
        /// 状态 描述
        /// </summary>
        [Computed]
        public string Status_Desc { get; set; }
    }
}
