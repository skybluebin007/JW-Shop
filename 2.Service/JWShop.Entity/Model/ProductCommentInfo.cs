using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 实体模型
    /// </summary>
    [Serializable]
    public sealed class ProductCommentInfo
    {
        public const string TABLENAME = "ProductComment";

        private int id;
        private int productId;
        private string title = string.Empty;
        private string content = string.Empty;
        private string userIP = string.Empty;
        private DateTime postDate = DateTime.Now;
        private int support;
        private int against;
        private int status;
        private int rank;
        private int replyCount;
        private string adminReplyContent = string.Empty;
        private DateTime adminReplyDate = DateTime.Now;
        private int userId;
        private string userName = string.Empty;
        private int orderId;
        private DateTime buyDate = DateTime.Now;
        private string name = string.Empty;
      
        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string Title
        {
            set { this.title = value; }
            get { return this.title; }
        }
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }
        public string UserIP
        {
            set { this.userIP = value; }
            get { return this.userIP; }
        }
        public DateTime PostDate
        {
            set { this.postDate = value; }
            get { return this.postDate; }
        }
        public int Support
        {
            set { this.support = value; }
            get { return this.support; }
        }
        public int Against
        {
            set { this.against = value; }
            get { return this.against; }
        }
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        public int Rank
        {
            set { this.rank = value; }
            get { return this.rank; }
        }
        public int ReplyCount
        {
            set { this.replyCount = value; }
            get { return this.replyCount; }
        }
        public string AdminReplyContent
        {
            set { this.adminReplyContent = value; }
            get { return this.adminReplyContent; }
        }
        public DateTime AdminReplyDate
        {
            set { this.adminReplyDate = value; }
            get { return this.adminReplyDate; }
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
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public DateTime BuyDate
        {
            set { this.buyDate = value; }
            get { return this.buyDate; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
    }
}