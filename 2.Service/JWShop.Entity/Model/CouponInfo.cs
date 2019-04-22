using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 优惠券实体模型
    /// </summary>
    [Table("Coupon")]
    public sealed class CouponInfo
    {
        public const string TABLENAME = "Coupon";

        private int id;
        private string name = string.Empty;
        private decimal money;
        private decimal useMinAmount;
        private DateTime useStartDate = DateTime.Now;
        private DateTime useEndDate = DateTime.Now;
        /// <summary>
        /// 后台添加优惠券类别：1-通用，2-注册赠送，3-确认收货赠送
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 优惠券封面图
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 优惠券总数量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 优惠券已发放数量
        /// </summary>
        public int UsedCount { get; set; }

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public decimal Money
        {
            set { this.money = value; }
            get { return this.money; }
        }
        public decimal UseMinAmount
        {
            set { this.useMinAmount = value; }
            get { return this.useMinAmount; }
        }
        public DateTime UseStartDate
        {
            set { this.useStartDate = value; }
            get { return this.useStartDate; }
        }
        public DateTime UseEndDate
        {
            set { this.useEndDate = value; }
            get { return this.useEndDate; }
        }
        /// <summary>
        /// ajax加载使用--开始日期
        /// </summary>
        [Computed]
        public string AjaxStartDate { get; set; }
        /// <summary>
        /// ajax加载使用--结束日期
        /// </summary>
        [Computed]
        public string AjaxEndDate { get; set; }
    }
}