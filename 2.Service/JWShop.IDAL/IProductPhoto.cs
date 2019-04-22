using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductPhoto
    {
        int Add(ProductPhotoInfo entity);
        void Update(ProductPhotoInfo entity);
        void Delete(int id, int proStyle);
        void DeleteList(int productId, int proStyle);
        ProductPhotoInfo Read(int id, int proStyle);
        List<ProductPhotoInfo> ReadList(int productId,int proStyle);
        /// <summary>
        /// 获取所有productphoto
        /// </summary>
        /// <returns></returns>
        List<ProductPhotoInfo> ReadAllProductPhotos();
        /// <summary>
        /// 获取当前商品图集的最大排序号
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
       int GetMaxOrderId(int productId);
                /// <summary>
        /// 上移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
       void MoveUpProductPhoto(int id);
       

        /// <summary>
        /// 下移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
       void MoveDownProductPhoto(int id);
    }
}