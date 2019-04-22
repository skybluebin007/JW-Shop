using System;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品搜索模型。
    /// </summary>
    public sealed class ProductSearchInfo
    {
        private int standardType = int.MinValue;
        private string key = string.Empty;
        private string name = string.Empty;
        private string aliasName = string.Empty;
        private string spelling = string.Empty;
        private string productNumber = string.Empty;
        private string classId = string.Empty;
        private string keywords = string.Empty;
        private int brandId = int.MinValue;
        private int isSpecial = int.MinValue;
        private int isNew = int.MinValue;
        private int isHot = int.MinValue;
        private int isSale = int.MinValue;
        private int isTop = int.MinValue;
        private DateTime startAddDate = DateTime.MinValue;
        private DateTime endAddDate = DateTime.MinValue;
        private string inProductId = string.Empty;
        private string notInProductId = string.Empty;
        private int storageAnalyse = int.MinValue;
        private string productOrderType = string.Empty;
        private string tags = string.Empty;
        private OrderType orderType = OrderType.Desc;
        private string inShopId = string.Empty;
        private int stockStart;
        private int stockEnd;
        private int lowerOrderCount = int.MinValue;
        private int upperOrderCount = int.MinValue;
        private decimal lowerSalePrice=decimal.MinValue;
        private decimal upperSalePrice = decimal.MinValue;
        private int isDelete = int.MinValue;

        private string yejiratio = string.Empty;

        public int StandardType
        {
            set { this.standardType = value; }
            get { return this.standardType; }
        }
        public string Key
        {
            set { this.key = value; }
            get { return this.key; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string AliasName
        {
            set { this.aliasName = value; }
            get { return this.aliasName; }
        }
        public string Spelling
        {
            set { this.spelling = value; }
            get { return this.spelling; }
        }
        public string ProductNumber
        {
            set { this.productNumber = value; }
            get { return this.productNumber; }
        }
        public string ClassId
        {
            set { this.classId = value; }
            get { return this.classId; }
        }
        public string Keywords
        {
            set { this.keywords = value; }
            get { return this.keywords; }
        }
        public int BrandId
        {
            set { this.brandId = value; }
            get { return this.brandId; }
        }
        public int IsSpecial
        {
            set { this.isSpecial = value; }
            get { return this.isSpecial; }
        }
        public int IsNew
        {
            set { this.isNew = value; }
            get { return this.isNew; }
        }
        public int IsHot
        {
            set { this.isHot = value; }
            get { return this.isHot; }
        }
        public int IsSale
        {
            set { this.isSale = value; }
            get { return this.isSale; }
        }
        public int IsTop
        {
            set { this.isTop = value; }
            get { return this.isTop; }
        }
        public DateTime StartAddDate
        {
            set { this.startAddDate = value; }
            get { return this.startAddDate; }
        }
        public DateTime EndAddDate
        {
            set { this.endAddDate = value; }
            get { return this.endAddDate; }
        }
        public string InProductId
        {
            set { this.inProductId = value; }
            get { return this.inProductId; }
        }
        public string NotInProductId
        {
            set { this.notInProductId = value; }
            get { return this.notInProductId; }
        }
        public int StorageAnalyse
        {
            set { this.storageAnalyse = value; }
            get { return this.storageAnalyse; }
        }
        /// <summary>
        /// 产品排序字段
        /// </summary>
        public string ProductOrderType
        {
            set { this.productOrderType = value; }
            get { return this.productOrderType; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags
        {
            set { this.tags = value; }
            get { return this.tags; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public OrderType OrderType
        {
            set { this.orderType = value; }
            get { return this.orderType; }
        }
        public string InShopId
        {
            get { return inShopId; }
            set { inShopId = value; }
        }
        public int StockStart
        {
            get { return this.stockStart; }
            set { this.stockStart = value; }
        }
        public int StockEnd
        {
            get { return this.stockEnd; }
            set { this.stockEnd = value; }
        }
        public int LowerOrderCount {
            get { return this.lowerOrderCount; }
            set { this.lowerOrderCount = value; }
        }
        public int UpperOrderCount {
            get { return this.upperOrderCount; }
            set { this.upperOrderCount = value; }
        }
        public decimal LowerSalePrice {
            get { return this.lowerSalePrice; }
            set { this.lowerSalePrice = value; }
        }
        public decimal UpperSalePrice {
            get { return this.upperSalePrice; }
            set { this.upperSalePrice = value; }
        }
        public int IsDelete {
            get { return this.isDelete; }
            set { this.isDelete = value; }
        }
        /// <summary>
        /// 是否开团
        /// </summary>
        public int OpenGroup { get; set; } = int.MinValue;

        /// <summary>
        /// 提成比例
        /// </summary>
        public string YejiRatio
        {
            get { return this.yejiratio; }
            set { this.yejiratio = value; }
        }
    }
}