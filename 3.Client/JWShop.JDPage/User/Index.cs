using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page
{
    public class Index : UserBasePage
    {
        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<ProductInfo> proListHot = new List<ProductInfo>();       
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected DataTable dt = new DataTable();
     
        protected int[] arrT = new int[6];
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();

        protected List<ProductInfo> proHistoryList = new List<ProductInfo>();
        /// <summary>
        /// 浏览过的商品ID串
        /// </summary>
        protected string strHistoryProduct = string.Empty;
        /// <summary>
        /// 用户收藏
        /// </summary>
        protected List<ProductCollectInfo> productCollectList = new List<ProductCollectInfo>();
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            //检查用户的待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime(base.UserId);
            //订单自动收货
            OrderBLL.CheckOrderRecieveTimeProg(base.UserId);
            //cartList = CartBLL.ReadList(base.UserId);
            topNav = 7;
        
            string keywords = RequestHelper.GetQueryString<string>("keywords");
         
            int count = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.UserId = base.UserId;
            orderSearch.IsDelete = 0;
            if (!string.IsNullOrEmpty(keywords))
            {
                orderSearch.OrderNumber = keywords;
            }
           

            orderList = OrderBLL.SearchList(1, 5, orderSearch, ref count);
       
            //commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
            //热销商品
            proListHot = ProductBLL.SearchList(1, 8, new ProductSearchInfo { IsHot = 1,IsSale=1,IsDelete=0 }, ref count);

            #region 对应状态个数

            arrT[0] = OrderBLL.SearchList(new OrderSearchInfo { UserId = base.UserId, IsDelete = 0 }).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.WaitPay);
            orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
            arrT[1] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.Shipping);
            orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
            arrT[2] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.HasShipping);
            orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
            arrT[3] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.WaitCheck);
            orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
            arrT[4] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.NoEffect);
            orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
            arrT[5] = OrderBLL.SearchList(orderSearch).Count;
            #endregion

            #region 浏览过的商品
            strHistoryProduct = Server.UrlDecode(CookiesHelper.ReadCookieValue("HistoryProduct"));
            if (strHistoryProduct.StartsWith(","))
            {
                strHistoryProduct = strHistoryProduct.Substring(1);
            }
            if (strHistoryProduct.EndsWith(","))
            {
                strHistoryProduct = strHistoryProduct.Substring(0, strHistoryProduct.Length - 1);
            }
            if (!string.IsNullOrEmpty(strHistoryProduct)) proHistoryList = ProductBLL.SearchList(1, 12, new ProductSearchInfo { IsSale = 1, InProductId = strHistoryProduct, IsDelete = 0 }, ref count);
            #endregion
            #region 收藏的商品
          productCollectList = ProductCollectBLL.ReadListByUserId(base.UserId);
            string strProductID = string.Empty;
            foreach (ProductCollectInfo productCollect in productCollectList)
            {
                if (strProductID == string.Empty)
                {
                    strProductID = productCollect.ProductId.ToString();
                }
                else
                {
                    strProductID += "," + productCollect.ProductId.ToString();
                }
            }

            if (!string.IsNullOrEmpty(strProductID)) productList = ProductBLL.SearchList(1, 8, new ProductSearchInfo { InProductId = strProductID, IsSale = 1, IsDelete = 0 }, ref count);
            #endregion 
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