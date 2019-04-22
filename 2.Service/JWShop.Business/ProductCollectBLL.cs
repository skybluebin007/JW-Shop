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
    public sealed class ProductCollectBLL : BaseBLL
    {
        private static readonly IProductCollect dal = FactoryHelper.Instance<IProductCollect>(Global.DataProvider, "ProductCollectDAL");

        public static int Add(ProductCollectInfo entity)
        {
            entity.Id = dal.Add(entity);
            ProductBLL.ChangeProductCollectCount(entity.ProductId, ChangeAction.Plus);
            return entity.Id;
        }

        public static void Delete(int[] ids, int userId)
        {
            foreach (var id in ids)
            {
                var collect = Read(id);
                ProductBLL.ChangeProductCollectCount(collect.ProductId, ChangeAction.Minus);
            }

            dal.Delete(ids, userId);
        }

        public static ProductCollectInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static ProductCollectInfo Read(int productId, int userId)
        {
            return dal.Read(productId, userId);
        }

        public static List<ProductCollectInfo> ReadList(int productId)
        {
            return dal.ReadList(productId);
        }

        public static List<ProductCollectInfo> ReadListByUserId(int userId)
        {
            return dal.ReadListByUserId(userId);
        }

        /// <summary>
        /// 读取产品收藏数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>产品收藏数据列表</returns>
        public static List<ProductCollectInfo> ReadList(int currentPage, int pageSize, ref int count, int userID)
        {
            return dal.ReadList(currentPage, pageSize, ref count, userID);
        }
    }
}