using System;

namespace JWShop.Entity
{
    public sealed class PointProductOrderSearchInfo
    {
        private string orderNumber = string.Empty;
        private int orderStatus = int.MinValue;
        private string productName = string.Empty;
        private int userId = int.MinValue;
        private string userName = string.Empty;
        private DateTime startAddDate = DateTime.MinValue;
        private DateTime endAddDate = DateTime.MinValue;

        public string OrderNumber
        {
            set { this.orderNumber = value; }
            get { return this.orderNumber; }
        }
        public int OrderStatus
        {
            set { this.orderStatus = value; }
            get { return this.orderStatus; }
        }
        public string ProductName
        {
            set { this.productName = value; }
            get { return this.productName; }
        }
        public int UserId {
            get { return this.userId; }
            set { this.userId = value; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
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
    }
}