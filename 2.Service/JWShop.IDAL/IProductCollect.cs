using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductCollect
    {
        int Add(ProductCollectInfo entity);
        void Delete(int[] ids, int userId);
        ProductCollectInfo Read(int id);
        ProductCollectInfo Read(int productId, int userId);
        List<ProductCollectInfo> ReadList(int productId);
        List<ProductCollectInfo> ReadListByUserId(int userId);

        /// <summary>
        /// 获得产品收藏数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>产品收藏数据列表</returns>
        List<ProductCollectInfo> ReadList(int currentPage, int pageSize, ref int count, int userID);
    }
}