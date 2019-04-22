using System;
using System.Collections.Generic;

namespace JWShop.Entity
{
    /// <summary>
    /// 热门搜索关键词实体模型
    /// </summary>
    public sealed class AdKeywordInfo
    {
        public const string TABLENAME = "AdKeyword";

        private int id;
        private string name = string.Empty;
        private string url = string.Empty;
        private int orderId;
        private string remark = string.Empty;
        private DateTime tm = DateTime.Now;

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
        public string Url
        {
            set { this.url = value; }
            get { return this.url; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public string Remark
        {
            set { this.remark = value; }
            get { return this.remark; }
        }
        public DateTime Tm
        {
            set { this.tm = value; }
            get { return this.tm; }
        }
    }
}