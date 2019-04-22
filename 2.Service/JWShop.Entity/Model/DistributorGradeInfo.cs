
namespace JWShop.Entity
{
    using System;
    /// <summary>
    /// 经销商等级 实体模型
    /// </summary>
    public sealed class DistributorGradeInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public decimal Min_Amount { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Max_Amount { get; set; }
        public decimal Discount { get; set; }
    }
}
