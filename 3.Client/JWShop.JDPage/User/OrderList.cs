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
    public class OrderList : UserBasePage
    {
        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<ProductInfo> proListHot = new List<ProductInfo>();
        protected decimal money, point;
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected DataTable dt = new DataTable();
        protected string type = "";
        protected int[] arrT = new int[6];

        protected string keywords = string.Empty;
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();
            //检查用户的待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime(base.UserId);
            type = RequestHelper.GetQueryString<string>("type");
            cartList = CartBLL.ReadList(base.UserId);
            topNav = 7;            
            keywords = RequestHelper.GetQueryString<string>("keywords");
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
            int count = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.UserId = base.UserId;
            orderSearch.IsDelete = 0;
            if (!string.IsNullOrEmpty(keywords))
            {
                orderSearch.OrderNumber = keywords;
            }
            if (!string.IsNullOrEmpty(type))
            {
                int tt = 0;
                switch (type)
                {
                    case "1": tt = (int)OrderStatus.WaitPay; break;
                    case "2": tt = (int)OrderStatus.WaitCheck; break;
                    case "3": tt = (int)OrderStatus.Shipping; break;
                    case "4": tt = (int)OrderStatus.HasShipping; break;
                    case "5": tt = (int)OrderStatus.NoEffect; break;
                }
                orderSearch.OrderStatus = tt;
            }

            orderList = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count);
            //commonPagerClass.CurrentPage = currentPage;
            //commonPagerClass.PageSize = pageSize;
            //commonPagerClass.Count = count;
            //commonPagerClass.FirstPage = "<<首页";
            //commonPagerClass.PreviewPage = "<<上一页";
            //commonPagerClass.NextPage = "下一页>>";
            //commonPagerClass.LastPage = "末页>>";
            //commonPagerClass.ListType = false;
            //commonPagerClass.DisCount = false;
            //commonPagerClass.PrenextType = true;
            commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            proListHot = ProductBLL.SearchList(1, 12, new ProductSearchInfo { IsHot = 1, }, ref count);
            #region 对应状态个数
            //OrderSearchInfo orderSearch1 = new OrderSearchInfo();
            //orderSearch1.UserId = base.UserId;
            //if (!string.IsNullOrEmpty(keywords))
            //{
            //    orderSearch1.OrderNumber = keywords;
            //}
            arrT[0] = OrderBLL.SearchList(currentPage, pageSize, new OrderSearchInfo { UserId = base.UserId, IsDelete = 0 }, ref count).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.WaitPay);
            arrT[1] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.Shipping); 
            arrT[2] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.HasShipping); 
            arrT[3] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.WaitCheck); 
            arrT[4] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.NoEffect); 
            arrT[5] = OrderBLL.SearchList(currentPage, pageSize, orderSearch, ref count).Count;
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