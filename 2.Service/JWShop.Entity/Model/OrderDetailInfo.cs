using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 订单详细实体模型
    /// </summary>
    public sealed class OrderDetailInfo
    {
        public const string TABLENAME = "OrderDetail";

        private int id;
        private int orderId;
        private int productId;
        private string productName = string.Empty;
        private string standardValueList = string.Empty;
        private decimal productWeight;
        private int sendPoint;
        private decimal productPrice;
        private decimal bidPrice;
        private int buyCount;
        private int parentId;
        private string randNumber = string.Empty;
        private int giftPackId;
        private int activityPoint;
        private int refundCount;


        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string ProductName
        {
            set { this.productName = value; }
            get { return this.productName; }
        }
        public string StandardValueList
        {
            set { this.standardValueList = value; }
            get { return this.standardValueList; }
        }
        public decimal ProductWeight
        {
            set { this.productWeight = value; }
            get { return this.productWeight; }
        }
        public int SendPoint
        {
            set { this.sendPoint = value; }
            get { return this.sendPoint; }
        }
        public decimal ProductPrice
        {
            set { this.productPrice = value; }
            get { return this.productPrice; }
        }
        public decimal BidPrice
        {
            set { this.bidPrice = value; }
            get { return this.bidPrice; }
        }
        public int BuyCount
        {
            set { this.buyCount = value; }
            get { return this.buyCount; }
        }
        public int ParentId
        {
            set { this.parentId = value; }
            get { return this.parentId; }
        }
        public string RandNumber
        {
            set { this.randNumber = value; }
            get { return this.randNumber; }
        }
        public int GiftPackId
        {
            set { this.giftPackId = value; }
            get { return this.giftPackId; }
        }
        /// <summary>
        /// 积分兑换时，花费的积分
        /// </summary>
        public int ActivityPoint
        {
            set { this.activityPoint = value; }
            get { return this.activityPoint; }
        }
        /// <summary>
        /// 正在处理或已退款的商品数量
        /// </summary>
        public int RefundCount
        {
            set { this.refundCount = value; }
            get { return this.refundCount; }
        }
        /// <summary>
        /// 商品主图
        /// </summary>
        public string ProductPhoto { get; set; }

    }
}