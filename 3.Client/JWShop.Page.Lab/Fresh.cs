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

namespace JWShop.Page.Lab
{
    public class Fresh : CommonBasePage
    {
        protected CommonPagerClass pager = new CommonPagerClass();
        protected List<ProductInfo> productList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            ProductSearchInfo searchInfo = new ProductSearchInfo();
            searchInfo.IsNew = (int)BoolType.True;
            searchInfo.ProductOrderType = "AddDate";
            searchInfo.OrderType = OrderType.Desc;

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 50;
            int count = 0;
            productList = ProductBLL.SearchList(currentPage, pageSize, searchInfo, ref count);
            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            Title = "新品上市";
        }
    }
}