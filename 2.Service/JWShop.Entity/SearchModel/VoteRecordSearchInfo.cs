using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 投票记录搜索模型
    /// </summary>
 public sealed   class VoteRecordSearchInfo
    {
        private string voteID = string.Empty;
        private string itemID = string.Empty;
        private string userIP = string.Empty;    
        private int userID=int.MinValue;
        private string userName = string.Empty;
        private DateTime addDate = DateTime.MinValue;
     
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
        public DateTime AddDate
        {
            get { return this.addDate; }
            set { this.addDate = value; }
        }
    }
}
