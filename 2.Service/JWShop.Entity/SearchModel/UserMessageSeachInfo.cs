using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户留言搜索模型。
    /// </summary>
    public sealed class UserMessageSeachInfo
    {
        private int messageClass = int.MinValue;
        private string title = string.Empty;
        private DateTime startPostDate = DateTime.MinValue;
        private DateTime endPostDate = DateTime.MinValue;
        private int isHandler = int.MinValue;
        private int userId = int.MinValue;
        private string userName = string.Empty;
        protected string inMessageClass = string.Empty;

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
        public DateTime StartPostDate
        {
            set { this.startPostDate = value; }
            get { return this.startPostDate; }
        }
        public DateTime EndPostDate
        {
            set { this.endPostDate = value; }
            get { return this.endPostDate; }
        }
        public int IsHandler
        {
            set { this.isHandler = value; }
            get { return this.isHandler; }
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

        public string InMessageClass
        {
            set { this.inMessageClass = value; }
            get { return this.inMessageClass; }
        }
    }
}