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
  public class News :CommonBasePage
  {/// <summary>
      /// 新闻列表
      /// </summary>
      protected List<ArticleInfo> articleList = new List<ArticleInfo>();
      /// <summary>
      /// 文章分类
      /// </summary>
      protected ArticleClassInfo thisClass = new ArticleClassInfo();
      /// <summary>
      /// 关键词搜索
      /// </summary>
      protected string keywords = string.Empty;
      /// <summary>
      /// 文章分类的顶级分类
      /// </summary>
      protected ArticleClassInfo topClass = new ArticleClassInfo();
      protected  int articleClassID=int.MinValue;
      /// <summary>
      /// 每页显示条数
      /// </summary>
      protected int pageSize = 4;
      protected override void PageLoad()
      {
          base.PageLoad();
          articleClassID = RequestHelper.GetQueryString<int>("ID");
          keywords = RequestHelper.GetQueryString<string>("Keywords");
          if (articleClassID <= 0) articleClassID = 64;//默认竞网快报
          thisClass = ArticleClassBLL.Read(articleClassID);

          int topClassID = 0;
          ArticleClassBLL.GetTopClassID(articleClassID, ref topClassID);
          topNav = topClassID;

          topClass = ArticleClassBLL.Read(topClassID);


          int currentPage = RequestHelper.GetQueryString<int>("Page");
          if (currentPage < 1)
          {
              currentPage = 1;
          }
      
          int count = int.MinValue;
          ArticleSearchInfo articleSearch = new ArticleSearchInfo();
          if (string.IsNullOrEmpty(keywords))
          { articleSearch.ClassId = "|" + articleClassID + "|"; }
          else
          {
              //articleSearch.Key = keywords;
              articleSearch.Title = keywords;
              //articleSearch.Keywords = keywords;
              //articleSearch.InClassId = "38,44,47";//只搜索 指定的分类
          }
          articleList = ArticleBLL.SearchList(1, pageSize, articleSearch, ref count);

        
          //SEO
          if (string.IsNullOrEmpty(keywords))
          {
              Title = thisClass.Name;
          }
          else {
              Title ="搜索结果";
          }
          Keywords = thisClass.Name;
          Description = thisClass.Description;
      }
    }
}
