using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 分销商商品推广码 实体
    /// </summary>
  public sealed  class DistributorProductQrcodeInfo
    {
        public int Product_Id { get; set; }
        public int Distributor_Id { get; set; }
        public string Qrcode { get; set; }
    }
}
