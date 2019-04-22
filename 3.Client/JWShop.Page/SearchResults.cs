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
            int pageSize = 15; int count = 0;
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
            int isNew = RequestHelper.GetQueryString<int>("isNew");
            int isHot = RequestHelper.GetQueryString<int>("isHot");
            int isSpecial = RequestHelper.GetQueryString<int>("isSpecial");
            int isTop = RequestHelper.GetQueryString<int>("isTop");

            productList = ProductBLL.SearchList(currentPage, pageSize, 0, "", "", "", orderField, orderType, minPrice, maxPrice, keywords,"", isNew, isHot, isSpecial, isTop, ref count, ref showAttributeList, ref showBrandList);

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
        }

        protected string ReplaceSearch(string sType, string sValue, string searchContion)
        {
            string[] arrSearch = searchContion.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            bool hasType = false;
            while (i < arrSearch.Length)
            {
                string[] subArr = arrSearch[i].Split('=');
                if (subArr[0].ToLower() == sType.ToLower())
                {
                    int afterVal = 1;
                    if (sValue == "1") afterVal = 0;
                    if (searchContion.IndexOf("&" + arrSearch[i] + "&") > -1)
                        searchContion = searchContion.Replace("&" + arrSearch[i], "&" + sType + "=" + afterVal);
                    else if (searchContion.IndexOf("&" + arrSearch[i]) > -1)
                        searchContion = searchContion.Replace("&" + arrSearch[i], "&" + sType + "=" + afterVal);

                    hasType = true;
                    break;
                }
                i++;
            }

            if (!hasType)
            {
                if (!string.IsNullOrEmpty(sValue))
                    searchContion += "&" + sType + "=" + sValue;
                else
                    searchContion += "&" + sType + "=1";
            }
            return searchContion;
        }

        protected string GetReplacePage(string url)
        {
            string[] urlArr = url.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            string linkUrl = urlArr[1];
            string[] arrSearch = linkUrl.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            if (linkUrl.ToLower().IndexOf("page=") < 0)
            {
                linkUrl += "&Page=1";
            }
            else
            {
                int i = 0;
                while (i < arrSearch.Length)
                {
                    string[] subArr = arrSearch[i].Split('=');
                    if (subArr[0].ToLower() == "page")
                    {
                        if (linkUrl.IndexOf("&" + arrSearch[i] + "&") > -1)
                            linkUrl = linkUrl.Replace("&" + arrSearch[i], "&Page=1");
                        else if (linkUrl.IndexOf("&" + arrSearch[i]) > -1)
                            linkUrl = linkUrl.Replace("&" + arrSearch[i], "&Page=1");

                        break;
                    }
                    i++;
                }
            }
            return urlArr[0] + "?" + linkUrl;
        }
    }
}