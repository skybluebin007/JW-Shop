using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 菜单实体模型
    /// </summary>
    [Serializable]
    public sealed class MenuInfo
    {
        private int id;
        private int fatherID;
        private int orderID;
        private string menuName = string.Empty;
        private int menuImage;
        private string uRL = string.Empty;
        private DateTime date = DateTime.Now;
        private string iP = string.Empty;

        public int ID
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int FatherID
        {
            set { this.fatherID = value; }
            get { return this.fatherID; }
        }
        public int OrderID
        {
            set { this.orderID = value; }
            get { return this.orderID; }
        }
        public string MenuName
        {
            set { this.menuName = value; }
            get { return this.menuName; }
        }
        public int MenuImage
        {
            set { this.menuImage = value; }
            get { return this.menuImage; }
        }
        public string URL
        {
            set { this.uRL = value; }
            get { return this.uRL; }
        }
        public DateTime Date
        {
            set { this.date = value; }
            get { return this.date; }
        }
        public string IP
        {
            set { this.iP = value; }
            get { return this.iP; }
        }
    }
}