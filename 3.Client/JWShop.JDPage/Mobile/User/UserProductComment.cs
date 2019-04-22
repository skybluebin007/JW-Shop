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
    public class UserProductComment : UserBasePage
    {
        protected List<ProductCommentInfo> productCommentList = new List<ProductCommentInfo>();
        protected CommonPagerClass pager = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            productCommentList = ProductCommentBLL.SearchList(currentPage, pageSize, new ProductCommentSearchInfo { UserId = base.UserId }, ref count);

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            Title = "订单评价";
        }
    }
}