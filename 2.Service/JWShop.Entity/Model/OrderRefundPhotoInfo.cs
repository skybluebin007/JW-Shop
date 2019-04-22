using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 退款单凭证图片实体模型
    /// </summary>
    public sealed class OrderRefundPhotoInfo
    {
        public const string TABLENAME = "OrderRefundPhoto";

        private int id;
        private int orderRefundId;
        private string imageUrl = string.Empty;
        private string remark = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int OrderRefundId
        {
            set { this.orderRefundId = value; }
            get { return this.orderRefundId; }
        }
        public string ImageUrl
        {
            set { this.imageUrl = value; }
            get { return this.imageUrl; }
        }
        public string Remark
        {
            set { this.remark = value; }
            get { return this.remark; }
        }
    }
}