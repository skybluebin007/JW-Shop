using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 拼团活动实体模型
    /// </summary>
    public sealed class GroupBuyInfo
    {
        public const string TABLENAME = "GroupBuy";

        public int Id { get; set; }
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
        /// <summary>
        /// 团长用户名
        /// </summary>
        [Computed]
        public string GroupUserName { get; set; }
        /// <summary>
        /// 团长头像
        /// </summary>
        [Computed]
        public string GroupUserAvatar { get; set; }
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
        /// 拼团订单号 
        /// </summary>
        [Computed]
        public int GroupOrderId { get; set; }
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
        /// 参团记录 
        /// </summary>
        [Computed]
        public List<GroupSignInfo> groupSignList { get; set; }
        /// <summary>
        /// 拼团状态：-1：失败，0：进行中，1：成功
        /// </summary>
        [Computed]  
        public int VirtualStatus { get; set; }     
    }
}
