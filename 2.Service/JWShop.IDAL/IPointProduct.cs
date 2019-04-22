using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IPointProduct
    {
        int Add(PointProductInfo entity);
        void Update(PointProductInfo entity);
        void Delete(int id);
        PointProductInfo Read(int id);
        List<PointProductInfo> ReadList();
        List<PointProductInfo> ReadList(int[] ids);
        List<PointProductInfo> SearchList(int currentPage, int pageSize, PointProductSearchInfo searchInfo, ref int count);

        void OffSale(int[] ids);
        void OnSale(int[] ids);
        void ChangeSendCount(int id, ChangeAction action);
    }
}