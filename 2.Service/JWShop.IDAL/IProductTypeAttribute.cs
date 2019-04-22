using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductTypeAttribute
    {
        int Add(ProductTypeAttributeInfo entity);
        void Update(ProductTypeAttributeInfo entity);
        void Delete(int id);
        void Delete(int productTypeId, int[] notInIds);
        void DeleteList(int productTypeId);
        ProductTypeAttributeInfo Read(int id);
        ProductTypeAttributeInfo Read(string name,int productTypeId);
        List<ProductTypeAttributeInfo> ReadList();
        List<ProductTypeAttributeInfo> ReadList(int productTypeId);
    }
}