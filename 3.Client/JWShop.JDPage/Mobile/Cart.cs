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

namespace JWShop.Page.Mobile
{
    public class Cart : CommonBasePage
    {
        protected List<CartInfo> cartList = new List<CartInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 11;

            Title = "购物车";

            cartList = CartBLL.ReadList(base.UserId);

            //关联的商品
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Count(), new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

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
                }
                else
                {
                    cart.Price = cart.Product.SalePrice;
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                }
            }
        }

        protected string GetPrice(int id, decimal price, string standardValue)
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