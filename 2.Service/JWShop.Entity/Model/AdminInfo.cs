using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 管理员实体模型
    /// </summary>
    public sealed class AdminInfo
    {
        public const string TABLENAME = "Admin";

        private int id;
        private string name = string.Empty;
        private string email = string.Empty;
        private int groupId;
        private string password = string.Empty;
        private string lastLoginIP = string.Empty;
        private DateTime lastLoginDate = DateTime.Now;
        private int loginTimes;
        private int loginErrorTimes;
        private string noteBook = string.Empty;
        private int isCreate;
        private int status;
        private string safeCode = string.Empty;
        private DateTime findDate = DateTime.Now;
        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string Email
        {
            set { this.email = value; }
            get { return this.email; }
        }
        public int GroupId
        {
            set { this.groupId = value; }
            get { return this.groupId; }
        }
        public string Password
        {
            set { this.password = value; }
            get { return this.password; }
        }
        public string LastLoginIP
        {
            set { this.lastLoginIP = value; }
            get { return this.lastLoginIP; }
        }
        public DateTime LastLoginDate
        {
            set { this.lastLoginDate = value; }
            get { return this.lastLoginDate; }
        }
        public int LoginTimes
        {
            set { this.loginTimes = value; }
            get { return this.loginTimes; }
        }
        public int LoginErrorTimes
        {
            set { this.loginErrorTimes = value; }
            get { return this.loginErrorTimes; }
        }
        public string NoteBook
        {
            set { this.noteBook = value; }
            get { return this.noteBook; }
        }
        public int IsCreate
        {
            set { this.isCreate = value; }
            get { return this.isCreate; }
        }
        /// <summary>
        /// 1：正常；0：冻结
        /// </summary>
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        public string SafeCode
        {
            set { this.safeCode = value; }
            get { return this.safeCode; }
        }
        public DateTime FindDate
        {
            set { this.findDate = value; }
            get { return this.findDate; }
        }
    }
}