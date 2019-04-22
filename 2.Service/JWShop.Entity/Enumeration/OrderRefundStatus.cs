using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 退款 订单/商品 状态
    /// </summary>
    public enum OrderRefundStatus
    {
        /// <summary>
        /// 退款申请已提交，等待客服审核
        /// </summary>
        [Enum("申请退款")]
        Submit = 1,
        /// <summary>
        /// 系统审核通过，等待处理退款
        /// </summary>
        [Enum("系统已审核")]
        Approve = 2,
        /// <summary>
        /// 正在处理退款
        /// </summary>
        [Enum("正在处理退款")]
        Returning = 10,
        /// <summary>
        /// 退款完成
        /// </summary>
        [Enum("退款完成")]
        HasReturn = 11,

        /// <summary>
        /// 取消退款
        /// </summary>
        [Enum("退款取消")]
        Cancel = -1,
        /// <summary>
        /// 退款请求审核不通过
        /// </summary>
        [Enum("审核不通过")]
        Reject = -2,
    }
}