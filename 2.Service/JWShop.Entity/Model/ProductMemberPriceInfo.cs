using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品会员价格实体模型
    /// </summary>
    public sealed class ProductMemberPriceInfo
    {
        public const string TABLENAME = "ProductMemberPrice";

        private int productId;
        private int gradeId;
        private decimal price;

        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        /// <summary>
        /// 用户等级Id
        /// </summary>
        public int GradeId
        {
            set { this.gradeId = value; }
            get { return this.gradeId; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price
        {
            set { this.price = value; }
            get { return this.price; }
        }
    }
}