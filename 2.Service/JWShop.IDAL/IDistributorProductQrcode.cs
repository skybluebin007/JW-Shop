using System;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 分销商商品推广码 接口层 
    /// </summary>
   public  interface IDistributorProductQrcode
    {
        bool Add(DistributorProductQrcodeInfo entity);
        DistributorProductQrcodeInfo Read(int distributor_Id, int product_Id);
        /// <summary>
        /// 更新二维码
        /// </summary>
        bool Update(DistributorProductQrcodeInfo entity);
        bool Delete(int distributor_Id, int product_Id);
    }
}
