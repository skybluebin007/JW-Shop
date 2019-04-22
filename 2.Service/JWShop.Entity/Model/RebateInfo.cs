using Dapper.Contrib.Extensions;
using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 返佣记录 实体模型
    /// </summary>
  public sealed  class RebateInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 分销商Id
        /// </summary>
        public int Distributor_Id { get; set; }
        /// <summary>
        /// 消费者Id
        /// </summary>
        public int User_Id { get; set; }
        /// <summary>
        /// 消费订单ID 
        /// </summary>
        public int Order_Id { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        public DateTime Time { get; set; }

        /// <summary>
        /// 分销商用户名
        /// </summary>
        [Computed]
        public string UserName { get; set; }
        /// <summary>
        /// 分销商真实姓名
        /// </summary>
        [Computed]
        public string RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Computed]
        public string Mobile { get; set; }
        /// <summary>
        /// 消费者用户名
        /// </summary>
        [Computed]
        public string Buyer_UserName { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Computed]
        public string OrderNumber { get; set; }
    }
}
