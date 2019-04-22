using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductTypeAttributeRecord
    {
        void Add(ProductTypeAttributeRecordInfo entity);
        void Update(ProductTypeAttributeRecordInfo entity);
        void Delete(int productId);

        void DeleteByAttr(int attrId);
        List<ProductTypeAttributeRecordInfo> ReadList();
        List<ProductTypeAttributeRecordInfo> ReadList(int productId);
    }
}