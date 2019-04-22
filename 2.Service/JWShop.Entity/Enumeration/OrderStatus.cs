using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 待付款
        /// </summary>
        [Enum("待付款")]
        WaitPay = 1,
        /// <summary>
        /// 待审核
        /// </summary>
        [Enum("待审核")]
        WaitCheck,
        /// <summary>
        /// 无效
        /// </summary>
        [Enum("无效")]
        NoEffect,
        /// <summary>
        /// 安装
        /// </summary>
        [Enum("安装")]
        Shipping,
        /// <summary>
        /// 试压
        /// </summary>
        [Enum("试压")]
        HasShipping,
        /// <summary>
        /// 已完成
        /// </summary>
        [Enum("已完成")]
        ReceiveShipping,
        /// <summary>
        /// 已退货
        /// </summary>
        [Enum("已退货")]
        HasReturn,
        /// <summary>
        /// 已删除
        /// </summary>
        [Enum("已删除")]
        HasDelete
    }
    /// <summary>
    /// 订单生命周期状态 
    /// </summary>
    public enum OrderPeriod
    {/// <summary>
     /// 进行中：待付款、待审核、配货中、已发货
     /// </summary>
        [Enum("进行中")]
        Going = 1,
        /// <summary>
        /// 已完成：已收货
        /// </summary>
        [Enum("已完成")]
        Finished,
        /// <summary>
        /// 已取消
        /// </summary>
        [Enum("已取消")]
        Closed
    }
}