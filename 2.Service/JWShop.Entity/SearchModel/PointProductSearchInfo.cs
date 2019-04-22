using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 积分兑换产品搜索模型。
	/// </summary>
    public sealed class PointProductSearchInfo
    {
        private int productId = int.MinValue;
        private string productName = string.Empty;
        private int point = int.MinValue;
        private DateTime beginDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MinValue;
        private int isSale = int.MinValue;
        private DateTime validDate = DateTime.MinValue;

        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string ProductName
        {
            set { this.productName = value; }
            get { return this.productName; }
        }
        public int Point
        {
            set { this.point = value; }
            get { return this.point; }
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
        public int IsSale
        {
            set { this.isSale = value; }
            get { return this.isSale; }
        }
        public DateTime ValidDate
        {
            set { this.validDate = value; }
            get { return this.validDate; }
        }
    }
}

