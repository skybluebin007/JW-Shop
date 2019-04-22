using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品类型规格实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductTypeStandardInfo
    {
        public const string TABLENAME = "ProductTypeStandard";

        private int id;
        private int productTypeId;
        private string name = string.Empty;
        private string valueList = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ProductTypeId
        {
            set { this.productTypeId = value; }
            get { return this.productTypeId; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string ValueList
        {
            set { this.valueList = value; }
            get { return this.valueList; }
        }
    }
}