using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IOrderRefundPhoto
    {
        int Add(OrderRefundPhotoInfo entity);
        void Delete(int id);
        OrderRefundPhotoInfo Read(int id);
        List<OrderRefundPhotoInfo> ReadList(int orderRefundId);
    }
}