using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品类型属性记录实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductTypeAttributeRecordInfo
    {
        public const string TABLENAME = "ProductTypeAttributeRecord";

        private int productId;
        private int attributeId;
        private string value = string.Empty;
        //use for join table
        private string attributeName = string.Empty;

        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public int AttributeId
        {
            set { this.attributeId = value; }
            get { return this.attributeId; }
        }
        public string Value
        {
            set { this.value = value; }
            get { return this.value; }
        }
        public string AttributeName
        {
            set { this.attributeName = value; }
            get { return this.attributeName; }
        }
    }
}