using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Page
{
    public class CartAjax : AjaxBasePage
    {
        protected List<CartInfo> cartList = new List<CartInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Read":
                    ReadCart();
                    break;
                case "AddToCart":
                    AddToCart();
                    break;
                case "ClearCart":
                    ClearCart();
                    break;
                case "ChangeBuyCount":
                    ChangeBuyCount();
                    break;
                case "DeleteCart":
                    DeleteCart();
                    break;
                case "Collect":
                    Collect();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 读取购物车
        /// </summary>
        private void ReadCart()
        {
            cartList = CartBLL.ReadList(base.UserId);

            //关联的商品
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Count(), new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            int productCount = 0;
            //规格
            foreach (var cart in cartList)
            {
                cart.Product = products.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.Price = standardRecord.SalePrice;
                    cart.LeftStorageCount = standardRecord.Storage - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                    //规格集合
                    cart.Standards = ProductTypeStandardBLL.ReadList(Array.ConvertAll<string, int>(standardRecord.StandardIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)));
                    productCount += cart.BuyCount;
                }
                else
                {
                    cart.Price = cart.Product.SalePrice;
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                    productCount += cart.BuyCount;
                }
            }
            Sessions.ProductBuyCount = productCount;
        }
        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        protected void AddToCart()
        {
            int productId = RequestHelper.GetQueryString<int>("ProductId");
            string productName = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("ProductName"));
            var cart = CartBLL.Read(productId, productName, base.UserId);
            if (cart.Id < 1)
            {
                int buyCount = RequestHelper.GetQueryString<int>("BuyCount");
                string standardValueList = RequestHelper.GetQueryString<string>("StandardValueList");
                var product = ProductBLL.Read(productId);

                cart.ProductId = productId;
                cart.ProductName = productName;
                cart.StandardValueList = standardValueList;
                cart.BuyCount = buyCount;
                cart.RandNumber = string.Empty;
                cart.UserId = base.UserId;
                cart.UserName = base.UserName;
                CartBLL.Add(cart, base.UserId);

                Sessions.ProductBuyCount += buyCount;
            }
            else
            {
                CartBLL.Update(new int[] { cart.Id }, ++cart.BuyCount, base.UserId);
            }

            ResponseHelper.Write("ok");
            ResponseHelper.End();
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        private void ClearCart()
        {
            CartBLL.Clear(base.UserId);
            ResponseHelper.End();
        }
        /// <summary>
        /// 改变购买数量
        /// </summary>
        private void ChangeBuyCount()
        {
            string strCartId = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("StrCartId"));
            int buyCount = RequestHelper.GetQueryString<int>("BuyCount");
            int oldCount = RequestHelper.GetQueryString<int>("OldCount");
            decimal price = RequestHelper.GetQueryString<decimal>("Price");
            int[] ids = Array.ConvertAll<string, int>(strCartId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            CartBLL.Update(ids, buyCount, base.UserId);
            var totalCount = CartBLL.ReadList(base.UserId).Where(k => k.Id.ToString() == strCartId).Sum(k => k.BuyCount);
            var totalPrice = totalCount * price;
            ResponseHelper.Write(strCartId + "|" + totalCount + "|" + totalPrice);
            ResponseHelper.End();
        }
        /// <summary>
        /// 删除购物车
        /// </summary>
        private void DeleteCart()
        {
            string strCartId = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("StrCartId"));
            if (string.IsNullOrEmpty(strCartId))
            {
                ResponseHelper.Write("error|请选择商品！");
                ResponseHelper.End();
            }

            int[] ids = Array.ConvertAll<string, int>(strCartId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            CartBLL.Delete(ids, base.UserId);
            ResponseHelper.Write("ok|");
            ResponseHelper.End();
        }
        /// <summary>
        /// 移到我的收藏
        /// </summary>
        private void Collect()
        {
            string strCartId = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("StrProductId"));

            if (string.IsNullOrEmpty(strCartId))
            {
                ResponseHelper.Write("error|请选择商品！");
                ResponseHelper.End();
            }

            if (base.UserId == 0)
            {
                ResponseHelper.Write("error|还未登录！");
                ResponseHelper.End();
            }

            int[] ids = Array.ConvertAll<string, int>(strCartId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            foreach (var id in ids)
            {
                if (ProductCollectBLL.Read(id, base.UserId).Id < 1)
                {
                    ProductCollectInfo productCollect = new ProductCollectInfo();
                    productCollect.ProductId = id;
                    productCollect.Tm = RequestHelper.DateNow;
                    productCollect.UserId = base.UserId;
                    ProductCollectBLL.Add(productCollect);
                }
            }
            ResponseHelper.Write("error|成功收藏！");
            ResponseHelper.End();
        }

        protected string GetPrice(int id,decimal price, string standardValue)
        {
            if (!string.IsNullOrEmpty(standardValue.Trim()))
            {
                return ProductBLL.GetCurrentPriceWithStandard(id, base.GradeID, standardValue).ToString();
            }
            else
            {
                return ProductBLL.GetCurrentPrice(price, base.GradeID).ToString();
            }
        }
    }
}