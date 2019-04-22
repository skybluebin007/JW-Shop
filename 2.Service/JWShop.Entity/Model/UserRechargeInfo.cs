using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户充值实体模型
    /// </summary>
    public sealed class UserRechargeInfo
    {
        public const string TABLENAME = "UserRecharge";

        private int id;
        private string number = string.Empty;
        private decimal money;
        private string payKey = string.Empty;
        private string payName = string.Empty;
        private DateTime rechargeDate = DateTime.Now;
        private string rechargeIP = string.Empty;
        private int isFinish;
        private int userId;
        private string userName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Number
        {
            set { this.number = value; }
            get { return this.number; }
        }
        public decimal Money
        {
            set { this.money = value; }
            get { return this.money; }
        }
        public string PayKey
        {
            set { this.payKey = value; }
            get { return this.payKey; }
        }
        public string PayName
        {
            set { this.payName = value; }
            get { return this.payName; }
        }
        public DateTime RechargeDate
        {
            set { this.rechargeDate = value; }
            get { return this.rechargeDate; }
        }
        public string RechargeIP
        {
            set { this.rechargeIP = value; }
            get { return this.rechargeIP; }
        }
        public int IsFinish
        {
            set { this.isFinish = value; }
            get { return this.isFinish; }
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