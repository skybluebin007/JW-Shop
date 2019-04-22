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

namespace JWShop.Page
{
    public class NewsAjax : AjaxBasePage
    {        
        protected int id = 0;
        /// <summary>
        /// 新闻列表
        /// </summary>
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        /// <summary>
        /// 热门新闻
        /// </summary>
        protected List<ArticleInfo> topArticleList = new List<ArticleInfo>();
        /// <summary>
        /// 分页
        /// </summary>
        protected AjaxPagerClass commonPagerClass = new AjaxPagerClass();

        protected ArticleClassInfo curArticleClass = new ArticleClassInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {

            base.PageLoad();

            ArticleSearchInfo articleSearch = new ArticleSearchInfo();

            id = RequestHelper.GetQueryString<int>("ID");
            if (id <= 0) id = 64;
            curArticleClass = ArticleClassBLL.Read(id);

            articleSearch.ClassId = "|" + curArticleClass.Id + "|";
            //articleSearch.IsTop = (int)BoolType.True;
            int count = int.MinValue;
            topArticleList = ArticleBLL.SearchList(1, 15, articleSearch, ref count);

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            count = 0;
            if (id > 0)
            {
                articleSearch.ClassId = "|" + id + "|";
            }
            articleList = ArticleBLL.SearchList(currentPage, pageSize, articleSearch, ref count);

            //commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            commonPagerClass.CurrentPage = currentPage;
            commonPagerClass.PageSize = pageSize;
            commonPagerClass.Count = count;
            commonPagerClass.FirstPage = "<<首页";
            commonPagerClass.PreviewPage = "<<上一页";
            commonPagerClass.NextPage = "下一页>>";
            commonPagerClass.LastPage = "末页>>";
            commonPagerClass.ListType = false;
            commonPagerClass.DisCount = false;
            commonPagerClass.PrenextType = true;

            //Title = "新闻资讯";
        }
    }
}
