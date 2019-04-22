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
    public class UserProductCommentAdd : UserBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected string productIds;

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Submit") this.Submit();

            int orderId = RequestHelper.GetQueryString<int>("orderId");

            order = OrderBLL.Read(orderId, base.UserId);
            orderDetailList = OrderDetailBLL.ReadList(orderId);
            productIds = string.Join(",", orderDetailList.Select(k => k.ProductId).ToArray());

            Title = "订单评价";
        }

        private void Submit()
        {
            string urlPrefix = string.IsNullOrEmpty(isMobile) ? "/user" : "/mobile";

            if (base.UserId < 1)
            {
                ResponseHelper.Write("error|登录状态已过期，请重新登录|" + urlPrefix + "/login.html");
                ResponseHelper.End();
            }

            int orderId = RequestHelper.GetForm<int>("orderId");
            if (ProductCommentBLL.HasCommented(orderId, base.UserId))
            {
                ResponseHelper.Write("error|订单已评价|" + urlPrefix + "/userproductcomment.html");
                ResponseHelper.End();
            }

            order = OrderBLL.Read(orderId, base.UserId);
            if (order.Id < 1)
            {
                ResponseHelper.Write("error|订单不存在|" + urlPrefix + "/order.html");
                ResponseHelper.End();
            }

            orderDetailList = OrderDetailBLL.ReadList(orderId);

            List<ProductCommentInfo> productCommentList = new List<ProductCommentInfo>();
            foreach (OrderDetailInfo orderDetail in orderDetailList)
            {
                int rank = RequestHelper.GetForm<int>("rank_" + orderDetail.ProductId);
                string content = StringHelper.AddSafe(RequestHelper.GetForm<string>("content_" + orderDetail.ProductId));

                if (rank >= 1 && rank <= 5 && !string.IsNullOrEmpty(content))
                {
                    ProductCommentInfo productComment = new ProductCommentInfo();
                    productComment.ProductId = orderDetail.ProductId;
                    productComment.Title = "";
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
                    ResponseHelper.Write("error|评价失败|" + urlPrefix + "/userproductcommentAdd.html?orderid=" + order.Id);
                    ResponseHelper.End();
                }
            }

            foreach (ProductCommentInfo comment in productCommentList)
            {
                ProductCommentBLL.Add(comment);
            }

            ResponseHelper.Write("ok|评价成功|" + urlPrefix + "/userproductcomment.html");
            ResponseHelper.End();
        }
    }
}