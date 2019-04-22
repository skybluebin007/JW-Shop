using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 邮件发送记录实体模型
    /// </summary>
    public sealed class EmailSendRecordInfo
    {
        private int id;
        private string title = string.Empty;
        private string content = string.Empty;
        private int isSystem;
        private string emailList = string.Empty;
        private string openEmailList = string.Empty;
        private int isStatisticsOpendEmail;
        private int sendStatus;
        private string note = string.Empty;
        private DateTime addDate = DateTime.Now;
        private DateTime sendDate = DateTime.Now;

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
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }
        public int IsSystem
        {
            set { this.isSystem = value; }
            get { return this.isSystem; }
        }
        public string EmailList
        {
            set { this.emailList = value; }
            get { return this.emailList; }
        }
        public string OpenEmailList
        {
            set { this.openEmailList = value; }
            get { return this.openEmailList; }
        }
        public int IsStatisticsOpendEmail
        {
            set { this.isStatisticsOpendEmail = value; }
            get { return this.isStatisticsOpendEmail; }
        }
        public int SendStatus
        {
            set { this.sendStatus = value; }
            get { return this.sendStatus; }
        }
        public string Note
        {
            set { this.note = value; }
            get { return this.note; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public DateTime SendDate
        {
            set { this.sendDate = value; }
            get { return this.sendDate; }
        }
    }
}