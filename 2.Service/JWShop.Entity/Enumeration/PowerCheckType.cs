using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{ 
    /// <summary>
    /// 权限判断类别
    /// </summary>
    public enum PowerCheckType
    {
        /// <summary>
        /// 单一
        /// </summary>
        Single,
        /// <summary>
        /// 或者
        /// </summary>
        OR,
        /// <summary>
        /// 并且
        /// </summary>
        AND
    }
}
