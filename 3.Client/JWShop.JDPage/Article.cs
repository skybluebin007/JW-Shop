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
  public  class Article:CommonBasePage
    {
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

        /// <summary>
        /// 关键词搜索
        /// </summary>
        protected string keywords = string.Empty;
      protected override void PageLoad()
      {
          base.PageLoad();
          int articleClassID = RequestHelper.GetQueryString<int>("ID");
          keywords = RequestHelper.GetQueryString<string>("Keywords");
          if (articleClassID <= 0) articleClassID = 2;//默认企业动态
          thisClass = ArticleClassBLL.Read(articleClassID);

          int topClassID = 0;
          ArticleClassBLL.GetTopClassID(articleClassID, ref topClassID);
          topNav = topClassID;
     
          topClass = ArticleClassBLL.Read(topClassID);

          //ArticleClassBLL.ReadChilds();
          navList = ArticleClassBLL.ReadArticleClassFullFatherID(articleClassID);

          int currentPage = RequestHelper.GetQueryString<int>("Page");
          if (currentPage < 1)
          {
              currentPage = 1;
          }
          int pageSize = 4;
          if (RequestHelper.RawUrl.ToLower().IndexOf("/mobile") > -1) pageSize = 4;//手机端每页4条
          int count = int.MinValue;
          ArticleSearchInfo articleSearch = new ArticleSearchInfo();
          if (string.IsNullOrEmpty(keywords))
          { articleSearch.ClassId = "|" + articleClassID + "|"; }
          else
          {
              //articleSearch.Key = keywords;
              articleSearch.Title = keywords;
              //articleSearch.Keywords = keywords;
              articleSearch.InClassId = "38,44,47";//只搜索 企业动态  养老政策  安华公益
          }
          articleList = ArticleBLL.SearchList(currentPage, pageSize, articleSearch, ref count);

          if (string.IsNullOrEmpty(keywords)) commonPagerClass.URL = "/article-C" + articleClassID + "-P$Page.html";
          else commonPagerClass.URL = "/article/Keyword/" + keywords + "-P$Page.html";
          commonPagerClass.CurrentPage = currentPage;
          commonPagerClass.PageSize = pageSize;
          commonPagerClass.Count = count;
          commonPagerClass.FirstLastType = true;
          commonPagerClass.FirstPage = "首页";
          commonPagerClass.LastPage = "尾页";
          //SEO
          if (string.IsNullOrEmpty(keywords))
          {
              Title = thisClass.Name;
          }
          else
          {
              Title = "搜索结果";
          }
          Keywords = thisClass.Name;
          Description = thisClass.Description;
      }
    }
}
