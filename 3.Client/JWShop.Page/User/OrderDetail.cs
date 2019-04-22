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
using System.Linq;

namespace JWShop.Page
{
    public class OrderDetail : UserBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();

        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGradeName = string.Empty;
        protected bool isPL = true;//是否已评论

        protected override void PageLoad()
        {
            base.PageLoad();

            int orderId = RequestHelper.GetQueryString<int>("id");
            userGradeName = UserGradeBLL.Read(base.GradeID).Name;


            order = OrderBLL.Read(orderId, base.UserId);
            

            if (order.Id <= 0)
            {
                ScriptHelper.AlertFront("订单不存在", "/user/index.html");
            }

            orderDetailList = OrderDetailBLL.ReadList(orderId);
            int[] productIds = orderDetailList.Select(k => k.ProductId).ToArray();
            if (productIds.Length > 0)
            {
                int count = 0;
                productList = ProductBLL.SearchList(1, productIds.Length, new ProductSearchInfo { InProductId = string.Join(",", productIds) }, ref count);
            }

            #region 判断是否已评论
            List<ProductCommentInfo>[] listPinfoArr = new List<ProductCommentInfo>[productList.Count];
            int pi = 0;
            foreach (ProductInfo item in productList)
            {
                ProductCommentSearchInfo psi = new ProductCommentSearchInfo();
                psi.ProductId = item.Id;
                psi.UserId = base.UserId;
                psi.OrderID = orderId;
                listPinfoArr[pi] = ProductCommentBLL.SearchProductCommentList(psi);
                if (listPinfoArr[pi].Count <= 0)
                    isPL = false;
            }
            #endregion
            Title = "我的订单";
        }
    }
}