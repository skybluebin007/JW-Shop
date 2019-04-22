using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 缺货登记搜索模型。
    /// </summary>
    public sealed class BookingProductSearchInfo
    {
        private string productName = string.Empty;
        private string relationUser = string.Empty;
        private string email = string.Empty;
        private string tel = string.Empty;
        private int isHandler = int.MinValue;
        private int userID = int.MinValue;

        public string ProductName
        {
            set { this.productName = value; }
            get { return this.productName; }
        }
        public string RelationUser
        {
            set { this.relationUser = value; }
            get { return this.relationUser; }
        }
        public string Email
        {
            set { this.email = value; }
            get { return this.email; }
        }
        public string Tel
        {
            set { this.tel = value; }
            get { return this.tel; }
        }
        public int IsHandler
        {
            set { this.isHandler = value; }
            get { return this.isHandler; }
        }
        public int UserID
        {
            set { this.userID = value; }
            get { return this.userID; }
        }
    }
}

