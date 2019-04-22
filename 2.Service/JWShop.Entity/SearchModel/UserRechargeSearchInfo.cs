using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户充值搜索模型。
    /// </summary>
    public sealed class UserRechargeSearchInfo
    {
        private string number = string.Empty;
        private DateTime startRechargeDate = DateTime.MinValue;
        private DateTime endRechargeDate = DateTime.MinValue;
        private int isFinish = int.MinValue;
        private int inUserId = int.MinValue;

        public string Number
        {
            set { this.number = value; }
            get { return this.number; }
        }
        public DateTime StartRechargeDate
        {
            set { this.startRechargeDate = value; }
            get { return this.startRechargeDate; }
        }
        public DateTime EndRechargeDate
        {
            set { this.endRechargeDate = value; }
            get { return this.endRechargeDate; }
        }
        public int IsFinish
        {
            set { this.isFinish = value; }
            get { return this.isFinish; }
        }
        public int InUserId
        {
            set { this.inUserId = value; }
            get { return this.inUserId; }
        }
    }
}