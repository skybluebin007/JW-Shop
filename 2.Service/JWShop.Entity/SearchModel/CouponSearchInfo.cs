using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 优惠券搜索模型。
    /// </summary>
    public sealed class CouponSearchInfo
    {
        private string name = string.Empty;
        private int[] inCouponIds = new int[] { };
        /// <summary>
        /// 是否可用：1-可用，不等于1则不限制
        /// </summary>
        public int CanUse { get; set; }
        /// <summary>
        /// 后台添加优惠券类别：1-通用，2-注册赠送，3-确认收货赠送
        /// </summary>
        public int Type { get; set; }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public int[] InCouponIds
        {
            set { this.inCouponIds = value; }
            get { return this.inCouponIds; }
        }
        /// <summary>
        /// 1-未开始，2-进行中，3-已结束
        /// </summary>
        public int TimePeriod
        {
            get;set;
        }
        /// <summary>
        /// 1-已耗尽（总数量全部发放完），0-还有余量
        /// </summary>
        public int Depleted
        {
            get;set;
        }
    }
}