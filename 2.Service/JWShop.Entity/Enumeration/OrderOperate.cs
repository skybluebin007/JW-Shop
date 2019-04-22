using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 订单操作
    /// </summary>
    public enum OrderOperate
    {
        /// <summary>
        /// 付款
        /// </summary>
        Pay = 1,
        /// <summary>
        /// 审核
        /// </summary>
        Check,
        /// <summary>
        /// 取消
        /// </summary>
        Cancle,
        /// <summary>
        /// 安装
        /// </summary>
        NoSend,
        /// <summary>
        /// 试压
        /// </summary>
        Send,
        /// <summary>
        /// 完成订单
        /// </summary>
        Received,
        /// <summary>
        /// 换货确认
        /// </summary>
        Change,
        /// <summary>
        /// 退货确认
        /// </summary>
        Return,
        /// <summary>
        /// 撤销
        /// </summary>
        Back,
        /// <summary>
        /// 退款
        /// </summary>
        Refund
    }
}