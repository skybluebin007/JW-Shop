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
    public class NewsDetail : CommonBasePage
    {
        /// <summary>
        /// 文章详细
        /// </summary>
        protected ArticleInfo article = new ArticleInfo();
        /// <summary>
        /// 文章分类
        /// </summary>
        protected int articleClassID = 0;
        protected ArticleClassInfo thisClass = new ArticleClassInfo();
        protected ArticleClassInfo topClass = new ArticleClassInfo();
        protected int topClassID = 0;
        protected string PreNews = "暂无数据", NextNews = "暂无数据";
        /// <summary>
        /// 新闻分类
        /// </summary>
        protected List<ArticleClassInfo> newsClassList = new List<ArticleClassInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int articleID = RequestHelper.GetQueryString<int>("ID");
            article = ArticleBLL.Read(articleID);

            thisClass = ArticleClassBLL.Read(ArticleClassBLL.GetLastClassID(article.ClassId));

             topNav = topClassID;

          

            if (article.ClassId != string.Empty)
            {
                articleClassID = ArticleClassBLL.GetLastClassID(article.ClassId);
                topClassID = ArticleClassBLL.GetTopClassID(article.ClassId);
                topClass = ArticleClassBLL.Read(topClassID);
            }

            newsClassList = ArticleClassBLL.ReadChilds(64);

            Title = article.Title;
            Keywords = (article.Keywords == string.Empty) ? article.Title : article.Keywords;
            Description = (article.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;

            List<ArticleInfo> nextPreList = new List<ArticleInfo>();
            var arlist = ArticleBLL.SearchListRowNumber(" Id =" + article.Id + "");
            if (arlist.Count>0)
            {
            ArticleInfo thisArtInfo = arlist[0];
            nextPreList = ArticleBLL.SearchListRowNumber(" ClassId Like'%" + article.ClassId + "%' and RowNumber>" + thisArtInfo.RowNumber + " Order by RowNumber asc");


            if (nextPreList.Count > 0)
                PreNews = "<a href=\"/NewsDetail.html?ID=" + nextPreList[0].Id + "\" title=\"" + nextPreList[0].Title + "\">" + StringHelper.Substring(nextPreList[0].Title, 20) + "</a>";

            nextPreList = ArticleBLL.SearchListRowNumber(" ClassId Like'%" + article.ClassId + "%' and RowNumber<" + thisArtInfo.RowNumber + " Order by RowNumber desc");
            if (nextPreList.Count > 0)
                NextNews = "<a href=\"/NewsDetail.html?ID=" + nextPreList[0].Id + "\" title=\"" + nextPreList[0].Title + "\">" + StringHelper.Substring(nextPreList[0].Title, 20) + "</a>";
        }
        }
    }
}