using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 投票选项实体模型
    /// </summary>
    public sealed class VoteItemInfo
    {
        private int id;
        private string voteID = string.Empty;
        private string itemName = string.Empty;
        private int voteCount;
        private int orderID;
        private string image = string.Empty;
        private string department = string.Empty;
        private string solution = string.Empty;
        private string point = string.Empty;
        private string coverDepartment = string.Empty;
        private string detail = string.Empty;
        private string exp1 = string.Empty;
        private string exp2 = string.Empty;
        private string exp3 = string.Empty;
        private string exp4 = string.Empty;
        private string exp5 = string.Empty;

        public int ID
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string VoteID
        {
            set { this.voteID = value; }
            get { return this.voteID; }
        }
        public string ItemName
        {
            set { this.itemName = value; }
            get { return this.itemName; }
        }
        public int VoteCount
        {
            set { this.voteCount = value; }
            get { return this.voteCount; }
        }
        public int OrderID
        {
            set { this.orderID = value; }
            get { return this.orderID; }
        }
        public string Image
        {
            set { this.image = value; }
            get { return this.image; }
        }
        public string Department
        {
            set { this.department = value; }
            get { return this.department; }
        }
        public string Solution
        {
            set { this.solution = value; }
            get { return this.solution; }
        }
        public string Point
        {
            set { this.point = value; }
            get { return this.point; }
        }
        public string CoverDepartment
        {
            set { this.coverDepartment = value; }
            get { return this.coverDepartment; }
        }
        public string Detail
        {
            set { this.detail = value; }
            get { return this.detail; }
        }
        /// <summary>
        /// 手机站详情
        /// </summary>
        public string Exp1
        {
            set { this.exp1 = value; }
            get { return this.exp1; }
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public string Exp2
        {
            set { this.exp2 = value; }
            get { return this.exp2; }
        }
        public string Exp3
        {
            set { this.exp3 = value; }
            get { return this.exp3; }
        }
        /// <summary>
        /// 二维码图片路径
        /// </summary>
        public string Exp4
        {
            set { this.exp4 = value; }
            get { return this.exp4; }
        }
        public string Exp5
        {
            set { this.exp5 = value; }
            get { return this.exp5; }
        }
    }
}