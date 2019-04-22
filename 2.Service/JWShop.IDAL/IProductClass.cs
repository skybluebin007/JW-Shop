using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductClass
    {
        int Add(ProductClassInfo entity);
        void Update(ProductClassInfo entity);
        void Delete(int id);
        ProductClassInfo Read(int id);
        List<ProductClassInfo> ReadList();

        void Move(int id, ChangeAction action);

        int GetProductClassType(int productClassID);
        /// <summary>
        /// 修改分类排序
        /// </summary>
        /// <param name="id">要移动的id</param>
        void ChangeProductClassOrder(int pid, int oid);
        void ChangeProductCount(int[] classIds, ChangeAction action);
    }
}