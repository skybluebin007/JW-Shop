using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 链接实体模型
    /// </summary>
    public sealed class LinkInfo
    {
        private int id;
        private int linkClass;
        private string display = string.Empty;
        private string uRL = string.Empty;
        private int orderID;
        private string remark = string.Empty;

        public int ID
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int LinkClass
        {
            set { this.linkClass = value; }
            get { return this.linkClass; }
        }
        public string Display
        {
            set { this.display = value; }
            get { return this.display; }
        }
        public string URL
        {
            set { this.uRL = value; }
            get { return this.uRL; }
        }
        public int OrderID
        {
            set { this.orderID = value; }
            get { return this.orderID; }
        }
        public string Remark
        {
            set { this.remark = value; }
            get { return this.remark; }
        }
    }
}