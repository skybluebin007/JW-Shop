using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    ///  库存分析
    /// </summary>
    public enum StorageAnalyseType
    {
        /// <summary>
        /// 短缺
        /// </summary>
        Lack=1,
        /// <summary>
        /// 安全
        /// </summary>
        Safe,
        /// <summary>
        /// 超储
        /// </summary>
        Over
    }
}
