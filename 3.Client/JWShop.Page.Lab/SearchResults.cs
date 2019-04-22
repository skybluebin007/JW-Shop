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
    public class SearchResults : CommonBasePage
    {
        protected ProductClassInfo currentClass = new ProductClassInfo();
        protected string baseQueryString;
        protected CommonPagerClass pager = new CommonPagerClass();
        protected int currentPage;

        //商品列表数据
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            //热门商品
            int hotCount = 0;
            hotProductList = ProductBLL.SearchList(1, 7, new ProductSearchInfo { IsHot = (int)BoolType.True, IsSale = (int)BoolType.True }, ref hotCount);

            //商品列表
            currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 24; int count = 0;
            List<ProductBrandInfo> showBrandList = new List<ProductBrandInfo>();
            List<ProductTypeAttributeInfo> showAttributeList = new List<ProductTypeAttributeInfo>();
            string orderField = RequestHelper.GetQueryString<string>("sort");
            string orderType = "";
            var orderParams = orderField.Split('_');
            if (orderParams.Length > 1)
            {
                orderField = orderParams[0];
                orderType = orderParams[1];
            }
            int minPrice = RequestHelper.GetQueryString<int>("min");
            int maxPrice = RequestHelper.GetQueryString<int>("max");
            string keywords = RequestHelper.GetQueryString<string>("kw");
            string isNew = RequestHelper.GetQueryString<string>("isNew");
            string isHot = RequestHelper.GetQueryString<string>("isHot");
            string isSpecial = RequestHelper.GetQueryString<string>("isSpecial");
            string isTop = RequestHelper.GetQueryString<string>("isTop");

            //productList = ProductBLL.SearchList(currentPage, pageSize, 0, "", "", "", orderField, orderType, minPrice, maxPrice, keywords, isNew, isHot, isSpecial, isTop, ref count, ref showAttributeList, ref showBrandList);

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
        }
    }
}