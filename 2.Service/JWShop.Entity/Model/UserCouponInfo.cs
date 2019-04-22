using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户优惠券实体模型
    /// </summary>
    [Table("UsrCoupon")]
    public sealed class UserCouponInfo
    {
        private int id;
        private int couponId;
        private int getType;
        private string number = string.Empty;
        private string password = string.Empty;
        private int isUse;
        private int orderId;
        private int userId;
        private string userName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int CouponId
        {
            set { this.couponId = value; }
            get { return this.couponId; }
        }
        public int GetType
        {
            set { this.getType = value; }
            get { return this.getType; }
        }
        public string Number
        {
            set { this.number = value; }
            get { return this.number; }
        }
        public string Password
        {
            set { this.password = value; }
            get { return this.password; }
        }
        public int IsUse
        {
            set { this.isUse = value; }
            get { return this.isUse; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
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