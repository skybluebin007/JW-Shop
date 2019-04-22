using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 产品收藏实体模型
    /// </summary>
    public sealed class ProductCollectInfo
    {
        public const string TABLENAME = "ProductCollect";

        private int id;
        private int productId;
        private DateTime tm = DateTime.Now;
        private int userId;

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
        public DateTime Tm
        {
            set { this.tm = value; }
            get { return this.tm; }
        }
        public int UserId
        {
            set { this.userId = value; }
            get { return this.userId; }
        }
    }
}