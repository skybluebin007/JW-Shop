using System;
using System.Collections.Generic;

namespace JWShop.Entity
{
    /// <summary>
    /// 购物车实体模型
    /// </summary>
    public sealed class CartInfo
    {
        public const string TABLENAME = "Cart";

        private int id;
        private int productId;
        private string productName = string.Empty;
        private int buyCount;
        private string randNumber = string.Empty;
        private int userId;
        private string userName = string.Empty;
        private string standardValueList = string.Empty;

        private decimal price;
        private int leftStorageCount;
        private ProductInfo product = new ProductInfo();
        private List<ProductTypeStandardInfo> standards = new List<ProductTypeStandardInfo>();

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string ProductName
        {
            set { this.productName = value; }
            get { return this.productName; }
        }
        public int BuyCount
        {
            set { this.buyCount = value; }
            get { return this.buyCount; }
        }
        public string RandNumber
        {
            set { this.randNumber = value; }
            get { return this.randNumber; }
        }
        public int UserId
        {
            set { this.userId = value; }
            get { return this.userId; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }
        public string StandardValueList
        {
            set { this.standardValueList = value; }
            get { return this.standardValueList; }
        }

        public decimal Price
        {
            set { this.price = value; }
            get { return this.price; }
        }
        public int LeftStorageCount
        {
            set { this.leftStorageCount = value; }
            get { return this.leftStorageCount; }
        }
        public ProductInfo Product
        {
            set { this.product = value; }
            get { return this.product; }
        }
        public List<ProductTypeStandardInfo> Standards
        {
            set { this.standards = value; }
            get { return this.standards; }
        }
    }
}