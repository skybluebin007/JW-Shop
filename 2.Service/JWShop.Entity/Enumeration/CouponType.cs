using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 优惠券获取类型
    /// </summary>
    public enum CouponType
    {
        /// <summary>
        /// 在线发放
        /// </summary>
        [Enum("在线发放")]
        Online = 1,
        /// <summary>
        /// 线下发放
        /// </summary>
        [Enum("线下发放")]
        Offline,
        /// <summary>
        /// 会员领取
        /// </summary>
        [Enum("会员领取")]
        CatchByUser,
        /// <summary>
        /// 注册赠送
        /// </summary>
        [Enum("注册赠送")]
        RegisterGet,
        /// <summary>
        /// 确认收货赠送
        /// </summary>
        [Enum("确认收货赠送")]
        ReceiveShippingGet,
        /// <summary>
        /// 生日赠送
        /// </summary>
        [Enum("生日赠送")]
        BirthdayGet
    }
    /// <summary>
    /// 后台添加优惠券的类别
    /// </summary>
    public enum CouponKind
    {
        /// <summary>
        /// 通用
        /// </summary>
        [Enum("商家优惠券")]
        Common = 1,
        /// <summary>
        /// 注册赠送
        /// </summary>
        [Enum("新客立减")]
        RegisterGet = 2,
        /// <summary>
        /// 确认收货赠送
        /// </summary>
        [Enum("下单返券")]
        ReceiveShippingGet = 3,
        /// <summary>
        /// 会员生日礼券
        /// </summary>
        [Enum("会员生日券")]
        BirthdayGet =4
    }
}
