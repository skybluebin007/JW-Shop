using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 会员性别
    /// </summary>
    public enum SexType
    {
        /// <summary>
        /// 男
        /// </summary>
        [Enum("男")]
        Men=1,
        /// <summary>
        /// 女
        /// </summary>
        [Enum("女")]
        Women,
        /// <summary>
        /// 保密
        /// </summary>
        [Enum("保密")]
        Secret
    }
}
