using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class ProductCommentBLL : BaseBLL
    {
        private static readonly IProductComment dal = FactoryHelper.Instance<IProductComment>(Global.DataProvider, "ProductCommentDAL");

        public static int Add(ProductCommentInfo entity)
        {
            entity.Id = dal.Add(entity);
            if (entity.Status == (int)CommentStatus.Show)
            {
                ProductBLL.ChangeProductCommentCountAndRank(entity.ProductId, entity.Rank, ChangeAction.Plus);
            }
            return entity.Id;
        }

        public static void Update(ProductCommentInfo entity, int oldStatus)
        {
            dal.Update(entity);

            if (oldStatus != entity.Status)
            {
                if (entity.Status == (int)CommentStatus.Show)
                {
                    ProductBLL.ChangeProductCommentCountAndRank(entity.ProductId, entity.Rank, ChangeAction.Plus);
                }
                else
                {
                    ProductBLL.ChangeProductCommentCountAndRank(entity.ProductId, entity.Rank, ChangeAction.Minus);
                }
            }
        }

        public static void Delete(int id)
        {
            var comment = Read(id);
            if (comment.Status == (int)CommentStatus.Show)
            {
                ProductBLL.ChangeProductCommentCountAndRank(comment.ProductId, comment.Rank, ChangeAction.Minus);
            }

            dal.Delete(id);
        }

        public static void Delete(int[] ids)
        {
            foreach (var id in ids)
            {
                var comment = Read(id);
                if (comment.Status == (int)CommentStatus.Show)
                {
                    ProductBLL.ChangeProductCommentCountAndRank(comment.ProductId, comment.Rank, ChangeAction.Minus);
                }
            }

            dal.Delete(ids);
        }

        public static ProductCommentInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<ProductCommentInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<ProductCommentInfo> SearchList(int currentPage, int pageSize, ProductCommentSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }       
        /// <summary>
        /// 搜索产品评论数据列表
        /// </summary>
        /// <param name="productComment">ProductCommentSearchInfo模型变量</param>
        /// <returns>产品评论数据列表</returns>
        public static List<ProductCommentInfo> SearchProductCommentList(ProductCommentSearchInfo productComment)
        {
            return dal.SearchProductCommentList(productComment);
        }

        public static List<ProductCommentInfo> SearchInnerProductList(int currentPage, int pageSize, ProductCommentSearchInfo searchInfo, ref int count)
        {
            return dal.SearchInnerProductList(currentPage, pageSize, searchInfo, ref count);
        }

        public static bool HasCommented(int orderId, int userId)
        {
            return dal.HasCommented(orderId, userId);
        }

        public static void ChangeStatus(int[] ids, int status)
        {
            foreach (var id in ids)
            {
                var comment = Read(id);
                if (status == (int)CommentStatus.Show)
                {
                    if (comment.Status != (int)CommentStatus.Show)
                        ProductBLL.ChangeProductCommentCountAndRank(comment.ProductId, comment.Rank, ChangeAction.Plus);
                }
                else
                {
                    if (comment.Status == (int)CommentStatus.Show)
                        ProductBLL.ChangeProductCommentCountAndRank(comment.ProductId, comment.Rank, ChangeAction.Minus);
                }
            }

            dal.ChangeStatus(ids, status);
        }

    }
}