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
    public class Picture : CommonBasePage
    {
        protected string sera_Keywords = string.Empty;
        /// <summary>
        /// 新闻列表
        /// </summary>
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        /// <summary>
        /// 文章分类
        /// </summary>
        protected ArticleClassInfo thisClass = new ArticleClassInfo();
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        protected ArticleClassInfo topClass = new ArticleClassInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            int articleClassID = RequestHelper.GetQueryString<int>("ID");
            sera_Keywords = RequestHelper.GetQueryString<string>("kw");
            if (articleClassID <= 0) articleClassID = 46;//默认企业动态
            thisClass = ArticleClassBLL.Read(articleClassID);
            if (thisClass.ShowType == 1)
            {
                if (ArticleBLL.SearchList(new ArticleSearchInfo { ClassId = "|" + articleClassID + "|" }).Count > 0)
                {
                    if (RequestHelper.RawUrl.ToLower().IndexOf("/mobile/") >= 0)
                    {
                        Response.Redirect("/HZ/Mobile/Product.aspx?id=" + ArticleBLL.SearchList(new ArticleSearchInfo { ClassId = "|" + articleClassID + "|" })[0].Id);
                    }
                    else
                    {
                        Response.Redirect("/HZ/Product.aspx?id=" + ArticleBLL.SearchList(new ArticleSearchInfo { ClassId = "|" + articleClassID + "|" })[0].Id);
                    }
                    Response.End();
                }
            }
            int topClassID = 0;
            ArticleClassBLL.GetTopClassID(articleClassID, ref topClassID);
            topNav = topClassID;
            switch (articleClassID)
            {
                case 54: topNav = 2;
                    break;
                case 55: topNav = 3;
                    break;
                case 56: topNav = 4;
                    break;
                case 57: topNav = 5;
                    break;
                default:
                    break;
            }
            topClass = ArticleClassBLL.Read(topClassID);


            navList = ArticleClassBLL.ReadArticleClassFullFatherID(articleClassID);

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 9;
            if (RequestHelper.RawUrl.ToLower().IndexOf("/mobile") > -1) pageSize = 4;//手机端每页4条
            int count = int.MinValue;
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            if (string.IsNullOrEmpty(sera_Keywords))
            { articleSearch.ClassId = "|" + articleClassID + "|"; }
            else
            {
                //articleSearch.Keywords = keywords;
                articleSearch.Title = sera_Keywords;
                //articleSearch.Keywords = sera_Keywords;
                //articleSearch.InClassId = "58,54,55,56,57";
                articleSearch.ClassId = "|58|";
                #region 添加搜索历史记录


                var historySearch = CookiesHelper.ReadCookieValue("HistorySearch");
                if (("," + historySearch + ",").IndexOf("," + Server.UrlDecode(sera_Keywords) + ",") == -1)
                {
                    if (historySearch == "")
                    {
                        historySearch = Server.UrlDecode(sera_Keywords);
                    }
                    else
                    {
                        historySearch = Server.UrlDecode(sera_Keywords) + "," + historySearch;
                    }
                    if (historySearch.ToString().IndexOf(",") > -1)
                    {
                        if (historySearch.Split(',').Length > 8)
                        {
                            historySearch = historySearch.Substring(0, historySearch.LastIndexOf(","));
                        }
                    }
                    CookiesHelper.AddCookie("HistorySearch", historySearch, 3, TimeType.Day);
                }
                #endregion
            }
            articleList = ArticleBLL.SearchList(currentPage, pageSize, articleSearch, ref count);

            //if (string.IsNullOrEmpty(keywords)) commonPagerClass.URL = "/article-C" + articleClassID + "-P$Page.html";
            //else commonPagerClass.URL = "/article/Keyword/" + keywords + "-P$Page.html";
            commonPagerClass.URL = "/picture-C" + articleClassID + "-P$Page.html";
            commonPagerClass.CurrentPage = currentPage;
            commonPagerClass.PageSize = pageSize;
            commonPagerClass.Count = count;
            commonPagerClass.FirstLastType = true;
            commonPagerClass.FirstPage = "首页";
            commonPagerClass.LastPage = "尾页";

            //SEO
            Title = thisClass.Name;
            Keywords = thisClass.Name;
            Description = thisClass.Description;
        }
    }
}
