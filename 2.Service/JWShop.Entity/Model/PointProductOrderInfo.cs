using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 积分商品订单实体模型
    /// </summary>
    [Serializable]
    public sealed class PointProductOrderInfo
    {
        public const string TABLENAME = "PointProductOrder";

        private int id;
        private string orderNumber = string.Empty;
        private int orderStatus;
        private string orderNote = string.Empty;
        private int point;
        private int productId;
        private string productName = string.Empty;
        private int buyCount;
        private string consignee = string.Empty;
        private string regionId = string.Empty;
        private string address = string.Empty;
        private string tel = string.Empty;
        private string shippingName = string.Empty;
        private string shippingNumber = string.Empty;
        private DateTime? shippingDate = DateTime.Now;
        private DateTime addDate = DateTime.Now;
        private string iP = string.Empty;
        private int userId;
        private string userName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string OrderNumber
        {
            set { this.orderNumber = value; }
            get { return this.orderNumber; }
        }
        public int OrderStatus
        {
            set { this.orderStatus = value; }
            get { return this.orderStatus; }
        }
        public string OrderNote
        {
            set { this.orderNote = value; }
            get { return this.orderNote; }
        }
        public int Point
        {
            set { this.point = value; }
            get { return this.point; }
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
        public int BuyCount
        {
            set { this.buyCount = value; }
            get { return this.buyCount; }
        }
        public string Consignee
        {
            set { this.consignee = value; }
            get { return this.consignee; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public string Address
        {
            set { this.address = value; }
            get { return this.address; }
        }
        public string Tel
        {
            set { this.tel = value; }
            get { return this.tel; }
        }
        public string ShippingName
        {
            set { this.shippingName = value; }
            get { return this.shippingName; }
        }
        public string ShippingNumber
        {
            set { this.shippingNumber = value; }
            get { return this.shippingNumber; }
        }
        public DateTime? ShippingDate
        {
            set { this.shippingDate = value; }
            get { return this.shippingDate; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public string IP
        {
            set { this.iP = value; }
            get { return this.iP; }
        }
        public int UserId
        {
            set { this.userId = value; }
            get { return this.userId; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }

    }
}