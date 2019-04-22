using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 评论状态
    /// </summary>
    public enum CommentStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Enum("未处理")]
        NoHandler = 1,
        /// <summary>
        /// 显示
        /// </summary>
        [Enum("显示")]
        Show,
        /// <summary>
        /// 不显示
        /// </summary>
        [Enum("不显示")]
        NoShow
    }
}
