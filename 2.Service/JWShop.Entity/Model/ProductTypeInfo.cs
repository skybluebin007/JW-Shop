using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品类型实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductTypeInfo
    {
        public const string TABLENAME = "ProductType";

        private int id;
        private string name = string.Empty;
        private string spelling = string.Empty;
        private string brandIds = string.Empty;

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
        public string Spelling
        {
            set { this.spelling = value; }
            get { return this.spelling; }
        }
        public string BrandIds
        {
            set { this.brandIds = value; }
            get { return this.brandIds; }
        }
    }
}