using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 邮件内容模型
    /// </summary>
    public class EmailContentInfo
    {
        private string emailTitle = string.Empty;
        private int isSystem;
        private string key = string.Empty;
        private string emailContent = string.Empty;
        private string note = string.Empty;

        public string EmailTitle
        {
            set { this.emailTitle = value; }
            get { return this.emailTitle; }
        }
        public int IsSystem
        {
            set { this.isSystem = value; }
            get { return this.isSystem; }
        }
        public string Key
        {
            set { this.key = value; }
            get { return this.key; }
        }
        public string EmailContent
        {
            set { this.emailContent = value; }
            get { return this.emailContent; }
        }
        public string Note
        {
            set { this.note = value; }
            get { return this.note; }
        }
    }
}
