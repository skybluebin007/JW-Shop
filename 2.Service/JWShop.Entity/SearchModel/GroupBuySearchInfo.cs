using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 拼团活动搜索模型
    /// </summary>
  public sealed  class GroupBuySearchInfo
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 团购商品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 团长Id
        /// </summary>
        public int Leader { get; set; }
        /// <summary>
        /// 排除团长Id
        /// </summary>
        public int NotLeader { get; set; }
        /// <summary>
        /// 拼团状态，对应enum--GroupBuyStatus.默认2是查找所有状态
        /// </summary>
        public int Status { get; set; } = 2;
    }
}
