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
    public class List : CommonBasePage
    {
        protected string keywords = string.Empty;
        protected ProductClassInfo thisProductClass = new ProductClassInfo();
      
        protected ProductClassInfo currentClass = new ProductClassInfo();
        protected Dictionary<string, ProductClassInfo> levelClass = new Dictionary<string, ProductClassInfo>();
        protected List<ProductClassInfo> showClassList = new List<ProductClassInfo>();
        protected List<ProductBrandInfo> showBrandList = new List<ProductBrandInfo>();
        protected List<ProductTypeAttributeInfo> showAttributeList = new List<ProductTypeAttributeInfo>();
        protected string baseQueryString;
        protected CommonPagerClass pager = new CommonPagerClass();
        protected int currentPage;
        protected int pagecount = 0;
        //商品列表数据
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();
        /// <summary>
        /// 浏览过的商品ID串
        /// </summary>
        protected string strHistoryProduct = string.Empty;
        /// <summary>
        /// 浏览过的商品
        /// </summary>
        protected List<ProductInfo> tempProductList = new List<ProductInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();

            var queryClass = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("cat"))).Split(',');
            int classLevel1 = 0, classLevel2 = 0, classLevel3 = 0;
            if (queryClass.Length > 0) classLevel1 = ShopCommon.ConvertToT<int>(queryClass[0]);
            if (queryClass.Length > 1) classLevel2 = ShopCommon.ConvertToT<int>(queryClass[1]);
            if (queryClass.Length > 2) classLevel3 = ShopCommon.ConvertToT<int>(queryClass[2]);
            int currentClassId = classLevel3 > 0 ? classLevel3 : classLevel2 > 0 ? classLevel2 : classLevel1;
            //if (currentClassId < 1) ResponseHelper.Redirect("/");
            if (currentClassId < 1) currentClassId = 0;
            
            if (classLevel1 > 0)
            {
                switch (classLevel1)
                {
                    case 1:
                        topNav = 2;
                        break;
                    case 2:
                        topNav = 3;
                        break;
                    case 4:
                        topNav = 4;
                        break;
                    case 6:
                        topNav = 5;
                        break;
                }
            }
            //面包屑区域--分类
            showClassList = ProductClassBLL.ReadList();
            currentClass = showClassList.FirstOrDefault(k => k.Id == currentClassId) ?? new ProductClassInfo();
            levelClass.Add("level1", showClassList.FirstOrDefault(k => k.Id == classLevel1) ?? new ProductClassInfo());
            levelClass.Add("level2", showClassList.FirstOrDefault(k => k.Id == classLevel2) ?? new ProductClassInfo());
            levelClass.Add("level3", showClassList.FirstOrDefault(k => k.Id == classLevel3) ?? new ProductClassInfo());
            if (currentClassId < 1) currentClassId = 0;
           
            //热门商品
            int hotCount = 0;
            if (currentClassId > 0)
            {
                hotProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { IsHot = (int)BoolType.True, IsSale = (int)BoolType.True, IsDelete = (int)BoolType.False, ClassId = "|" + currentClassId + "|" }, ref hotCount);
            }
            else
            {
                hotProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { IsHot = (int)BoolType.True, IsSale = (int)BoolType.True, IsDelete=(int)BoolType.False}, ref hotCount);
            }
          

            //商品列表
            currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20; int count = 0;
            string brandIds = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("brand")));
            string attributeIds =StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("at")));
            string attributeValues =StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("ex")));
            string orderField = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("sort")));
            string orderType = "";
            var orderParams = orderField.Split('_');
            if (orderParams.Length > 1)
            {
                orderField = orderParams[0];
                orderType = orderParams[1];
            }
            int minPrice = RequestHelper.GetQueryString<int>("min");
            int maxPrice = RequestHelper.GetQueryString<int>("max");
            keywords =StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("kw")));
            string stf_Keyword = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("stf_Keyword")));
            if (!string.IsNullOrEmpty(stf_Keyword))
            {
                if (string.IsNullOrEmpty(keywords))
                {
                    keywords = stf_Keyword;
                }
            }
            int isNew = RequestHelper.GetQueryString<int>("isNew");
            int isHot = RequestHelper.GetQueryString<int>("isHot");
            int isSpecial = RequestHelper.GetQueryString<int>("isSpecial");
            int isTop = RequestHelper.GetQueryString<int>("isTop");

            productList = ProductBLL.SearchList(currentPage, pageSize, currentClassId, brandIds, attributeIds, attributeValues, orderField, orderType, minPrice, maxPrice, keywords, stf_Keyword, isNew, isHot, isSpecial, isTop, ref count, ref showAttributeList, ref showBrandList);
            pagecount = (int)Math.Round((double)(count + 0.1) / pageSize, 0);
            if (pagecount <= 0) pagecount = 1;
            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
            pager.ShowType = 3;
            #region 如果没有查找到商品则显示RequestHelper.GetQueryString<string>("brand")获取的所有品牌
            if (count <= 0) {
                string[] _brandids = brandIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int[] _showbrandids = Array.ConvertAll<string, int>(_brandids, s=>Convert.ToInt32(s));
                showBrandList = ProductBrandBLL.ReadList(_showbrandids);
            }
            #endregion
            #region 添加热门关键词
            //如果有搜索结果且关键词不为空
            if (productList.Count > 0 && !string.IsNullOrEmpty(keywords)) {
                var theSearchKey=HotSearchKeyBLL.Read(keywords);
                //之前没有则增加
                if (theSearchKey.Id <= 0)
                {
                    theSearchKey.Name = StringHelper.AddSafe(HttpUtility.HtmlEncode(keywords));
                    theSearchKey.SearchTimes = 1;
                    HotSearchKeyBLL.Add(theSearchKey);
                }
                else
                { //有则修改更新搜索次数
                    theSearchKey.SearchTimes +=1;
                    HotSearchKeyBLL.Update(theSearchKey);
                }

            }
