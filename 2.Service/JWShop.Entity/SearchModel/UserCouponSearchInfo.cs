using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户优惠券搜索模型。
    /// </summary>
    public sealed class UserCouponSearchInfo
    {
        private int couponId = int.MinValue;
        private int getType = int.MinValue;
        private string number = string.Empty;
        private int isUse = int.MinValue;
        private int userId = int.MinValue;
        public int IsTimeOut = int.MinValue;
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
        public int IsUse
        {
            set { this.isUse = value; }
            get { return this.isUse; }
        }
        public int UserId
        {
            set { this.userId = value; }
            get { return this.userId; }
        }
    }
}