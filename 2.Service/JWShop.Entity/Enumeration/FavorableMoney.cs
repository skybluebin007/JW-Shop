using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 价格优惠
    /// </summary>
    public enum FavorableMoney
    {
        /// <summary>
        /// 无价格优惠
        /// </summary>
        No=0,
        /// <summary>
        /// 现金优惠
        /// </summary>
        Money,
        /// <summary>
        /// 价格折扣优惠
        /// </summary>
        Discount
    }
}
