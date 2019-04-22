using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户账户类型： 积分、金额
    /// </summary>
    public enum AccountRecordType
    {
        /// <summary>
        /// 金额
        /// </summary>
        [Enum("金额")]
        Money = 1,
        /// <summary>
        /// 积分
        /// </summary>
        [Enum("积分")]
        Point
    }
}
