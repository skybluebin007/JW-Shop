using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 订单种类
    /// </summary>
   public enum OrderKind
    {
      /// <summary>
      /// 普通订单（直接购买）
      /// </summary>
     [Enum("直接购买") ]
        Common=0,
        /// <summary>
        /// 积分兑换
        /// </summary>
        [Enum("积分订单")]
        PointBuy=1,
        /// <summary>
        /// 团购订单
        /// </summary>
        [Enum("拼团订单")]
        GroupBuy=2,

        /// <summary>
        /// 砍价订单
        /// </summary>
        [Enum("砍价订单")]
        Bargain = 3
    }
}
