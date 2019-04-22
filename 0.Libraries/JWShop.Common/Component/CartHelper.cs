using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// cookies购物车处理
    /// </summary>
    public sealed class CartHelper
    {
        private static string cookiesName = "CookiesCart";
        private static HttpCookie cartCookies = new HttpCookie(cookiesName);
        private static Hashtable ht = new Hashtable();
        private static string secureKey = ShopConfig.ReadConfigInfo().SecureKey;

        /// <summary>
        /// 购物车初始化
        /// </summary>
        public static void Init()
        {
            ht = new Hashtable();
            cartCookies = new HttpCookie(cookiesName);
            ht["cartId"] = "#";
            ht["productId"] = "#";
            ht["productName"] = "#";
            ht["standardValueList"] = "#";
            ht["buyCount"] = "#";
            ht["randNumber"] = "#";
            EncodeCart();
            cartCookies.Expires = DateTime.Now.AddDays(7);
            HttpContext.Current.Response.Cookies.Add(cartCookies);
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        public static void ClearCart()
        {
            ht = new Hashtable();
            ht["cartId"] = "#";
            ht["productId"] = "#";
            ht["productName"] = "#";
            ht["standardValueList"] = "#";
            ht["buyCount"] = "#";
            ht["randNumber"] = "#";
            EncodeCart();
            HttpContext.Current.Response.Cookies.Add(cartCookies);
        }
        /// <summary>
        /// 加密购物车并且给Cookies写值
        /// </summary>
        private static void EncodeCart()
        {
            cartCookies["cartId"] = StringHelper.Encode(ht["cartId"].ToString(), secureKey);
            cartCookies["productId"] = StringHelper.Encode(ht["productId"].ToString(), secureKey);
            cartCookies["productName"] = StringHelper.Encode(ht["productName"].ToString(), secureKey);
            cartCookies["standardValueList"] = StringHelper.Encode(ht["standardValueList"].ToString(), secureKey);
            cartCookies["buyCount"] = StringHelper.Encode(ht["buyCount"].ToString(), secureKey);
            cartCookies["randNumber"] = StringHelper.Encode(ht["randNumber"].ToString(), secureKey);
            cartCookies["Key"] = StringHelper.Encode(ClientHelper.Agent, secureKey);
        }
        /// <summary>
        /// 解密Cookies并且给购物车写值
        /// </summary>
        private static void DecodeCart()
        {
            cartCookies = HttpContext.Current.Request.Cookies[cookiesName];
            if (cartCookies != null && cartCookies["Key"] != null)
            {
                string key = StringHelper.Decode(cartCookies["Key"], secureKey);
                if (key == ClientHelper.Agent)
                {
                    ht["cartId"] = StringHelper.Decode(cartCookies["cartID"], secureKey);
                    ht["productId"] = StringHelper.Decode(cartCookies["productID"], secureKey);
                    ht["productName"] = StringHelper.Decode(cartCookies["productName"], secureKey);
                    ht["standardValueList"] = StringHelper.Decode(cartCookies["standardValueList"], secureKey);
                    ht["buyCount"] = StringHelper.Decode(cartCookies["buyCount"], secureKey);
                    ht["randNumber"] = StringHelper.Decode(cartCookies["randNumber"], secureKey);
                }
                else
                {
                    Init();
                }
            }
            else
            {
                Init();
            }
        }
        /// <summary>
        /// 检查购物车是否存在该商品(普通商品购买需判断)
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        public static bool IsProductInCart(int productID, string productName)
        {
            bool inCart = false;
            DecodeCart();
            foreach (CartInfo cart in ReadCart())
            {
                if (cart.RandNumber == string.Empty && cart.ProductId == productID && cart.ProductName == productName)
                {
                    inCart = true;
                    break;
                }
            }
            return inCart;
        }
        /// <summary>
        /// 根据商品Id和名称读取购物车对象
        /// </summary>
        public static CartInfo ReadCart(int productId, string productName)
        {
            DecodeCart();
            foreach (CartInfo cart in ReadCart())
            {
                if (string.IsNullOrEmpty(cart.RandNumber) && cart.ProductId == productId && cart.ProductName == productName)
                {
                    return cart;
                }
            }
            return new CartInfo();
        }
        /// <summary>
        /// 增加一条购物数据
        /// </summary>
        /// <param name="cart"></param>
        public static int AddToCart(CartInfo cart)
        {
            DecodeCart();
            int length = ht["productId"].ToString().Split('#').Length - 2;
            int maxID = 1;
            if (length > 0)
            {
                maxID = Convert.ToInt32(ht["cartId"].ToString().Split('#')[length]) + 1;
            }
            ht["cartId"] += maxID.ToString() + "#";
            ht["productId"] += cart.ProductId.ToString() + "#";
            ht["productName"] += cart.ProductName.ToString() + "#";
            ht["standardValueList"] += cart.StandardValueList.ToString() + "#";
            ht["buyCount"] += cart.BuyCount.ToString() + "#";
            ht["randNumber"] += cart.RandNumber + "#";
            EncodeCart();
            HttpContext.Current.Response.Cookies.Add(cartCookies);
            return maxID;
        }
        /// <summary>
        /// 读取购物车的产品
        /// </summary>
        /// <returns></returns>
        public static List<CartInfo> ReadCart()
        {
            DecodeCart();
            List<CartInfo> cartList = new List<CartInfo>();
            string[] cartIdArray = ht["cartId"].ToString().Split('#');
            string[] productIdArray = ht["productId"].ToString().Split('#');
            string[] productNameArray = ht["productName"].ToString().Split('#');
            string[] standardValueListArray = ht["standardValueList"].ToString().Split('#');
            string[] buyCountArray = ht["buyCount"].ToString().Split('#');
            string[] randNumberArray = ht["randNumber"].ToString().Split('#');
            for (int i = 1; i < productIdArray.Length - 1; i++)
            {
                CartInfo cart = new CartInfo();
                cart.Id = Convert.ToInt32(cartIdArray[i]);
                cart.ProductId = Convert.ToInt32(productIdArray[i]);
                cart.ProductName = productNameArray[i];
                cart.StandardValueList = standardValueListArray[i];
                cart.BuyCount = Convert.ToInt32(buyCountArray[i]);
                cart.RandNumber = randNumberArray[i];
                cartList.Add(cart);
            }
            return cartList;
        }
        /// <summary>
        /// 删除购物车产品
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteCart(string strId)
        {
            if (strId != string.Empty)
            {
                DecodeCart();
                strId = "#" + strId.Replace(",", "#") + "#";
                string[] cartIdArray = ht["cartId"].ToString().Split('#');
                string[] productIdArray = ht["productId"].ToString().Split('#');
                string[] productNameArray = ht["productName"].ToString().Split('#');
                string[] standardValueListArray = ht["standardValueList"].ToString().Split('#');
                string[] buyCountArray = ht["buyCount"].ToString().Split('#');
                string[] randNumberArray = ht["randNumber"].ToString().Split('#');
                string cartId = "#";
                string productId = "#";
                string productName = "#";
                string standardValueList = "#";
                string buyCount = "#";
                string randNumber = "#";
                for (int i = 1; i < cartIdArray.Length - 1; i++)
                {
                    if (strId.IndexOf("#" + cartIdArray[i] + "#") == -1)
                    {
                        cartId += cartIdArray[i] + "#";
                        productId += productIdArray[i] + "#";
                        productName += productNameArray[i] + "#";
                        standardValueList += standardValueListArray[i] + "#";
                        buyCount += buyCountArray[i] + "#";
                        randNumber += randNumberArray[i] + "#";
                    }
                }
                ht["cartId"] = cartId;
                ht["productId"] = productId;
                ht["productName"] = productName;
                ht["standardValueList"] = standardValueList;
                ht["buyCount"] = buyCount;
                ht["randNumber"] = randNumber;
                EncodeCart();
                HttpContext.Current.Response.Cookies.Add(cartCookies);
            }
        }

        /// <summary>
        /// 更新购物车产品的数量
        /// </summary>
        /// <param name="strId"></param>
        /// <param name="count"></param>
        public static void UpdateCart(string strId, int count)
        {
            DecodeCart();
            strId = "#" + strId.Replace(",", "#") + "#";
            string[] buyCountArray = ht["buyCount"].ToString().Split('#');
            string[] cartIdArray = ht["cartId"].ToString().Split('#');
            string buyCount = "#";
            for (int i = 1; i < buyCountArray.Length - 1; i++)
            {
                if (strId.IndexOf("#" + cartIdArray[i] + "#") > -1)
                {
                    buyCount += count + "#";
                }
                else
                {
                    buyCount += buyCountArray[i] + "#";
                }
            }
            ht["buyCount"] = buyCount;
            EncodeCart();
            HttpContext.Current.Response.Cookies.Add(cartCookies);
        }
    }
}