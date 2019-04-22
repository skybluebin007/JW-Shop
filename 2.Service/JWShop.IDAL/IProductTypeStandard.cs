using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductTypeStandard
    {
        int Add(ProductTypeStandardInfo entity);
        void Update(ProductTypeStandardInfo entity);
        void Delete(int id);
        void Delete(int productTypeId, int[] notInIds);
        void DeleteList(int productTypeId);
        ProductTypeStandardInfo Read(int id);
        ProductTypeStandardInfo Read(string name, int productTypeId);
        List<ProductTypeStandardInfo> ReadList();
        List<ProductTypeStandardInfo> ReadList(int[] ids);
        List<ProductTypeStandardInfo> ReadList(int productTypeId);
    }
}