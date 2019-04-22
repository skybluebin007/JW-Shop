using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IOrder
    {
        int Add(OrderInfo entity);
        void Update(OrderInfo entity);
        void UpdateIsNoticed(int orderid, int isNoticed);
        void UpdateIsNoticed(int[] orderids, int isNoticed);
        void Delete(int id);
        void Delete(int[] ids, int userId);
        OrderInfo Read(int id);
        OrderInfo Read(int id, int userId);
        OrderInfo ReadByShopId(int id, int shopId);
        OrderInfo Read(string orderNumber, int userId);
        OrderInfo Read(string orderNumber);
        List<OrderInfo> ReadList();
        List<OrderInfo> ReadList(int[] ids);
        List<OrderInfo> ReadList(int[] ids, int userId);
        int ReadCount(int userId);
        /// <summary>
        /// 待付款订单失效检查
        /// </summary>
        /// <param name="ids"></param>
        void CheckOrderPayTime(int[] ids);

        List<OrderInfo> SearchList(OrderSearchInfo searchInfo);
        List<OrderInfo> SearchList(int currentPage, int pageSize, OrderSearchInfo searchInfo, ref int count);
        /// <summary>
        /// 搜索拼团订单
        /// </summary>     
        List<OrderInfo> SearchGroupOrderList(int currentPage, int pageSize, OrderSearchInfo searchInfo, ref int count);
        /// <summary>
        /// 统计订单状态
        /// </summary>
        DataTable StatisticsOrderStatus(OrderSearchInfo orderSearch);
        /// <summary>
        /// 统计订单数量
        /// </summary>
        DataTable StatisticsOrderCount(OrderSearchInfo orderSearch, DateType dateType);
        /// <summary>
        /// 统计订单区域
        /// </summary>
        DataTable StatisticsOrderArea(OrderSearchInfo orderSearch);
        /// <summary>
        /// 统计销售汇总
        /// </summary>
        DataTable StatisticsSaleTotal(OrderSearchInfo orderSearch, DateType dateType);
        /// <summary>
        /// 统计销售汇总
        /// </summary>
        DataTable StatisticsAllTotal(OrderSearchInfo orderSearch);
        /// <summary>
        /// 统计已付款汇总
        /// </summary>
        /// <param name="orderSearch"></param>
        /// <returns></returns>
        DataTable StatisticsPaidTotal(OrderSearchInfo orderSearch);
        /// <summary>
        /// 统计滞销分析
        /// </summary>
        DataTable StatisticsSaleStop(string productIds);

        /// <summary>
        /// 读取订单的上一个，下一个
        /// </summary>
        List<OrderInfo> ReadPreNextOrder(int id);
        /// <summary>
        /// 订单超时失效检查--所有订单（存储过程）
        /// </summary>
        void CheckOrderPayTimeProg();
         /// <summary>
        /// 待付款订单失效检查--指定用户订单--存储过程
        /// </summary>       
        void CheckOrderPayTimeProg(int userId);
        /// <summary>
        /// 待收货订单自动收货--所有订单--存储过程
        /// </summary>       
        void CheckOrderRecieveTimeProg();

        /// <summary>
        /// 待收货订单自动收货--指定订单--存储过程
        /// </summary>       
        void CheckOrderRecieveTimeProg(int userId);
        /// <summary>
        /// 启用不限库存后，获取商品单日销量
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        int GetProductOrderCountDaily(int productId, int productStandType, DateTime date, string standardValueList = null);
    }
}