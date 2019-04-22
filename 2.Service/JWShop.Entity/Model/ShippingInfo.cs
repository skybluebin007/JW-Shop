using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 物流实体模型
    /// </summary>
    public sealed class ShippingInfo
    {
        public const string TABLENAME = "Shipping";

        private int id;
        private string name = string.Empty;
        private string description = string.Empty;
        private int isEnabled;
        private int shippingType;
        private int firstWeight;
        private int againWeight;
        private int orderId;
        private string shippingCode;

        public string ShippingCode
        {
            set { this.shippingCode = value; }
            get { return this.shippingCode; }
        }

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
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        public int IsEnabled
        {
            set { this.isEnabled = value; }
            get { return this.isEnabled; }
        }
        public int ShippingType
        {
            set { this.shippingType = value; }
            get { return this.shippingType; }
        }
        public int FirstWeight
        {
            set { this.firstWeight = value; }
            get { return this.firstWeight; }
        }
        public int AgainWeight
        {
            set { this.againWeight = value; }
            get { return this.againWeight; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
    }
}