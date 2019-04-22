using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 时间类型（统计使用）
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// 按天统计（统计该月的每天）
        /// </summary>
        Day=1,
        /// <summary>
        /// 按月统计
        /// </summary>
        Month,
        /// <summary>
        /// 按小时统计
        /// </summary>
        Hour
    }
}
