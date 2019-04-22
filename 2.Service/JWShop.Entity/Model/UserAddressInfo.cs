using System;
using System.Collections.Generic;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户收货地址实体模型
    /// </summary>
    public sealed class UserAddressInfo
    {
        public const string TABLENAME = "UserAddress";

        private int id;
        private string consignee = string.Empty;
        private string email = string.Empty;
        private string regionId = string.Empty;
        private string address = string.Empty;
        private string zipCode = string.Empty;
        private string tel = string.Empty;
        private string mobile = string.Empty;
        private int isDefault;
        private int userId;
        private string userName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Consignee
        {
            set { this.consignee = value; }
            get { return this.consignee; }
        }
        public string Email
        {
            set { this.email = value; }
            get { return this.email; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public string Address
        {
            set { this.address = value; }
            get { return this.address; }
        }
        public string ZipCode
        {
            set { this.zipCode = value; }
            get { return this.zipCode; }
        }
        public string Tel
        {
            set { this.tel = value; }
            get { return this.tel; }
        }
        public string Mobile
        {
            set { this.mobile = value; }
            get { return this.mobile; }
        }
        public int IsDefault
        {
            set { this.isDefault = value; }
            get { return this.isDefault; }
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

    }
}