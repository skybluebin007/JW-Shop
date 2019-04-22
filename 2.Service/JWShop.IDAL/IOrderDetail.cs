using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IOrderDetail
    {
        int Add(OrderDetailInfo entity);
        void Update(OrderDetailInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        void DeleteByOrderId(int[] orderIds);
        OrderDetailInfo Read(int id);
        List<OrderDetailInfo> ReadList(int orderId);
        List<OrderDetailInfo> ReadListByProductId(int productId);

        /// <summary>
        /// 改变购买数量
        /// </summary>
        void ChangeBuyCount(int id, int buyCount);
        /// <summary>
        /// 改变退款数量
        /// </summary>
        void ChangeRefundCount(int id, int refundCount, ChangeAction action);

        /// <summary>
        /// 统计销售流水账
        /// </summary>
        DataTable StatisticsSaleDetail(int currentPage, int pageSize, OrderSearchInfo orderSearch, ProductSearchInfo productSearch, ref int count);

        int GetOrderCount(int productId, string standardValueList);
    }
}