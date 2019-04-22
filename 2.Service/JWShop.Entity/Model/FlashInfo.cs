using System;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 广告位实体模型
    /// </summary>
    [Serializable]
    [Table("Flash")]
    public sealed class FlashInfo
    {
        public const string TABLENAME = "Flash";

        public int Id { get; set; }
        public string Title { get; set; }
        public string Introduce { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime EndDate { get; set; }
        public string AddCol1 { get; set; }
        public string AddCol2 { get; set; }
        public string AddCol3 { get; set; }
    }
}
