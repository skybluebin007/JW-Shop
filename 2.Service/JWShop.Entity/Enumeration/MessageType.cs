using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 留言类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 留言
        /// </summary>
        Message = 1,
        /// <summary>
        /// 投诉
        /// </summary>
        Complain,
        /// <summary>
        /// 询问
        /// </summary>
        Inquire,
        /// <summary>
        /// 售后
        /// </summary>
        AfterSale,
        /// <summary>
        /// 求购
        /// </summary>
        DemandBuy
    }
}
