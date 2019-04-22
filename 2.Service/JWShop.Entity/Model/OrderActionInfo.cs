using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 订单操作实体模型
    /// </summary>
    public sealed class OrderActionInfo
    {
        public const string TABLENAME = "OrderAction";

        private int id;
        private int orderId;
        private int orderOperate;
        private int startOrderStatus;
        private int endOrderStatus;
        private string note = string.Empty;
        private string iP = string.Empty;
        private DateTime date = DateTime.Now;
        private int adminID;
        private string adminName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public int OrderOperate
        {
            set { this.orderOperate = value; }
            get { return this.orderOperate; }
        }
        public int StartOrderStatus
        {
            set { this.startOrderStatus = value; }
            get { return this.startOrderStatus; }
        }
        public int EndOrderStatus
        {
            set { this.endOrderStatus = value; }
            get { return this.endOrderStatus; }
        }
        public string Note
        {
            set { this.note = value; }
            get { return this.note; }
        }
        public string IP
        {
            set { this.iP = value; }
            get { return this.iP; }
        }
        public DateTime Date
        {
            set { this.date = value; }
            get { return this.date; }
        }
        public int AdminID
        {
            set { this.adminID = value; }
            get { return this.adminID; }
        }
        public string AdminName
        {
            set { this.adminName = value; }
            get { return this.adminName; }
        }
    }
}