using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品品牌实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductBrandInfo
    {
        public const string TABLENAME = "ProductBrand";

        private int id;
        private string name = string.Empty;
        private string spelling = string.Empty;
        private string imageUrl = string.Empty;
        private string linkUrl = string.Empty;
        private string remark = string.Empty;
        private int orderId;
        private int isTop;

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
        public string Spelling
        {
            set { this.spelling = value; }
            get { return this.spelling; }
        }
        public string ImageUrl
        {
            set { this.imageUrl = value; }
            get { return this.imageUrl; }
        }
        public string LinkUrl
        {
            set { this.linkUrl = value; }
            get { return this.linkUrl; }
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
        public int IsTop
        {
            set { this.isTop = value; }
            get { return this.isTop; }
        }
    }
}