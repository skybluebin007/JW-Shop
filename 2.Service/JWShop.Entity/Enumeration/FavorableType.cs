using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 优惠活动类型
    /// </summary>
    public enum FavorableType
    {
        /// <summary>
        /// 针对整站订单
        /// </summary>
        AllOrders=0,
        /// <summary>
        /// 针对商品分类
        /// </summary>
        ProductClass=1

    }
}
