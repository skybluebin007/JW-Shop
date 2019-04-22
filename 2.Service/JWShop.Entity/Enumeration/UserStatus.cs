using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 未验证
        /// </summary>
        [Enum("未验证")]
        NoCheck = 1,
        /// <summary>
        /// 正常
        /// </summary>
        [Enum("正常")]
        Normal,
        /// <summary>
        /// 冻结
        /// </summary>
        [Enum("冻结")]
        Frozen
    }
}
