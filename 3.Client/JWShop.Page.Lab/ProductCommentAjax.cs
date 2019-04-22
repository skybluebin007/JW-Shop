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

namespace JWShop.Page.Lab
{
    public class ProductCommentAjax : CommonBasePage
    {
        protected List<ProductCommentInfo> commentList = new List<ProductCommentInfo>();
        protected List<UserInfo> userList = new List<UserInfo>();
        protected AjaxPagerClass pager = new AjaxPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 15;
            int count = 0;

            ProductCommentSearchInfo productCommentSearch = new ProductCommentSearchInfo();
            productCommentSearch.ProductId = RequestHelper.GetQueryString<int>("id");
            productCommentSearch.Status = (int)CommentStatus.Show;
            commentList = ProductCommentBLL.SearchList(currentPage, pageSize, productCommentSearch, ref count);
            userList = UserBLL.SearchList(new UserSearchInfo { InUserId = string.Join(",", commentList.Select(k => k.UserId).ToArray()) });

            pager.CurrentPage = currentPage;
            pager.PageSize = pageSize;
            pager.Count = count;
            pager.DisCount = false;
            pager.ListType = false;
        }
    }
}