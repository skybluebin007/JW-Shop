using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品图片实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductPhotoInfo
    {
        public const string TABLENAME = "ProductPhoto";

        private int id;
        private int productId;
        private string name = string.Empty;
        private string imageUrl = string.Empty;
        private int proStyle;
        private DateTime addDate;
        private int orderId;
        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string ImageUrl
        {
            set { this.imageUrl = value; }
            get { return this.imageUrl; }
        }
        public int ProStyle
        {
            set { this.proStyle = value; }
            get { return this.proStyle; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public int OrderId {
            get { return this.orderId; }
            set { this.orderId = value; }
        }
    }
}