using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 管理员日志实体模型
    /// </summary>
    public sealed class AdminLogInfo
    {
        public const string TABLENAME = "AdminLog";

        private int id;
        private int groupId;
        private string action = string.Empty;
        private string iP = string.Empty;
        private DateTime addDate = DateTime.Now;
        private int adminId;
        private string adminName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int GroupId
        {
            set { this.groupId = value; }
            get { return this.groupId; }
        }
        public string Action
        {
            set { this.action = value; }
            get { return this.action; }
        }
        public string IP
        {
            set { this.iP = value; }
            get { return this.iP; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public int AdminId
        {
            set { this.adminId = value; }
            get { return this.adminId; }
        }
        public string AdminName
        {
            set { this.adminName = value; }
            get { return this.adminName; }
        }
    }
}