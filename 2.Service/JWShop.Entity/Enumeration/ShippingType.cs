using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 配送运费方式
    /// </summary>
    public enum ShippingType
    {
        /// <summary>
        /// 固定运费
        /// </summary>
        Fixed=1,
        /// <summary>
        /// 按重量计算
        /// </summary>
        Weight,
        /// <summary>
        /// 按商品数量
        /// </summary>
        ProductCount
    }
}
