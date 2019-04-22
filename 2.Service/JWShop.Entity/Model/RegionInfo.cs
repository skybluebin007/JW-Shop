using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 地区实体模型
    /// </summary>
    [Serializable]
    public sealed class RegionInfo
    {
        private int id;
        private int fatherID;
        private int orderID;
        private string regionName = string.Empty;

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
        public string RegionName
        {
            set { this.regionName = value; }
            get { return this.regionName; }
        }
    }
}