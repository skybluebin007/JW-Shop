using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IPointProductOrder
    {
        int Add(PointProductOrderInfo entity);
        void Update(PointProductOrderInfo entity);
        void Delete(int id);
        PointProductOrderInfo Read(int id);
        List<PointProductOrderInfo> ReadList(int userId);
        List<PointProductOrderInfo> SearchList(int currentPage, int pageSize, PointProductOrderSearchInfo searchInfo, ref int count);
    }
}