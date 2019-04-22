using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户留言实体模型
    /// </summary>
    public sealed class UserMessageInfo
    {
        public const string TABLENAME = "UsrMessage";

        private int id;
        private int messageClass;
        private string title = string.Empty;
        private string content = string.Empty;
        private string userIP = string.Empty;
        private DateTime postDate = DateTime.Now;
        private int isHandler;
        private string adminReplyContent = string.Empty;
        private DateTime adminReplyDate = DateTime.Now;
        private int userId;
        private string userName = string.Empty;
        public string Tel { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Birthday { get; set; }
        public string Birthplace { get; set; }
        public string Liveplace { get; set; }
        public string Address { get; set; }
        public int Servedays { get; set; }
        public string Servemode { get; set; }
        public string AddCol1 { get; set; }
        public string AddCol2 { get; set; }
        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int MessageClass
        {
            set { this.messageClass = value; }
            get { return this.messageClass; }
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
        public int IsHandler
        {
            set { this.isHandler = value; }
            get { return this.isHandler; }
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

    }
}