using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class CartBLL : BaseBLL
    {
        private static readonly ICart dal = FactoryHelper.Instance<ICart>(Global.DataProvider, "CartDAL");

        public static int Add(CartInfo entity, int userId)
        {
            if (userId > 0)
            {
                return dal.Add(entity);
            }
            else
            {
                return CartHelper.AddToCart(entity);
            }
        }

        public static void Update(int[] ids, int count, int userId)
        {
            if (userId > 0)
            {
                dal.Update(ids, count);
            }
            else
            {
                CartHelper.UpdateCart(string.Join(",", ids), count);
            }
        }

        public static void Delete(int[] ids, int userId)
        {
            if (userId > 0)
            {
                dal.Delete(ids, userId);
            }
            else
            {
                CartHelper.DeleteCart(string.Join(",", ids));
            }
        }
    
        public static void Clear(int userId)
        {
            if (userId > 0)
            {
                dal.Clear(userId);
            }
            else
            {
                CartHelper.ClearCart();
            }
        }

        public static List<CartInfo> ReadList(int userId)
        {
            if (userId > 0)
            {
                var cartList=dal.ReadList(userId);
                foreach(CartInfo cart in cartList){
                    if (ProductBLL.Read(cart.ProductId).Id <= 0) {//如果商品已删除则删除购物车里的
                        Delete(Array.ConvertAll<string, int>(cart.Id.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)), userId);
                    }
                }
                return dal.ReadList(userId);
            }
            else
            {
                var cartList = CartHelper.ReadCart();
                foreach (CartInfo cart in cartList)
                {
                    if (ProductBLL.Read(cart.ProductId).Id <= 0)
                    {
                        Delete(Array.ConvertAll<string, int>(cart.Id.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)), userId);
                    }
                }
                return CartHelper.ReadCart();
            }
        }

        public static int Count(int userId)
        {
            var cartList = ReadList(userId);
            return cartList.Sum(k => k.BuyCount);
        }

        public static CartInfo Read(int productId, string productName, int userId)
        {
            if (userId > 0)
            {
                return dal.Read(productId, productName, userId);
            }
            else
            {
                return CartHelper.ReadCart(productId, productName);
            }
        }

        public static void UpdateCartNum(int cartId, int num, int uid)
        {
            dal.UpdateCartNum(cartId, num, uid);
        }

        /// <summary>
        /// 用户登录之后Cookies购物转入到数据库
        /// </summary>
        public static void CookiesImportDataBase(int userId, string userName)
        {
            foreach (CartInfo cart in CartHelper.ReadCart())
            {
                var dbCart = CartBLL.Read(cart.ProductId, cart.ProductName, userId);
                if (dbCart.Id < 1)
                {
                    cart.UserId = userId;
                    cart.UserName = userName;
                    dal.Add(cart);
                }
            }
            CartHelper.ClearCart();
        }

        /// <summary>
        /// 检查购物车是否存在该商品(普通商品购买需判断)
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        /// <param name="userID">用户ID</param>
        public static bool IsProductInCart(int productID, string productName, int userID)
        {
            if (userID > 0)
            {
                return dal.IsProductInCart(productID, productName, userID);
            }
            else
            {
                return CartHelper.IsProductInCart(productID, productName);
            }
        }
    }
}