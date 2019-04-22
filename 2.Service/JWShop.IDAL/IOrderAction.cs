using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IOrderAction
    {
        int Add(OrderActionInfo entity);
        void Delete(int[] ids);
        void DeleteByOrderId(int[] orderIds);
        OrderActionInfo Read(int id);
        OrderActionInfo ReadLast(int orderId, int orderStatus);
        List<OrderActionInfo> ReadList(int orderId);
        List<OrderActionInfo> ReadListLastDate(int[] orderIds);
    }
}