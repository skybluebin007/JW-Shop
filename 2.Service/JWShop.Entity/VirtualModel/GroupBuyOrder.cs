﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 拼团活动实体 & 订单实体  虚拟模型
    /// </summary>
  public sealed  class GroupBuyOrder
    {
        private int id;
        private int shopId;
        private string orderNumber = string.Empty;
        private int isActivity;
        private int orderStatus;
        private string orderNote = string.Empty;
        private decimal productMoney;
        private decimal balance;
        private decimal favorableMoney;
        private decimal otherMoney;
        private decimal couponMoney;
        private int point;
        private decimal pointMoney;
        private string consignee = string.Empty;
        private string regionId = string.Empty;
        private string address = string.Empty;
        private string zipCode = string.Empty;
        private string tel = string.Empty;
        private string email = string.Empty;
        private string mobile = string.Empty;
        private int shippingId;
        private DateTime shippingDate = DateTime.Now;
        private string shippingNumber = string.Empty;
        private decimal shippingMoney;
        private string payKey = string.Empty;
        private string payName = string.Empty;
        private DateTime payDate = DateTime.Now;
        private int isRefund;
        private int favorableActivityId;
        private int giftId;
        private string invoiceTitle = string.Empty;
        private string invoiceContent = string.Empty;
        private string userMessage = string.Empty;
        private DateTime addDate = DateTime.Now;
        private string iP = string.Empty;
        private int userId;
        private string userName = string.Empty;
        private int isDelete;
        private int activityPoint;
        private string aliPayTradeNo = string.Empty;
        private string wxPayTradeNo = string.Empty;
        private string giftMessige = string.Empty;
        private string addCol1 = string.Empty;
        private string addCol2 = string.Empty;

        public int IsNoticed { set; get; }
        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ShopId
        {
            set { this.shopId = value; }
            get { return this.shopId; }
        }
        public string OrderNumber
        {
            set { this.orderNumber = value; }
            get { return this.orderNumber; }
        }
        public int IsActivity
        {
            set { this.isActivity = value; }
            get { return this.isActivity; }
        }
        public int OrderStatus
        {
            set { this.orderStatus = value; }
            get { return this.orderStatus; }
        }
        public string OrderNote
        {
            set { this.orderNote = value; }
            get { return this.orderNote; }
        }
        public decimal ProductMoney
        {
            set { this.productMoney = value; }
            get { return this.productMoney; }
        }
        public decimal Balance
        {
            set { this.balance = value; }
            get { return this.balance; }
        }
        public decimal FavorableMoney
        {
            set { this.favorableMoney = value; }
            get { return this.favorableMoney; }
        }
        public decimal OtherMoney
        {
            set { this.otherMoney = value; }
            get { return this.otherMoney; }
        }
        public decimal CouponMoney
        {
            set { this.couponMoney = value; }
            get { return this.couponMoney; }
        }
        public int Point
        {
            set { this.point = value; }
            get { return this.point; }
        }
        public decimal PointMoney
        {
            set { this.pointMoney = value; }
            get { return this.pointMoney; }
        }
        public string Consignee
        {
            set { this.consignee = value; }
            get { return this.consignee; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public string Address
        {
            set { this.address = value; }
            get { return this.address; }
        }
        public string ZipCode
        {
            set { this.zipCode = value; }
            get { return this.zipCode; }
        }
        public string Tel
        {
            set { this.tel = value; }
            get { return this.tel; }
        }
        public string Email
        {
            set { this.email = value; }
            get { return this.email; }
        }
        public string Mobile
        {
            set { this.mobile = value; }
            get { return this.mobile; }
        }
        public int ShippingId
        {
            set { this.shippingId = value; }
            get { return this.shippingId; }
        }
        public DateTime ShippingDate
        {
            set { this.shippingDate = value; }
            get { return this.shippingDate; }
        }
        public string ShippingNumber
        {
            set { this.shippingNumber = value; }
            get { return this.shippingNumber; }
        }
        public decimal ShippingMoney
        {
            set { this.shippingMoney = value; }
            get { return this.shippingMoney; }
        }
        public string PayKey
        {
            set { this.payKey = value; }
            get { return this.payKey; }
        }
        public string PayName
        {
            set { this.payName = value; }
            get { return this.payName; }
        }
        public DateTime PayDate
        {
            set { this.payDate = value; }
            get { return this.payDate; }
        }
        public int IsRefund
        {
            set { this.isRefund = value; }
            get { return this.isRefund; }
        }
        /// <summary>
        /// 团购活动Id,对应GroupBuy表的Id
        /// </summary>
        public int FavorableActivityId
        {
            set { this.favorableActivityId = value; }
            get { return this.favorableActivityId; }
        }
        public int GiftId
        {
            set { this.giftId = value; }
            get { return this.giftId; }
        }
        public string InvoiceTitle
        {
            set { this.invoiceTitle = value; }
            get { return this.invoiceTitle; }
        }
        public string InvoiceContent
        {
            set { this.invoiceContent = value; }
            get { return this.invoiceContent; }
        }
        public string UserMessage
        {
            set { this.userMessage = value; }
            get { return this.userMessage; }
        }
        public DateTime AddDate
        {
            set { this.addDate = value; }
            get { return this.addDate; }
        }
        public string IP
        {
            set { this.iP = value; }
            get { return this.iP; }
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
        public int IsDelete
        {
            set { this.isDelete = value; }
            get { return this.isDelete; }
        }
        /// <summary>
        /// 积分兑换时，花费的积分
        /// </summary>
        public int ActivityPoint
        {
            set { this.activityPoint = value; }
            get { return this.activityPoint; }
        }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string AliPayTradeNo
        {
            set { this.aliPayTradeNo = value; }
            get { return this.aliPayTradeNo; }
        }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string WxPayTradeNo
        {
            set { this.wxPayTradeNo = value; }
            get { return this.wxPayTradeNo; }
        }
        public string GiftMessige
        {
            set { this.giftMessige = value; }
            get { return this.giftMessige; }
        }
        public string AddCol1
        {
            set { this.addCol1 = value; }
            get { return this.addCol1; }
        }
        public string AddCol2
        {
            set { this.addCol2 = value; }
            get { return this.addCol2; }
        }
        /// <summary>
        /// 订单详细购买信息列表
        /// </summary>
        [Computed]
        public List<OrderDetailInfo> OrderDetailList
        {
            set; get;
        }
        /// <summary>
        /// 是否有退款处正在理中
        /// </summary>
        [Computed]
        public bool IsRefunding
        {
            get; set;
        }
        /// <summary>
        /// 有效退款记录
        /// </summary>
        [Computed]
        public List<OrderRefundInfo> OrderRefundList { get; set; }
        /// <summary>
        /// 查看退款详情Url
        /// </summary>
        [Computed]
        public string OrderRefundUrl { get; set; }

        /// <summary>
        /// 团长Id（userId）
        /// </summary>
        public int Leader { get; set; }
        /// <summary>
        /// 拼团商品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 团购人数
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 参团人数
        /// </summary>
        public int SignCount { get; set; }

    }
}
