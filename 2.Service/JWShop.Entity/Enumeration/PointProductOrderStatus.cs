using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 积分商品订单状态
    /// </summary>
    public enum PointProductOrderStatus
    {
        /// <summary>
        /// 待发货
        /// </summary>
        [Enum("待发货")]
        Shipping = 1,
        /// <summary>
        /// 待收货
        /// </summary>
        [Enum("待收货")]
        HasShipping = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        [Enum("已完成")]
        ReceiveShipping = 3,
        /// <summary>
        /// 已取消
        /// </summary>
        [Enum("已取消")]
        Cancel = -1
    }
}