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
    public class ArticleDetail : CommonBasePage
    {        
        /// <summary>
        /// 文章详细
        /// </summary>
        protected ArticleInfo article = new ArticleInfo();
        /// <summary>
        /// 文章分类
        /// </summary>
        protected int articleClassID = 0;
        /// <summary>
        /// 热门新闻
        /// </summary>
        protected List<ArticleInfo> topArticleList = new List<ArticleInfo>();
        /// <summary>
        /// 产品
        /// </summary>
        protected ProductInfo product = new ProductInfo();
        /// <summary>
        /// 产品文章
        /// </summary>
        protected List<ArticleInfo> productArticleList = new List<ArticleInfo>();

        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int articleID = RequestHelper.GetQueryString<int>("ID");

            if (articleID == 1) topNav = 6;
            article = ArticleBLL.Read(articleID);
            
            if (article.ClassId != string.Empty)
            {
                article.ClassId = article.ClassId.Substring(1);
                articleClassID = Convert.ToInt32(article.ClassId.Substring(0, article.ClassId.IndexOf('|')));

                ArticleSearchInfo aSearch = new ArticleSearchInfo();
                aSearch.ClassId = "|" + articleClassID + "|";
                articleList = ArticleBLL.SearchList(aSearch);
            }
           
            Title = article.Title;
            Keywords = (article.Keywords == string.Empty) ? article.Title : article.Keywords;
            Description = (article.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}
