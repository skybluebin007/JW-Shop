using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户等级实体模型
    /// </summary>
    public sealed class UserGradeInfo
    {
        public const string TABLENAME = "UsrGrade";

        private int id;
        private string name = string.Empty;
        private decimal minMoney;
        private decimal maxMoney;
        private decimal discount;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        /// <summary>
        /// 等级名
        /// </summary>
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        /// <summary>
        /// 最少金额
        /// </summary>
        public decimal MinMoney
        {
            set { this.minMoney = value; }
            get { return this.minMoney; }
        }
        /// <summary>
        /// 最大金额
        /// </summary>
        public decimal MaxMoney
        {
            set { this.maxMoney = value; }
            get { return this.maxMoney; }
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount
        {
            set { this.discount = value; }
            get { return this.discount; }
        }
    }
}