using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品库存计算类型
    /// </summary>
    public enum ProductStorageType
    {
        /// <summary>
        /// 自身的库存系统
        /// </summary>
        SelfStorageSystem = 1,
        /// <summary>
        /// 导入第三方的库存系统
        /// </summary>
        ImportStorageSystem
    }
}
