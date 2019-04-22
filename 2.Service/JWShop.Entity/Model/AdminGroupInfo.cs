using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 管理组实体模型
    /// </summary>
    public sealed class AdminGroupInfo
    {
        public const string TABLENAME = "AdminGroup";

        private int id;
        private string name = string.Empty;
        private string power = string.Empty;
        private int adminCount;
        private DateTime addDate = DateTime.Now;
        private string iP = string.Empty;
        private string note = string.Empty;

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
        public string Power
        {
            set { this.power = value; }
            get { return this.power; }
        }
        public int AdminCount
        {
            set { this.adminCount = value; }
            get { return this.adminCount; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public string IP
        {
            set { this.iP = value; }
            get { return this.iP; }
        }
        public string Note
        {
            set { this.note = value; }
            get { return this.note; }
        }
    }
}