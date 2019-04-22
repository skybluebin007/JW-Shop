using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 投票实体模型
    /// </summary>
    public sealed class VoteInfo
    {
        private int id;
        private string title = string.Empty;
        private int itemCount;
        private int voteType;
        private string note = string.Empty;
        private int fatherID;
        private int orderID;
        public int ID
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Title
        {
            set { this.title = value; }
            get { return this.title; }
        }
        public int ItemCount
        {
            set { this.itemCount = value; }
            get { return this.itemCount; }
        }
        public int VoteType
        {
            set { this.voteType = value; }
            get { return this.voteType; }
        }
        public string Note
        {
            set { this.note = value; }
            get { return this.note; }
        }
        public int FatherID
        {
            get { return this.fatherID; }
            set { this.fatherID = value; }
        }
        public int OrderID
        {
            get { return this.orderID; }
            set { this.orderID = value; }
        }
    }
}