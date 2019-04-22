using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IOrderRefund
    {
        int Add(OrderRefundInfo entity);
        void Update(OrderRefundInfo entity);
        void UpdateBatchNo(int id, string batchNo);
        void Comment(int id, string content);
        OrderRefundInfo Read(int id);
        OrderRefundInfo ReadByBatchNo(string batchNo);
        List<OrderRefundInfo> ReadList();
        List<OrderRefundInfo> ReadList(int orderId);
        List<OrderRefundInfo> ReadList(int[] orderIds);
        List<OrderRefundInfo> ReadListByOwnerId(int ownerId);

        List<OrderRefundInfo> SearchList(int currentPage, int pageSize, OrderRefundSearchInfo searchInfo, ref int count);
    }
}