using System;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    [Serializable]
    [Table("ReceiveMessage")]
    public sealed class ReceiveMessageInfo
    {
        public const string TABLENAME = "ReceiveMessage";

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int IsRead { get; set; }
        public int IsAdmin { get; set; }
        public int FromUserID { get; set; }
        public string FromUserName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    
    }
}
