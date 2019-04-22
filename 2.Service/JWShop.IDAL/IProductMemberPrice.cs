using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductMemberPrice
    {
        int Add(ProductMemberPriceInfo entity);
        void DeleteByGradeId(int[] gradeIds);
        void DeleteByProductId(int[] productIds);

        List<ProductMemberPriceInfo> ReadList(int productId);
        List<ProductMemberPriceInfo> ReadList(int[] productIds);
        List<ProductMemberPriceInfo> ReadList(int[] productIds, int gradeId);
    }
}