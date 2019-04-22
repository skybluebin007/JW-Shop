using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    public sealed class ProductMemberPriceBLL : BaseBLL
    {
        private static readonly IProductMemberPrice dal = FactoryHelper.Instance<IProductMemberPrice>(Global.DataProvider, "ProductMemberPriceDAL");

        public static int Add(ProductMemberPriceInfo entity)
        {
            return dal.Add(entity);
        }

        public static void DeleteByGradeId(int[] gradeIds)
        {
            dal.DeleteByGradeId(gradeIds);
        }

        public static void DeleteByProductId(int[] productIds)
        {
            dal.DeleteByGradeId(productIds);
        }

        public static List<ProductMemberPriceInfo> ReadList(int productId)
        {
            return dal.ReadList(productId);
        }

        public static List<ProductMemberPriceInfo> ReadList(int[] productIds)
        {
            return dal.ReadList(productIds);
        }

        public static List<ProductMemberPriceInfo> ReadList(int[] productIds, int gradeId)
        {
            return dal.ReadList(productIds, gradeId);
        }
    }
}