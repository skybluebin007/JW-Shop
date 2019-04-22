using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品类型规格记录实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductTypeStandardRecordInfo
    {
        public const string TABLENAME = "ProductTypeStandardRecord";

        private int productId;
        private string standardIdList = string.Empty;
        private string valueList = string.Empty;
        private decimal marketPrice;
        private decimal salePrice;
        private int storage;
        private int orderCount;
        private int sendCount;
        private string productCode = string.Empty;
        private string photo = string.Empty;

        private string groupTag = string.Empty;
        /// <summary>
        /// 团购价格
        /// </summary>
        public decimal GroupPrice { get; set; }
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string StandardIdList
        {
            set { this.standardIdList = value; }
            get { return this.standardIdList; }
        }
        public string ValueList
        {
            set { this.valueList = value; }
            get { return this.valueList; }
        }
        public decimal MarketPrice
        {
            set { this.marketPrice = value; }
            get { return this.marketPrice; }
        }
        public decimal SalePrice
        {
            set { this.salePrice = value; }
            get { return this.salePrice; }
        }
        public int Storage
        {
            set { this.storage = value; }
            get { return this.storage; }
        }
        public int OrderCount
        {
            set { this.orderCount = value; }
            get { return this.orderCount; }
        }
        public int SendCount
        {
            set { this.sendCount = value; }
            get { return this.sendCount; }
        }
        public string ProductCode
        {
            set { this.productCode = value; }
            get { return this.productCode; }
        }
        public string GroupTag
        {
            set { this.groupTag = value; }
            get { return this.groupTag; }
        }

        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
    }
}