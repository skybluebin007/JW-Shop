using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductComment
    {
        int Add(ProductCommentInfo entity);
        void Update(ProductCommentInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        ProductCommentInfo Read(int id);
        List<ProductCommentInfo> ReadList();
        List<ProductCommentInfo> SearchList(int currentPage, int pageSize, ProductCommentSearchInfo searchInfo, ref int count);
        List<ProductCommentInfo> SearchInnerProductList(int currentPage, int pageSize, ProductCommentSearchInfo searchInfo, ref int count);

        /// <summary>
        /// 搜索产品评论数据列表
        /// </summary>
        /// <param name="productComment">ProductCommentSearchInfo模型变量</param>
        /// <returns>产品评论数据列表</returns>
        List<ProductCommentInfo> SearchProductCommentList(ProductCommentSearchInfo productComment);

        bool HasCommented(int orderId, int userId);
        void ChangeStatus(int[] ids, int status);
    }
}