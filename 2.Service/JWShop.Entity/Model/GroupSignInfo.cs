using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 参团记录模型
    /// </summary>
 public sealed   class GroupSignInfo
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 参团的订单Id
        /// </summary>
        public int OrderId { get; set; }
       /// <summary>
       /// 参团时间
       /// </summary>
        public DateTime SignTime { get; set; }
        /// <summary>
        /// 参团者姓名
        /// </summary>
        [Computed]
        public string UserName { get; set; }
        /// <summary>
        /// 参团者头像
        /// </summary>
        [Computed]
        public string UserAvatar { get; set; }
        /// <summary>
        /// 团长ID
        /// </summary>
        [Computed]
        public string Leader { get; set; }
        /// <summary>
        /// 开团时间
        /// </summary>
        [Computed]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Computed]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 团购人数
        /// </summary>
        [Computed]
        public int Quantity { get; set; }
        /// <summary>
        /// 参团人数
        /// </summary>
        [Computed]
        public int SignCount { get; set; }
        /// <summary>
        /// 拼团订单编号
        /// </summary>
        [Computed]
        public string GroupOrderNumber { get; set; }
        /// <summary>
        /// 拼团订单状态[orderstatus]
        /// </summary>
        [Computed]
        public int GroupOrderStatus { get; set; }
        /// <summary>
        /// 拼团订单是否退款（1-退款，0-未退款）
        /// </summary>
        [Computed]
        public int IsRefund { get; set; }
        /// <summary>
        /// 拼团商品Id
        /// </summary>
        [Computed]
        public int ProductId { get; set; }
        /// <summary>
        /// 拼团商品名称（含规格名称）
        /// </summary>
        [Computed]
        public string ProductName { get; set; }
        /// <summary>
        /// 拼团价格
        /// </summary>
        [Computed]
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 拼团商品图片
        /// </summary>
        [Computed]
        public string ProductPhoto { get; set; }
        /// <summary>
        /// 拼团状态：-1：失败，0：进行中，1：成功
        /// </summary>
        [Computed]
        public int VirtualStatus { get; set; }

    }
}
