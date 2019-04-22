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
    public sealed class ProductPhotoBLL : BaseBLL
    {
        private static readonly IProductPhoto dal = FactoryHelper.Instance<IProductPhoto>(Global.DataProvider, "ProductPhotoDAL");
        public static readonly int TableID = UploadTable.ReadTableID("ProductPhoto");

        public static int Add(ProductPhotoInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(ProductPhotoInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id, int proStyle)
        {
            dal.Delete(id, proStyle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="proStyle">0代表商品类型,1代表文章类型</param>
        public static void DeleteList(int productId,int proStyle)
        {
            dal.DeleteList(productId, proStyle);
        }

        public static ProductPhotoInfo Read(int id, int proStyle)
        {
            return dal.Read(id, proStyle);
        }

        public static List<ProductPhotoInfo> ReadList(int productId, int proStyle)
        {
            return dal.ReadList(productId, proStyle);
        }
        /// <summary>
        /// 获取所有productphoto
        /// </summary>
        /// <returns></returns>
        public static List<ProductPhotoInfo> ReadAllProductPhotos()
        {
            return dal.ReadAllProductPhotos();
        }
        /// <summary>
        /// 获取当前商品图集的最大排序号
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        public static int GetMaxOrderId(int productId)
        {
            return dal.GetMaxOrderId(productId);
        }
        /// <summary>
        /// 上移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveUpProductPhoto(int id)
        {
            dal.MoveUpProductPhoto(id);
        }


        /// <summary>
        /// 下移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveDownProductPhoto(int id)
        {
            dal.MoveDownProductPhoto(id);
        }

    }
}