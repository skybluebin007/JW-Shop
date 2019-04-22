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
   public class Picture:CommonBasePage
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        /// <summary>
        /// 文章分类
        /// </summary>
        protected ArticleClassInfo thisClass = new ArticleClassInfo();
       
        protected ArticleClassInfo topClass = new ArticleClassInfo();

        protected int articleClassID = int.MinValue;

        protected override void PageLoad()
        {
            base.PageLoad();
           articleClassID = RequestHelper.GetQueryString<int>("ID");

            if (articleClassID <= 0) articleClassID = 46;//默认企业动态
            thisClass = ArticleClassBLL.Read(articleClassID);

            int topClassID = 0;
            ArticleClassBLL.GetTopClassID(articleClassID, ref topClassID);
            topNav = topClassID;

            topClass = ArticleClassBLL.Read(topClassID);


            navList = ArticleClassBLL.ReadArticleClassFullFatherID(articleClassID);

           
            int count = int.MinValue;
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            //if (string.IsNullOrEmpty(keywords))
            articleSearch.ClassId = "|" + articleClassID + "|";
            //else
            //{
            //    //articleSearch.Key = keywords;
            //    articleSearch.Title = keywords;
            //    //articleSearch.Keywords = keywords;
            //    articleSearch.InClassId = "38,44,46,47";
            //}
            articleList = ArticleBLL.SearchList(1, 4, articleSearch, ref count);

            //if (string.IsNullOrEmpty(keywords)) commonPagerClass.URL = "/article-C" + articleClassID + "-P$Page.html";
            //else commonPagerClass.URL = "/article/Keyword/" + keywords + "-P$Page.html";


            //SEO
            Title = thisClass.Name;
            Keywords = thisClass.Name;
            Description = thisClass.Description;
        }
    }
}
