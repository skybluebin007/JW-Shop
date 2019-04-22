using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 物流区域实体模型
    /// </summary>
    public sealed class ShippingRegionInfo
    {
        public const string TABLENAME = "ShippingRegion";

        private int id;
        private string name = string.Empty;
        private int shippingId;
        private string regionId = string.Empty;
        private decimal fixedMoeny;
        private decimal firstMoney;
        private decimal againMoney;
        private decimal oneMoeny;
        private decimal anotherMoeny;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public int ShippingId
        {
            set { this.shippingId = value; }
            get { return this.shippingId; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public decimal FixedMoeny
        {
            set { this.fixedMoeny = value; }
            get { return this.fixedMoeny; }
        }
        public decimal FirstMoney
        {
            set { this.firstMoney = value; }
            get { return this.firstMoney; }
        }
        public decimal AgainMoney
        {
            set { this.againMoney = value; }
            get { return this.againMoney; }
        }
        public decimal OneMoeny
        {
            set { this.oneMoeny = value; }
            get { return this.oneMoeny; }
        }
        public decimal AnotherMoeny
        {
            set { this.anotherMoeny = value; }
            get { return this.anotherMoeny; }
        }
    }
}