using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductBrand
    {
        int Add(ProductBrandInfo entity);
        void Update(ProductBrandInfo entity);
        void Delete(int id);
        ProductBrandInfo Read(int id);
        ProductBrandInfo Read(string name);
        List<ProductBrandInfo> ReadList();
        List<ProductBrandInfo> ReadList(int[] ids);
      List<ProductBrandInfo> SearchList(ProductBrandSearchInfo brandSearch);
      List<ProductBrandInfo> SearchList(int currentPage, int pageSize, ProductBrandSearchInfo searchInfo, ref int count);
        void Move(int id, ChangeAction action);
    }
}