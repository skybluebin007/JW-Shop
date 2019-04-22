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
    public class OrderEval : UserBasePage
    {
        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGradeName = string.Empty;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 订单
        /// </summary>
        protected OrderInfo order = new OrderInfo();
        /// <summary>
        /// 订单详细列表
        /// </summary>
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();//订单下产品
        protected bool isPL = true;//是否已评论
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserId);
            userGradeName = UserGradeBLL.Read(base.GradeID).Name;
            int orderID = RequestHelper.GetQueryString<int>("ID");
            order = OrderBLL.Read(orderID, base.UserId);
            orderDetailList = OrderDetailBLL.ReadList(orderID);
            #region 加载订单下产品
            string strProductID = string.Empty;
            foreach (OrderDetailInfo orderDetail in orderDetailList)
            {
                if (strProductID == string.Empty)
                {
                    strProductID = orderDetail.ProductId.ToString();
                }
                else
                {
                    strProductID += "," + orderDetail.ProductId.ToString();
                }
            }
            if (strProductID != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = strProductID;
                productList = ProductBLL.SearchList(productSearch);
            }
            #endregion
            #region 判断是否已评论
            List<ProductCommentInfo>[] listPinfoArr = new List<ProductCommentInfo>[productList.Count];
            int pi = 0;
            foreach (ProductInfo item in productList)
            {
                ProductCommentSearchInfo psi = new ProductCommentSearchInfo();
                psi.ProductId = item.Id;
                psi.UserId = base.UserId;
                psi.OrderID= orderID;
                listPinfoArr[pi] = ProductCommentBLL.SearchProductCommentList(psi);
                if (listPinfoArr[pi].Count <= 0)
                    isPL = false;
            }
            #endregion
            if (isPL)
                Response.Redirect("/User/OrderDetail.html?ID=" + orderID);
        }
        private string footaddress = string.Empty;
     
    }
}
