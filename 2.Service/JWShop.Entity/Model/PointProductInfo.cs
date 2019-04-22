using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 积分商品实体模型
    /// </summary>
    [Serializable]
    public sealed class PointProductInfo
    {
        public const string TABLENAME = "PointProduct";

        private int id;
        private string name = string.Empty;
        private string subTitle = string.Empty;
        private decimal marketPrice;
        private int point;
        private string photo = string.Empty;
        private string keywords = string.Empty;
        private string summary = string.Empty;
        private string introduction1 = string.Empty;
        private string introduction1_Mobile = string.Empty;
        private int isSale;
        private int viewCount;
        private int totalStorageCount;
        private int sendCount;
        private DateTime addDate = DateTime.Now;
        private int orderId;
        private DateTime beginDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;

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
        public string SubTitle
        {
            set { this.subTitle = value; }
            get { return this.subTitle; }
        }
        public decimal MarketPrice
        {
            set { this.marketPrice = value; }
            get { return this.marketPrice; }
        }
        public int Point
        {
            set { this.point = value; }
            get { return this.point; }
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
        public int IsSale
        {
            set { this.isSale = value; }
            get { return this.isSale; }
        }
        public int ViewCount
        {
            set { this.viewCount = value; }
            get { return this.viewCount; }
        }
        public int TotalStorageCount
        {
            set { this.totalStorageCount = value; }
            get { return this.totalStorageCount; }
        }
        public int SendCount
        {
            set { this.sendCount = value; }
            get { return this.sendCount; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public DateTime BeginDate
        {
            set { this.beginDate = value; }
            get { return this.beginDate; }
        }
        public DateTime EndDate
        {
            set { this.endDate = value; }
            get { return this.endDate; }
        }
    }
}