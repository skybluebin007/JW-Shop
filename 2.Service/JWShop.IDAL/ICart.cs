using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface ICart
    {
        int Add(CartInfo entity);
        void Update(int[] ids, int count);
        void Delete(int[] ids, int userId);
       
        void Clear(int userId);
        CartInfo Read(int productId, string productName, int userId);
        void UpdateCartNum(int cartId, int num,int uid);
        List<CartInfo> ReadList(int userId);
        /// <summary>
        /// 检查购物车是否存在该商品(普通商品购买需判断)
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        /// <param name="userID">用户ID</param>
        bool IsProductInCart(int productID, string productName, int userID);
    }
}