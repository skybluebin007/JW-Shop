using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品类型属性实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductTypeAttributeInfo
    {
        public const string TABLENAME = "ProductTypeAttribute";

        private int id;
        private int productTypeId;
        private string name = string.Empty;
        private int inputType;
        private string inputValue = string.Empty;
        private int orderId;
        private ProductTypeAttributeRecordInfo attributeRecord = new ProductTypeAttributeRecordInfo();

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
        public int InputType
        {
            set { this.inputType = value; }
            get { return this.inputType; }
        }
        public string InputValue
        {
            set { this.inputValue = value; }
            get { return this.inputValue; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public ProductTypeAttributeRecordInfo AttributeRecord
        {
            set { this.attributeRecord = value; }
            get { return this.attributeRecord; }
        }
    }
}