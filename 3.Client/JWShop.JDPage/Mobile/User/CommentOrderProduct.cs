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

namespace JWShop.Page.Mobile
{
    public class CommentOrderProduct : UserBasePage
    {
        /// <summary>
        /// 用户评论
        /// </summary>
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected OrderInfo order = new OrderInfo();
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected string strProductID2 = string.Empty;
        protected int proID = int.MinValue;
        protected bool isPL = true;//是否已评论
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();

            int orderID = RequestHelper.GetQueryString<int>("OrderID");


            order = OrderBLL.Read(orderID, base.UserId);
            if (order.OrderStatus != (int)OrderStatus.ReceiveShipping)
            {
                ScriptHelper.AlertFront("只能评论已收货订单");
            }
            orderDetailList = OrderDetailBLL.ReadList(orderID);
#region 加载订单下商品
            foreach (OrderDetailInfo orderDetail in orderDetailList)
            {
                if (strProductID2 == string.Empty)
                {
                    strProductID2 = orderDetail.ProductId.ToString();
                }
                else
                {
                    strProductID2 += "," + orderDetail.ProductId.ToString();
                }
            }
            if (strProductID2 != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = strProductID2;
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
                psi.OrderID = orderID;
                listPinfoArr[pi] = ProductCommentBLL.SearchProductCommentList(psi);
                if (listPinfoArr[pi].Count <= 0)
                    isPL = false;
            }
            if (isPL) {
                string url = "";
                if (Request.RawUrl.ToLower().IndexOf("/mobile/") >= 0)
                {
                    url = "/mobile/User/UserProductComment.html";
                }
                else
                {
                    url = "/User/UserProductComment.html";
                }
                Response.Redirect(url);
            }
            #endregion
            //ProductCommentSearchInfo productCommentSearch = new ProductCommentSearchInfo();        
            //productCommentSearch.UserId = base.UserId;           
            //productCommentSearch.OrderID = orderID;
            //if (ProductCommentBLL.SearchProductCommentList(productCommentSearch).Count >= productList.Count)
            //{
            //    string url = "";
            //    if (Request.RawUrl.ToLower().IndexOf("/mobile/") >= 0)
            //    {
            //        url = "/mobile/User/UserProductComment.html";
            //    }
            //    else
            //    {
            //        url = "/User/UserProductComment.html";
            //    }
            // Response.Redirect(url);
            
           
            Title = "订单评价 - 会员中心";
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            string url = "";

            if (Request.RawUrl.ToLower().IndexOf("/mobile/") >= 0)
            {
                url = "/mobile/User/";
            }
            else
            {
                url = "/User/";
            }
            if (base.UserId > 0)
            {

                int orderID = RequestHelper.GetForm<int>("OrderID");

                order = OrderBLL.Read(orderID, base.UserId);
                orderDetailList = OrderDetailBLL.ReadList(orderID);

                if (order.Id > 0)
                {
                
                    if (ProductCommentBLL.SearchProductCommentList(new ProductCommentSearchInfo { OrderID = order.Id }).Count<= 0)
                    {
                        List<ProductCommentInfo> productCommentList = new List<ProductCommentInfo>();
                        foreach (OrderDetailInfo orderDetail in orderDetailList)
                        {
                            if (RequestHelper.GetForm<int>("pid_" + orderDetail.ProductId) > 0)
                            {
                                proID = RequestHelper.GetForm<int>("pid_" + orderDetail.ProductId);
                            }
                        }
                        //foreach (OrderDetailInfo orderDetail in orderDetailList)
                        //{
                        int rank = RequestHelper.GetForm<int>("rank_" + proID);
                        string title = StringHelper.AddSafe(RequestHelper.GetForm<string>("title_" + proID));
                        string content = StringHelper.AddSafe(RequestHelper.GetForm<string>("content_" + proID));

                        //   ProductInfo product = ProductBLL.ReadProduct(orderDetail.ProductID);
                        ProductInfo product = ProductBLL.Read(proID);
                        if (rank >= 1 && rank <= 5 && content != string.Empty)
                        {
                            ProductCommentInfo productComment = new ProductCommentInfo();
                            //   productComment.ProductID = orderDetail.ProductID;
                            productComment.ProductId = proID;
                            productComment.Title = title;
                            productComment.Content = content;
                            productComment.UserIP = ClientHelper.IP;
                            productComment.PostDate = RequestHelper.DateNow;
                            productComment.Support = 0;
                            productComment.Against = 0;
                            productComment.Status = ShopConfig.ReadConfigInfo().CommentDefaultStatus;
                            productComment.Rank = rank;
                            productComment.ReplyCount = 0;
                            productComment.AdminReplyContent = string.Empty;
                            productComment.AdminReplyDate = RequestHelper.DateNow;
                            productComment.UserId = base.UserId;
                            productComment.UserName = base.UserName;
                            productComment.OrderId = order.Id;
                            productComment.BuyDate = order.AddDate;
                            productCommentList.Add(productComment);
                        }
                        else
                        {
                            ResponseHelper.Redirect(url + "CommentOrderProduct-O" + order.Id + ".html");
                        }
                        //}

                        foreach (ProductCommentInfo comment in productCommentList)
                        {
                            ProductCommentBLL.Add(comment);
                        }

                       ResponseHelper.Redirect(url + "UserProductComment.html");
                    
                    }
                    else
                    {
                        ResponseHelper.Redirect(url + "UserProductComment.html");
                    
                    }
                }
                else
                {
                    ResponseHelper.Redirect(url + "Order.html");
                }

            }
            else
            {
                ResponseHelper.Redirect(url + "Login.html");
            }
        }
    }
}
