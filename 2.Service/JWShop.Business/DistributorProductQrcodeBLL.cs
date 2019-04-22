using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.IDAL;
using JWShop.Entity;
using JWShop.Common;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 分销商商品推广码 业务层
    /// </summary>
  public sealed  class DistributorProductQrcodeBLL
    {
        private static readonly IDistributorProductQrcode dal = FactoryHelper.Instance<IDistributorProductQrcode>(Global.DataProvider, "DistributorProductQrcodeDAL");
        public static bool Add(DistributorProductQrcodeInfo entity)
        {
            return dal.Add(entity);
        }
       public static DistributorProductQrcodeInfo Read(int distributor_Id, int product_Id)
        {
            return dal.Read(distributor_Id, product_Id);
        }
        /// <summary>
        /// 更新二维码
        /// </summary>
       public static bool Update(DistributorProductQrcodeInfo entity)
        {
            return dal.Update(entity);
        }
       public bool Delete(int distributor_Id, int product_Id)
        {
            return dal.Delete(distributor_Id, product_Id);
        }
    }
}
