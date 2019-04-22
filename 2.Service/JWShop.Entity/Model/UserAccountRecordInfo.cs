using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户账户记录实体模型
    /// </summary>
    public sealed class UserAccountRecordInfo
    {
        public const string TABLENAME = "UserAccountRecord";

        private int id;
        private int recordType;
        private decimal money;
        private int point;
        private DateTime date = DateTime.Now;
        private string iP = string.Empty;
        private string note = string.Empty;
        private int userId;
        private string userName = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int RecordType
        {
            set { this.recordType = value; }
            get { return this.recordType; }
        }
        public decimal Money
        {
            set { this.money = value; }
            get { return this.money; }
        }
        public int Point
        {
            set { this.point = value; }
            get { return this.point; }
        }
        public DateTime Date
        {
            set { this.date = value; }
            get { return this.date; }
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