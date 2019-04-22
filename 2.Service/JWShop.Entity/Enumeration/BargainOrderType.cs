using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 砍价状态
    /// </summary>
    public enum Bargain_Status
    {
        /// <summary>
        /// 已结束
        /// </summary>
        End=-1,
        /// <summary>
        /// 已关闭
        /// </summary>
        ShutDown=0,
        /// <summary>
        /// 进行中
        /// </summary>
        OnGoing=1
    }
    /// <summary>
    /// 砍价订单状态
    /// </summary>
    public enum BargainOrderType
    {
        /// <summary>
        /// 未砍到底价
        /// </summary>
        进行中=1,
        /// <summary>
        /// 砍到底价，待下单
        /// </summary>
        砍价成功=2,
        /// <summary>
        /// 砍价失败
        /// </summary>
        砍价失败=3,
        /// <summary>
        /// 已下单，待付款
        /// </summary>
        待付款=4,
        /// <summary>
        /// 已付款
        /// </summary>
        支付完成=5
        
    }
}
