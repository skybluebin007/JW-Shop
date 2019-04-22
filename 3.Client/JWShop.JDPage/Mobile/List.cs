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

namespace JWShop.Page.Mobile
{
    public class List : CommonBasePage
    {
        protected ProductClassInfo currentClass = new ProductClassInfo();
        protected Dictionary<string, ProductClassInfo> levelClass = new Dictionary<string, ProductClassInfo>();
        protected List<ProductClassInfo> showClassList = new List<ProductClassInfo>();
        protected List<ProductBrandInfo> showBrandList = new List<ProductBrandInfo>();
        protected List<ProductTypeAttributeInfo> showAttributeList = new List<ProductTypeAttributeInfo>();
        protected string baseQueryString;
        protected CommonPagerClass pager = new CommonPagerClass();
        protected int currentPage;

        //商品列表数据
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            var queryClass = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("cat"))).Split(',');
            int classLevel1 = 0, classLevel2 = 0, classLevel3 = 0;
            if (queryClass.Length > 0) classLevel1 = ShopCommon.ConvertToT<int>(queryClass[0]);
            if (queryClass.Length > 1) classLevel2 = ShopCommon.ConvertToT<int>(queryClass[1]);
            if (queryClass.Length > 2) classLevel3 = ShopCommon.ConvertToT<int>(queryClass[2]);
            int currentClassId = classLevel3 > 0 ? classLevel3 : classLevel2 > 0 ? classLevel2 : classLevel1;
            if (currentClassId < 0) currentClassId = 0;
            //if (currentClassId < 1) ResponseHelper.Redirect("/");

            //面包屑区域--分类
            showClassList = ProductClassBLL.ReadList();
            currentClass = showClassList.FirstOrDefault(k => k.Id == currentClassId) ?? new ProductClassInfo();
            levelClass.Add("level1", showClassList.FirstOrDefault(k => k.Id == classLevel1) ?? new ProductClassInfo());
            levelClass.Add("level2", showClassList.FirstOrDefault(k => k.Id == classLevel2) ?? new ProductClassInfo());
            levelClass.Add("level3", showClassList.FirstOrDefault(k => k.Id == classLevel3) ?? new ProductClassInfo());

            //热门商品
            int hotCount = 0;
            hotProductList = ProductBLL.SearchList(1, 7, new ProductSearchInfo { IsHot = (int)BoolType.True, IsSale = (int)BoolType.True }, ref hotCount);

            //商品列表
            currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 6; int count = 0;
            string brandIds = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("brand")));
            string attributeIds = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("at")));
            string attributeValues = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("ex").Trim()));
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
            string keywords = StringHelper.AddSafe(HttpUtility.UrlDecode(RequestHelper.GetQueryString<string>("kw")));
            #region 添加到搜索记录
            if (keywords != string.Empty)
            {
                var historySearch =Server.UrlDecode(CookiesHelper.ReadCookieValue("HistorySearch"));
                if (("," + historySearch + ",").IndexOf("," + Server.UrlDecode(keywords) + ",") == -1)
                {
                    if (historySearch == "")
                    {
                        historySearch = Server.UrlDecode(keywords);
                    }
                    else
                    {
                        historySearch = Server.UrlDecode(keywords) + "," + historySearch;
                    }
                    if (historySearch.ToString().IndexOf(",") > -1)
                    {
                        if (historySearch.Split(',').Length > 8)
                        {
                            historySearch = historySearch.Substring(0, historySearch.LastIndexOf(","));
                        }
                    }
                    CookiesHelper.AddCookie("HistorySearch",Server.UrlEncode(historySearch));
                }
            }
            #endregion
            int isNew = RequestHelper.GetQueryString<int>("isNew");
            int isHot = RequestHelper.GetQueryString<int>("isHot");
            int isSpecial = RequestHelper.GetQueryString<int>("isSpecial");
            int isTop = RequestHelper.GetQueryString<int>("isTop");

            productList = ProductBLL.SearchList(currentPage, pageSize, currentClassId, brandIds, attributeIds, attributeValues, orderField, orderType, minPrice, maxPrice, keywords,"", isNew, isHot, isSpecial, isTop, ref count, ref showAttributeList, ref showBrandList);

            //手机端特殊，显示所有当前产品类型下的品牌和属性，不会判断有无产品
            if (currentClassId > 0)
            {
                //取得当前分类所属类型
                ProductTypeInfo productType = ProductTypeBLL.Read(ProductClassBLL.Read(currentClassId).ProductTypeId);
                showBrandList = ProductBrandBLL.ReadList(Array.ConvertAll<string, int>(productType.BrandIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)));
                showAttributeList = ProductTypeAttributeBLL.ReadList(productType.Id);
            }
            #region 添加热门关键词
            //如果有搜索结果且关键词不为空
            if (productList.Count > 0 && !string.IsNullOrEmpty(keywords))
            {
                var theSearchKey = HotSearchKeyBLL.Read(keywords);
                //之前没有则增加
                if (theSearchKey.Id <= 0)
                {
                    theSearchKey.Name = keywords;
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
            ProductClassInfo thisProductClass = new ProductClassInfo();
            if (classLevel3 > 0)
                thisProductClass = ProductClassBLL.Read(classLevel3);
            else if (classLevel2 > 0)
                thisProductClass = ProductClassBLL.Read(classLevel2);
            else
                thisProductClass = ProductClassBLL.Read(classLevel1);

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

        protected void SetAttributeValue(List<string> idArr, List<string> valArr, string val, ref string idval, ref string valval)
        {
            bool hasit = false;
            for (int i = 0; i < idArr.Count; i++)
            {
                if (val == idArr[i])
                {
                    idval = idArr[i];
                    valval = valArr[i];
                    hasit = true;
                    break;
                }
            }

            if (!hasit)
            {
                idval = "0";
                valval = " ";
            }
        }

        protected void GetBrandValue(List<string> idArr, List<string> valArr, string val, ref string idval, ref string valval)
        {
            bool hasit = false;
            for (int i = 0; i < idArr.Count; i++)
            {
                if (val == idArr[i])
                {
                    idval = idArr[i];
                    valval = valArr[i];
                    hasit = true;
                    break;
                }
            }

            if (!hasit)
            {
                idval = "0";
                valval = " ";
            }
        }
    }
}