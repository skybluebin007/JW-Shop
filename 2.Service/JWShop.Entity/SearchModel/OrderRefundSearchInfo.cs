using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 退款订单/商品 搜索模型。
    /// </summary>
    public sealed class OrderRefundSearchInfo
    {
        private string refundNumber = string.Empty;
        private string orderNumber = string.Empty;
        private int status = int.MinValue;
        private DateTime startTmCreate = DateTime.MinValue;
        private DateTime endTmCreate = DateTime.MinValue;
        private int ownerId = int.MinValue;

        public string RefundNumber
        {
            set { this.refundNumber = value; }
            get { return this.refundNumber; }
        }
        public string OrderNumber
        {
            set { this.orderNumber = value; }
            get { return this.orderNumber; }
        }
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        public DateTime StartTmCreate
        {
            set { this.startTmCreate = value; }
            get { return this.startTmCreate; }
        }
        public DateTime EndTmCreate
        {
            set { this.endTmCreate = value; }
            get { return this.endTmCreate; }
        }
        public int OwnerId
        {
            set { this.ownerId = value; }
            get { return this.ownerId; }
        }
    }
}