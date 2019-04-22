using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 投票选项搜索模型
    /// </summary>
    public sealed class VoteItemSearchInfo
    {
        private string voteID = string.Empty;
        private string itemName = string.Empty;
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
        public string Exp1
        {
            set { this.exp1 = value; }
            get { return this.exp1; }
        }
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

