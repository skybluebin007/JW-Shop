using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 提货码 实体模型
    /// </summary>
   public sealed class PickUpCodeInfo
    {
        public const string TABLENAME = "PickUpCode";

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PickCode { get; set; }
        /// <summary>
        /// 提货码状态：1-已使用，0-未使用
        /// </summary>
        public int Status { get; set; }
    }
}
