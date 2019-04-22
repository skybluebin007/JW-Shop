using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 投票记录实体模型
    /// </summary>
    public sealed class VoteRecordInfo
    {
        private int id;
        private string voteID = string.Empty;
        private string itemID = string.Empty;
        private string userIP = string.Empty;
        private DateTime addDate = DateTime.Now;
        private int userID;
        private string userName = string.Empty;

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
        public string ItemID
        {
            set { this.itemID = value; }
            get { return this.itemID; }
        }
        public string UserIP
        {
            set { this.userIP = value; }
            get { return this.userIP; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public int UserID
        {
            set { this.userID = value; }
            get { return this.userID; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }
    }
}