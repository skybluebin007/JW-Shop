using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
namespace JWShop.Page.Mobile
{
   public class ProductPhotos:CommonBasePage
    {
        /// <summary>
        /// 产品
        /// </summary>
        protected ProductInfo product = new ProductInfo();    
        /// <summary>
        /// 产品图片列表
        /// </summary>
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
          /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("ID");
            if (id <= 0)
            {
                ScriptHelper.AlertFront("该产品未上市，不能查看");
            }
            product = ProductBLL.Read(id);
            if (product.IsSale == (int)BoolType.False || product.IsDelete == 1)
            {
                
                    ScriptHelper.Alert("该产品未上市，不能查看");
               
            }
            //产品图片
            ProductPhotoInfo productPhoto = new ProductPhotoInfo();
            productPhoto.Name = product.Name;
            productPhoto.ImageUrl = product.Photo;
            productPhotoList.Add(productPhoto);
            productPhotoList.AddRange(ProductPhotoBLL.ReadList(id, 0));
        }
    }
}
