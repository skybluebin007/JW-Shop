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
    public class ProductCommentAjax : AjaxBasePage
    {
     
        /// <summary>
        /// 评论列表
        /// </summary>
        protected List<ProductCommentInfo> productCommentList = new List<ProductCommentInfo>();
        /// <summary>
        /// 用户列表
        /// </summary>
        protected List<UserInfo> userList = new List<UserInfo>();
        /// <summary>
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Add":
                    PostProductComment();
                    break;
                //case "Against":
                //    AgainstComment();
                //    break;
                //case "Support":
                //    SupportComment();
                //    break;
                //case "AddTags":
                //    PostTags();
                //    break;
                default:
                    break;
            }

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 8;
            int count = 0;

            ProductCommentSearchInfo productCommentSearch = new ProductCommentSearchInfo();
            productCommentSearch.ProductId = RequestHelper.GetQueryString<int>("ProductID");
            productCommentSearch.Status = (int)CommentStatus.Show;
            productCommentList = ProductCommentBLL.SearchList(currentPage, pageSize, productCommentSearch, ref count);

            ajaxPagerClass.CurrentPage = currentPage;
            ajaxPagerClass.PageSize = pageSize;
            ajaxPagerClass.Count = count;
            ajaxPagerClass.FirstPage = "<<首页";
            ajaxPagerClass.PreviewPage = "<<上一页";
            ajaxPagerClass.NextPage = "下一页>>";
            ajaxPagerClass.LastPage = "末页>>";
            ajaxPagerClass.ListType = false;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.NumType = false;
            ajaxPagerClass.PrenextType = true;

            string strUserID = string.Empty;
            foreach (ProductCommentInfo productComment in productCommentList)
            {
                if (strUserID == string.Empty)
                {
                    strUserID = productComment.UserId.ToString();
                }
                else
                {
                    strUserID += "," + productComment.UserId.ToString();
                }
            }
            if (strUserID != string.Empty)
            {
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InUserId = strUserID;
                userList = UserBLL.SearchList(userSearch);
            }
        }
        /// <summary>
        /// 提交评论
        /// </summary>
        public void PostProductComment()
        {
            string result = "ok";
            if (ShopConfig.ReadConfigInfo().AllowAnonymousComment == (int)BoolType.False && base.UserId == 0)
            {
                result = "还未登录";
            }
            else
            {
                AddProductComment(ref result);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="result"></param>
        public void AddProductComment(ref string result)
        {
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            string commentCookies = CookiesHelper.ReadCookieValue("CommentCookies" + productID.ToString());
            if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0 && commentCookies != string.Empty)
            {
                result = "请不要频繁提交";
            }
            else
            {
                ProductCommentInfo productComment = new ProductCommentInfo();
                productComment.ProductId = productID;
                productComment.Title = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Title"));
                productComment.Content = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Content"));
                productComment.UserIP = ClientHelper.IP;
                productComment.PostDate = RequestHelper.DateNow;
                productComment.Support = 0;
                productComment.Against = 0;
                productComment.Status = ShopConfig.ReadConfigInfo().CommentDefaultStatus;
                productComment.Rank = RequestHelper.GetQueryString<int>("Rank");
                productComment.ReplyCount = 0;
                productComment.AdminReplyContent = string.Empty;
                productComment.AdminReplyDate = RequestHelper.DateNow;
                productComment.UserId = base.UserId;
                productComment.UserName = base.UserName;
                productComment.OrderId = orderID;
                ProductCommentBLL.Add(productComment);
                if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0)
                {
                    CookiesHelper.AddCookie("CommentCookies" + productID.ToString(), "CommentCookies" + productID.ToString(), ShopConfig.ReadConfigInfo().CommentRestrictTime, TimeType.Second);
                }
            }
        }
       
    }
}