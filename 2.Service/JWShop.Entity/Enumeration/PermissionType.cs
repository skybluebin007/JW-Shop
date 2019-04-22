using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    public enum PermissionType
    {
        /// <summary>
        /// 面板
        /// </summary>
        [Enum("面板")]
        Panel = 1,
        /// <summary>
        /// 页面
        /// </summary>
        [Enum("页面")]
        Page = 2,
        /// <summary>
        /// 动作
        /// </summary>
        [Enum("动作")]
        Action = 3
    }
}
