using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 会员价格实体模型
    /// </summary>
    [Serializable]
    public sealed class MemberPriceInfo
    {
        private int productID;
        private int gradeID;
        private decimal price;

        public int ProductID
        {
            set { this.productID = value; }
            get { return this.productID; }
        }
        public int GradeID
        {
            set { this.gradeID = value; }
            get { return this.gradeID; }
        }
        public decimal Price
        {
            set { this.price = value; }
            get { return this.price; }
        }
    }
}