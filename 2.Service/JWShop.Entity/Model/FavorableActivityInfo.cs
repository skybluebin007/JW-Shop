using Dapper.Contrib.Extensions;
using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 优惠活动模型
    /// </summary>
    [Serializable]
    public sealed class FavorableActivityInfo
    {
        private int id;
        private string name = string.Empty;
        private string photo = string.Empty;
        private string content = string.Empty;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private string userGrade = string.Empty;
        private decimal orderProductMoney;
        private string regionId = string.Empty;
        private int shippingWay;
        private int reduceWay;
        private decimal reduceMoney;
        private decimal reduceDiscount;
        private string giftId = string.Empty;
        public int Type { get; set; }
        public string ClassIds { get; set; }
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
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }
        public DateTime StartDate
        {
            set { this.startDate = value; }
            get { return this.startDate; }
        }
        public DateTime EndDate
        {
            set { this.endDate = value; }
            get { return this.endDate; }
        }
        public string UserGrade
        {
            set { this.userGrade = value; }
            get { return this.userGrade; }
        }
        public decimal OrderProductMoney
        {
            set { this.orderProductMoney = value; }
            get { return this.orderProductMoney; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public int ShippingWay
        {
            set { this.shippingWay = value; }
            get { return this.shippingWay; }
        }
        public int ReduceWay
        {
            set { this.reduceWay = value; }
            get { return this.reduceWay; }
        }
        public decimal ReduceMoney
        {
            set { this.reduceMoney = value; }
            get { return this.reduceMoney; }
        }
        public decimal ReduceDiscount
        {
            set { this.reduceDiscount = value; }
            get { return this.reduceDiscount; }
        }
        public string GiftId
        {
            set { this.giftId = value; }
            get { return this.giftId; }
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
        /// <summary>
        /// ajax加载使用--会员等级名称
        /// </summary>
        [Computed]
        public string UserGradeNames { get; set; }
    }
}