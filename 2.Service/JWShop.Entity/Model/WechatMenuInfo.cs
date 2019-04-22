using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 微信自定义菜单实体模型
    /// </summary>
    public sealed class WechatMenuInfo
    {
        public int Id { get; set; }
        public int FatherId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public int OrderId { get; set; }
    }
}
