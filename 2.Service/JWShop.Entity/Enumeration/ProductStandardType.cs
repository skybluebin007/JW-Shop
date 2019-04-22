using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 产品规格类型
    /// </summary>
    public enum ProductStandardType
    {
        /// <summary>
        /// 无规格
        /// </summary>
        [Enum("无规格")]
        No=0,
        /// <summary>
        /// 单产品规格
        /// </summary>
        [Enum("单产品规格")]
        Single,
        /// <summary>
        /// 产品组规格
        /// </summary>
        [Enum("产品组规格")]
        Group
    }
}
