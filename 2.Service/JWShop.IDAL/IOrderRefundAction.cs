using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IOrderRefundAction
    {
        int Add(OrderRefundActionInfo entity);
        List<OrderRefundActionInfo> ReadList(int orderRefundId);
    }
}