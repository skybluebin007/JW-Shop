using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 返佣记录 搜索模型
    /// </summary>
  public sealed  class RebateSearchInfo
    {
        public int Distributor_Id { get; set; } = int.MinValue;
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTtime { get; set; } = DateTime.MinValue;
        /// <summary>
        /// 分销商用户名
        /// </summary>     
        public string UserName { get; set; }
        /// <summary>
        /// 分销商真实姓名
        /// </summary>      
        public string RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>       
        public string Mobile { get; set; }
    }
}
