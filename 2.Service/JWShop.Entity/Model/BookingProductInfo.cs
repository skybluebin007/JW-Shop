using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 缺货登记实体模型
    /// </summary>
    public sealed class BookingProductInfo
    {
        private int id;
        private int productID;
        private string productName = string.Empty;
        private string relationUser = string.Empty;
        private string email = string.Empty;
        private string tel = string.Empty;
        private string userNote = string.Empty;
        private DateTime bookingDate = DateTime.Now;
        private string bookingIP = string.Empty;
        private int isHandler;
        private DateTime handlerDate = DateTime.Now;
        private int handlerAdminID;
        private string handlerAdminName = string.Empty;
        private string handlerNote = string.Empty;
        private int userID;
        private string userName = string.Empty;

        public int ID
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ProductID
        {
            set { this.productID = value; }
            get { return this.productID; }
        }
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
        public string UserNote
        {
            set { this.userNote = value; }
            get { return this.userNote; }
        }
        public DateTime BookingDate
        {
            set { this.bookingDate = value; }
            get { return this.bookingDate; }
        }
        public string BookingIP
        {
            set { this.bookingIP = value; }
            get { return this.bookingIP; }
        }
        public int IsHandler
        {
            set { this.isHandler = value; }
            get { return this.isHandler; }
        }
        public DateTime HandlerDate
        {
            set { this.handlerDate = value; }
            get { return this.handlerDate; }
        }
        public int HandlerAdminID
        {
            set { this.handlerAdminID = value; }
            get { return this.handlerAdminID; }
        }
        public string HandlerAdminName
        {
            set { this.handlerAdminName = value; }
            get { return this.handlerAdminName; }
        }
        public string HandlerNote
        {
            set { this.handlerNote = value; }
            get { return this.handlerNote; }
        }
        public int UserID
        {
            set { this.userID = value; }
            get { return this.userID; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }
    }
}