#endregion
            if (classLevel3 > 0)
                thisProductClass = ProductClassBLL.Read(classLevel3);
            else if (classLevel2 > 0)
                thisProductClass = ProductClassBLL.Read(classLevel2);
            else
                thisProductClass = ProductClassBLL.Read(classLevel1);

            //浏览过的商品
            strHistoryProduct = Server.UrlDecode(CookiesHelper.ReadCookieValue("HistoryProduct"));
            if (strHistoryProduct.StartsWith(","))
            {
                strHistoryProduct = strHistoryProduct.Substring(1);
            }
            if (strHistoryProduct.EndsWith(","))
            {
                strHistoryProduct = strHistoryProduct.Substring(0, strHistoryProduct.Length - 1);
            }
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.InProductId = strHistoryProduct;
            productSearch.IsSale = 1;
            productSearch.IsDelete = 0;
            tempProductList = ProductBLL.SearchList(productSearch);

            Title = thisProductClass.PageTitle.Trim() == "" ? thisProductClass.Name : thisProductClass.PageTitle;
            Keywords = thisProductClass.PageKeyWord.Trim() == "" ? thisProductClass.Name : thisProductClass.PageKeyWord;
            //Description = thisProductClass.PageSummary.Trim() == "" ? thisProductClass.Remark : thisProductClass.PageSummary;
            Description = thisProductClass.PageSummary;
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
            if (searchContion.IndexOf("?") < 0)
            {
                if (!string.IsNullOrEmpty(sValue))
                    searchContion += "?" + sType + "=" + sValue;
                else
                    searchContion += "?" + sType + "=1";
            }
            else
            {
                if (!hasType)
                {
                    if (!string.IsNullOrEmpty(sValue))
                        searchContion += "&" + sType + "=" + sValue;
                    else
                        searchContion += "&" + sType + "=1";
                }
            }
            return HttpUtility.HtmlEncode(searchContion);
        }

        protected string GetReplacePage(string url)
        {          
                string[] urlArr =url.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
                if (urlArr.Length > 1)
                {
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
                    //return urlArr[0] + "?" + HttpUtility.HtmlEncode(linkUrl);
                    return urlArr[0] + "?" +linkUrl;
                }
                else
                    return urlArr[0] + "?";
            
        }
    }
}