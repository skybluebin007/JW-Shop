using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductType
    {
        int Add(ProductTypeInfo entity);
        void Update(ProductTypeInfo entity);
        void Delete(int id);
        ProductTypeInfo Read(int id);
        List<ProductTypeInfo> ReadList();
    }
}