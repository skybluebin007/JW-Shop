using System;
using System.Collections.Generic;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Entity;
using System.Linq;

namespace JWShop.Page
{
    public class ProviderGoodsAjax : ShopBasePage
    {
        protected AjaxPagerClass pager = new AjaxPagerClass();
        protected List<ProductInfo> ProductList = new List<ProductInfo>();

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
            ProductSearchInfo searchInfo = new ProductSearchInfo();
            string productNumber = StringHelper.AddSafe(RequestHelper.GetForm<string>("productCode"));
            string name = StringHelper.AddSafe(RequestHelper.GetForm<string>("name"));
            string tmStart = StringHelper.AddSafe(RequestHelper.GetForm<string>("tmStart"));
            string tmEnd = StringHelper.AddSafe(RequestHelper.GetForm<string>("tmEnd"));

            searchInfo.InShopId = CurrentUser.Id.ToString();
            if (!string.IsNullOrEmpty(productNumber)) searchInfo.ProductNumber = productNumber;
            if (!string.IsNullOrEmpty(name)) searchInfo.Name = name;
            if (!string.IsNullOrEmpty(tmStart)) searchInfo.StartAddDate = Convert.ToDateTime(tmStart);
            if (!string.IsNullOrEmpty(tmEnd)) searchInfo.EndAddDate = Convert.ToDateTime(tmEnd);

            ProductList = ProductBLL.SearchList(currentPage, pageSize, searchInfo, ref count);
            pager.CurrentPage = currentPage;
            pager.PageSize = pageSize;
            pager.Count = count;
        }

    }
}