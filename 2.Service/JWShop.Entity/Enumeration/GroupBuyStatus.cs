using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 团购状态
    /// </summary>
   public enum GroupBuyStatus
    {
        /// <summary>
        /// 拼团失败（到期未拼满）
        /// </summary>
        Fail=-1,
        /// <summary>
        /// 进行中（在有效期内，暂未拼满）
        /// </summary>
        Going=0,
        /// <summary>
        /// 拼团成功
        /// </summary>
        Success=1
    }
}
