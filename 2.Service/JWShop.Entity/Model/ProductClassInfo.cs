using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品分类实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductClassInfo
    {
        public const string TABLENAME = "ProductClass";

        private int id;
        private int parentId;
        private int productTypeId;
        private string name = string.Empty;
        private string number = string.Empty;
        private int productCount;
        private string keywords = string.Empty;
        private string remark = string.Empty;
        private int orderId;
        private DateTime tm;

        private string photo = string.Empty;
        private string enClassName = string.Empty;
        private string pageTitle = string.Empty;
        private string pageKeyWord = string.Empty;
        private string pageSummary = string.Empty;
        private int isSystem;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ParentId
        {
            set { this.parentId = value; }
            get { return this.parentId; }
        }
        public int ProductTypeId
        {
            set { this.productTypeId = value; }
            get { return this.productTypeId; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string Number
        {
            set { this.number = value; }
            get { return this.number; }
        }
        public int ProductCount
        {
            set { this.productCount = value; }
            get { return this.productCount; }
        }
        public string Keywords
        {
            set { this.keywords = value; }
            get { return this.keywords; }
        }
        public string Remark
        {
            set { this.remark = value; }
            get { return this.remark; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public DateTime Tm
        {
            get { return tm; }
            set { tm = value; }
        }

        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string EnClassName
        {
            set { this.enClassName = value; }
            get { return this.enClassName; }
        }
        public string PageTitle
        {
            set { this.pageTitle = value; }
            get { return this.pageTitle; }
        }
        public string PageKeyWord
        {
            set { this.pageKeyWord = value; }
            get { return this.pageKeyWord; }
        }
        public string PageSummary
        {
            set { this.pageSummary = value; }
            get { return this.pageSummary; }
        }
        public int IsSystem
        {
            set { this.isSystem = value; }
            get { return this.isSystem; }
        }
    }
}