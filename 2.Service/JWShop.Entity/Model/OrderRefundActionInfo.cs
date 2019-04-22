using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 退款订单/商品操作实体模型
    /// </summary>
    public sealed class OrderRefundActionInfo
    {
        public const string TABLENAME = "OrderRefundAction";

        private int id;
        private int orderRefundId;
        private int status;
        private string remark = string.Empty;
        private DateTime tm = DateTime.Now;
        private int userType;
        private int userId;
        private string userName = string.Empty;

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
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
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
        /// <summary>
        /// 处理人类型：1，普通用户；2，管理员
        /// </summary>
        public int UserType
        {
            set { this.userType = value; }
            get { return this.userType; }
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