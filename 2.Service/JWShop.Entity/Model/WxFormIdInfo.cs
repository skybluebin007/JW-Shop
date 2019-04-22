using System;


namespace JWShop.Entity
{
    /// <summary>
    /// 微信小程序 fromid 模型
    /// </summary>
    public sealed class WxFormIdInfo
    {
        public const string TABLENAME = "WxFormId";

        public int Id { get; set; }
        public string FormId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 是否使用：0--未使用；1--已使用
        /// </summary>
        public int Used { get; set; }
        /// <summary>
        /// 添加日期（小程序规定：formid7天内有效）
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
