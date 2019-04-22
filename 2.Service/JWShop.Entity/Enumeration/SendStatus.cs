using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 发送状态
    /// </summary>
    public enum SendStatus
    {
        /// <summary>
        /// 未发送
        /// </summary>
        [Enum("未发送")]
        No=1,
        /// <summary>
        /// 发送中
        /// </summary>
        [Enum("发送中")]
        Sending,
        /// <summary>
        /// 发送完成
        /// </summary>
       [Enum("发送完成")]
        Finished
    }
}
