using System;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 图片尺寸模型
    /// </summary>
    [Serializable]
    [Table("PhotoSize")]
    public sealed class PhotoSizeInfo
    {
        public const string TABLENAME = "PhotoSize";

        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }      
        public int Width { get; set; }
        public int Height { get; set; }
        public string Introduce { get; set; }
    }
}
