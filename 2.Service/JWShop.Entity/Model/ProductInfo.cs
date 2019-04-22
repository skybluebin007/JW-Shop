using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductInfo
    {
        public const string TABLENAME = "Product";

        private int id;
        private int shopId;
        private string name = string.Empty;
        private string subTitle = string.Empty;
        private string classId = string.Empty;
        private string spelling = string.Empty;
        private string color = string.Empty;
        private string fontStyle = string.Empty;
        private int brandId;
        private string productNumber = string.Empty;
        private string productCode = string.Empty;
        private decimal marketPrice;
        private decimal bidPrice;
        private decimal salePrice;
        private decimal weight;
        private int sendPoint;
        private string photo = string.Empty;
        private string keywords = string.Empty;
        private string summary = string.Empty;
        private string introduction1 = string.Empty;
        private string introduction1_Mobile = string.Empty;
        private string introduction2 = string.Empty;
        private string introduction2_Mobile = string.Empty;
        private string introduction3 = string.Empty;
        private string introduction3_Mobile = string.Empty;
        private string remark = string.Empty;
        private int isSpecial;
        private int isNew;
        private int isHot;
        private int isSale;
        private int isTop;
        private string accessory = string.Empty;
        private string relationProduct = string.Empty;
        private string relationArticle = string.Empty;
        private int viewCount;
        private int allowComment;
        private int commentCount;
        private int sumPoint;
        private decimal perPoint;
        private int photoCount;
        private int collectCount;
        private int totalStorageCount;
        private int orderCount;
        private int sendCount;
        private int importActualStorageCount;
        private int importVirtualStorageCount;
        private int lowerCount;
        private int upperCount;
        private DateTime addDate = DateTime.Now;
        private int taobaoId;
        private int orderId;
        private string unit = string.Empty;
        private decimal taxRate;
        private int likeNum;
        private int unLikeNum;
        private string productionPlace = string.Empty;
        private string grossRate = string.Empty;      
        private int standardType;
        private string sellPoint = string.Empty;
        private int isDelete;
        private string qrcode = string.Empty;

        private string yejiratio = string.Empty;

        /// <summary>
        /// 不限库存，1-启用，0-不启用 
        /// </summary>
        public int UnlimitedStorage { get; set; }
        /// <summary>
        /// 是否启用虚拟销量,1-是，0-否
        /// </summary>
        public int UseVirtualOrder { get; set; }
        /// <summary>
        /// 虚拟销量数量
        /// </summary>
        public int VirtualOrderCount { get; set; }
        /// <summary>
        /// 是否支持开团，1-支持，0-不支持
        /// </summary>
        public int OpenGroup { get; set; }
        /// <summary>
        /// 团购人数
        /// </summary>
        public int GroupQuantity { get; set; }
        /// <summary>
        /// 团购价格
        /// </summary>
        public decimal GroupPrice { get; set; }
        /// <summary>
        /// 拼团图片
        /// </summary>
        public string GroupPhoto { get; set; }

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ShopId
        {
            set { this.shopId = value; }
            get { return this.shopId; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string SubTitle
        {
            set { this.subTitle = value; }
            get { return this.subTitle; }
        }
        public string ClassId
        {
            set { this.classId = value; }
            get { return this.classId; }
        }
        public string Spelling
        {
            set { this.spelling = value; }
            get { return this.spelling; }
        }
        public string Color
        {
            set { this.color = value; }
            get { return this.color; }
        }
        public string FontStyle
        {
            set { this.fontStyle = value; }
            get { return this.fontStyle; }
        }
        public int BrandId
        {
            set { this.brandId = value; }
            get { return this.brandId; }
        }
        public string ProductNumber
        {
            set { this.productNumber = value; }
            get { return this.productNumber; }
        }
        public string ProductCode
        {
            set { this.productCode = value; }
            get { return this.productCode; }
        }
        public decimal MarketPrice
        {
            set { this.marketPrice = value; }
            get { return this.marketPrice; }
        }
        public decimal BidPrice
        {
            set { this.bidPrice = value; }
            get { return this.bidPrice; }
        }
        public decimal SalePrice
        {
            set { this.salePrice = value; }
            get { return this.salePrice; }
        }
        public decimal Weight
        {
            set { this.weight = value; }
            get { return this.weight; }
        }
        public int SendPoint
        {
            set { this.sendPoint = value; }
            get { return this.sendPoint; }
        }
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string Keywords
        {
            set { this.keywords = value; }
            get { return this.keywords; }
        }
        public string Summary
        {
            set { this.summary = value; }
            get { return this.summary; }
        }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string Introduction1
        {
            set { this.introduction1 = value; }
            get { return this.introduction1; }
        }
        public string Introduction1_Mobile
        {
            set { this.introduction1_Mobile = value; }
            get { return this.introduction1_Mobile; }
        }
        /// <summary>
        /// 包装及参数
        /// </summary>
        public string Introduction2
        {
            set { this.introduction2 = value; }
            get { return this.introduction2; }
        }
        public string Introduction2_Mobile
        {
            set { this.introduction2_Mobile = value; }
            get { return this.introduction2_Mobile; }
        }
        /// <summary>
        /// 售后保障
        /// </summary>
        public string Introduction3
        {
            set { this.introduction3 = value; }
            get { return this.introduction3; }
        }
        public string Introduction3_Mobile
        {
            set { this.introduction3_Mobile = value; }
            get { return this.introduction3_Mobile; }
        }
        public string Remark
        {
            set { this.remark = value; }
            get { return this.remark; }
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
        public string Accessory
        {
            set { this.accessory = value; }
            get { return this.accessory; }
        }
        public string RelationProduct
        {
            set { this.relationProduct = value; }
            get { return this.relationProduct; }
        }
        public string RelationArticle
        {
            set { this.relationArticle = value; }
            get { return this.relationArticle; }
        }
        public int ViewCount
        {
            set { this.viewCount = value; }
            get { return this.viewCount; }
        }
        public int AllowComment
        {
            set { this.allowComment = value; }
            get { return this.allowComment; }
        }
        public int CommentCount
        {
            set { this.commentCount = value; }
            get { return this.commentCount; }
        }
        public int SumPoint
        {
            set { this.sumPoint = value; }
            get { return this.sumPoint; }
        }
        public decimal PerPoint
        {
            set { this.perPoint = value; }
            get { return this.perPoint; }
        }
        public int PhotoCount
        {
            set { this.photoCount = value; }
            get { return this.photoCount; }
        }
        public int CollectCount
        {
            set { this.collectCount = value; }
            get { return this.collectCount; }
        }
        public int TotalStorageCount
        {
            set { this.totalStorageCount = value; }
            get { return this.totalStorageCount; }
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
        public int ImportActualStorageCount
        {
            set { this.importActualStorageCount = value; }
            get { return this.importActualStorageCount; }
        }
        public int ImportVirtualStorageCount
        {
            set { this.importVirtualStorageCount = value; }
            get { return this.importVirtualStorageCount; }
        }
        public int LowerCount
        {
            set { this.lowerCount = value; }
            get { return this.lowerCount; }
        }
        public int UpperCount
        {
            set { this.upperCount = value; }
            get { return this.upperCount; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public int TaobaoId
        {
            set { this.taobaoId = value; }
            get { return this.taobaoId; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public string Unit
        {
            set { this.unit = value; }
            get { return this.unit; }
        }
        public decimal TaxRate
        {
            set { this.taxRate = value; }
            get { return this.taxRate; }
        }

        //Total
        private int total;
        public int Total
        {
            set { this.total = value; }
            get { return this.total; }
        }

        public int LikeNum
        {
            set { this.likeNum = value; }
            get { return this.likeNum; }
        }
        public int UnLikeNum
        {
            set { this.unLikeNum = value; }
            get { return this.unLikeNum; }
        }
        /// <summary>
        /// 产地
        /// </summary>
        public string ProductionPlace
        {
            set { this.productionPlace = value; }
            get { return this.productionPlace; }
        }
        /// <summary>
        /// 毛利率
        /// </summary>
        public string GrossRate
        {
            set { this.grossRate = value; }
            get { return this.grossRate; }
        }


        public int StandardType
        {
            set { this.standardType = value; }
            get { return this.standardType; }
        }
        /// <summary>
        /// 商品卖点
        /// </summary>
        public string SellPoint {
            get { return this.sellPoint; }
            set { this.sellPoint = value; }
        }
        /// <summary>
        /// 逻辑删除:1-已删，0-未删
        /// </summary>
        public int IsDelete {
            get { return this.isDelete; }
            set { this.isDelete = value; }
        }
        /// <summary>
        /// 商品二维码
        /// </summary>
        public string Qrcode {
            get { return this.qrcode; }
            set { this.qrcode = value; }
        }

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