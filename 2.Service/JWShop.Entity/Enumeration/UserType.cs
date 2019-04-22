using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 会员类别
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 客户
        /// </summary>
        [Enum("客户")]
        Member = 0,
        /// <summary>
        /// 试压人员
        /// </summary>
        [Enum("试压人员")]
        shiya = 1,
        /// <summary>
        /// 水工人员
        /// </summary>
        [Enum("水工人员")]
        shuigong = 2,
        /// <summary>
        /// 经销商
        /// </summary>
        [Enum("经销商")]
        Provider = 3,

    }
}
