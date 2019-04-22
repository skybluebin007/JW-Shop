using System;
using Dapper.Contrib.Extensions;
namespace JWShop.Entity
{
    /// <summary>
    /// 退款订单/商品实体模型
    /// </summary>
    public sealed class OrderRefundInfo
    {
        public const string TABLENAME = "OrderRefund";

        private int id;
        private string refundNumber = string.Empty;
        private int orderId;
        private string orderNumber = string.Empty;
        private int orderDetailId;
        private int refundCount;
        private decimal refundBalance;
        private decimal refundMoney;
        private string refundPayKey = string.Empty;
        private string refundPayName = string.Empty;
        private string batchNo = string.Empty;
        private int status;
        private DateTime tmCreate = DateTime.Now;
        private DateTime? tmRefund;
        private string refundRemark = string.Empty;
        private string remark = string.Empty;
        private int isCommented;
        private DateTime? tmComment;
        private string commentContent = string.Empty;
        private int userType;
        private int userId;
        private string userName = string.Empty;
        private int ownerId;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        /// <summary>
        /// 服务工单号
        /// </summary>
        public string RefundNumber
        {
            set { this.refundNumber = value; }
            get { return this.refundNumber; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public string OrderNumber
        {
            set { this.orderNumber = value; }
            get { return this.orderNumber; }
        }
        public int OrderDetailId
        {
            set { this.orderDetailId = value; }
            get { return this.orderDetailId; }
        }
        /// <summary>
        /// 退款商品数量
        /// </summary>
        public int RefundCount
        {
            set { this.refundCount = value; }
            get { return this.refundCount; }
        }
        /// <summary>
        /// 退到账户余额的金额
        /// </summary>
        public decimal RefundBalance
        {
            set { this.refundBalance = value; }
            get { return this.refundBalance; }
        }
        /// <summary>
        /// 退到某种支付方式中的金额
        /// </summary>
        public decimal RefundMoney
        {
            set { this.refundMoney = value; }
            get { return this.refundMoney; }
        }
        /// <summary>
        /// 退款支付方式
        /// </summary>
        public string RefundPayKey
        {
            set { this.refundPayKey = value; }
            get { return this.refundPayKey; }
        }
        public string RefundPayName
        {
            set { this.refundPayName = value; }
            get { return this.refundPayName; }
        }
        /// <summary>
        /// 退款批次号
        /// </summary>
        public string BatchNo
        {
            set { this.batchNo = value; }
            get { return this.batchNo; }
        }
        /// <summary>
        /// 0：退款请求审核不通过；1：客服审核；2：财务审核；10：正在处理退款；11：退款完成
        /// </summary>
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        /// <summary>
        /// 退款记录产生时间
        /// </summary>
        public DateTime TmCreate
        {
            set { this.tmCreate = value; }
            get { return this.tmCreate; }
        }
        /// <summary>
        /// 退款完成时间
        /// </summary>
        public DateTime? TmRefund
        {
            set { this.tmRefund = value; }
            get { return this.tmRefund; }
        }
        /// <summary>
        /// 用户提交的退款理由说明
        /// </summary>
        public string RefundRemark
        {
            set { this.refundRemark = value; }
            get { return this.refundRemark; }
        }
        /// <summary>
        /// 各状态说明
        /// </summary>
        public string Remark
        {
            set { this.remark = value; }
            get { return this.remark; }
        }
        /// <summary>
        /// 是否已对退款服务做出评价
        /// </summary>
        public int IsCommented
        {
            set { this.isCommented = value; }
            get { return this.isCommented; }
        }
        /// <summary>
        /// 用户对退款服务的评价时间
        /// </summary>
        public DateTime? TmComment
        {
            set { this.tmComment = value; }
            get { return this.tmComment; }
        }
        /// <summary>
        /// 用户对退款服务的评价内容
        /// </summary>
        public string CommentContent
        {
            set { this.commentContent = value; }
            get { return this.commentContent; }
        }
        /// <summary>
        /// 申请人类型：1，用户申请；2，管理员代申请
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
        /// <summary>
        /// 退款单所有者（下单用户）
        /// </summary>
        public int OwnerId
        {
            set { this.ownerId = value; }
            get { return this.ownerId; }
        }
        /// <summary>
        /// 退款状态中文描述--虚拟属性
        /// </summary>
        [Computed]
        public string StatusDescription { get; set; }
        /// <summary>
        /// 订单信息--虚拟属性
        /// </summary>
        [Computed]
        public OrderInfo Order { get; set; }
    }
}