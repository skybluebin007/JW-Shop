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

        protected ArticleClassInfo thisClass = new ArticleClassInfo();
        protected ArticleClassInfo topClass = new ArticleClassInfo();
        protected string PreNews = "没有了", NextNews = "没有了";
        protected string PreNewsM = "没有了", NextNewsM = "没有了";
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int articleID = RequestHelper.GetQueryString<int>("ID");
            if (articleID == 60)
            {
                topNav = 6;
                int counts = 0;
                if ((ArticleBLL.SearchList(1, 1, new ArticleSearchInfo { ClassId = "|60|" }, ref counts).Count > 0))
                {
                    articleID = ArticleBLL.SearchList(1, 1, new ArticleSearchInfo { ClassId = "|60|" }, ref counts)[0].Id;

                }
            }
            article = ArticleBLL.Read(articleID);
            ArticleInfo tmp = article;
            tmp.ViewCount = tmp.ViewCount + 1;

            ArticleBLL.Update(tmp);

            thisClass = ArticleClassBLL.Read(ArticleClassBLL.GetLastClassID(article.ClassId));

            int topClassID = ArticleClassBLL.GetTopClassID(article.ClassId);
            topNav = topClassID;

            topClass = ArticleClassBLL.Read(topClassID);

            if (thisClass.Id == 60)
            {
                topNav = 6;
            }

            string theArticleClassID = article.ClassId;
            int lastClassID = int.MinValue;
            if (theArticleClassID != string.Empty)
            {
                theArticleClassID = theArticleClassID.Substring(1);
                lastClassID = Convert.ToInt32(theArticleClassID.Substring(0, theArticleClassID.IndexOf('|')));
            }

            navList = ArticleClassBLL.ReadArticleClassFullFatherID(ArticleClassBLL.GetLastClassID(article.ClassId));
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();


            List<ArticleInfo> nextPreList = new List<ArticleInfo>();
            if (ArticleBLL.SearchListRowNumber(" ID =" + article.Id + "").Count > 0)
            {
                ArticleInfo thisArtInfo = ArticleBLL.SearchListRowNumber(" ID =" + article.Id + "")[0];
                nextPreList = ArticleBLL.SearchListRowNumber(" [ClassID] Like'%" + article.ClassId + "%' and [RowNumber]>" + thisArtInfo.RowNumber + " Order by RowNumber asc");


                if (nextPreList.Count > 0)
                {
                    NextNews = "<a href=\"/articledetail-I" + nextPreList[0].Id + ".html\" title=\"" + nextPreList[0].Title + "\">" + StringHelper.Substring(nextPreList[0].Title, 20) + "</a>";
                    NextNewsM = "<a href=\"/mobile/articledetail-I" + nextPreList[0].Id + ".html\" title=\"" + nextPreList[0].Title + "\">" + StringHelper.Substring(nextPreList[0].Title, 13) + "</a>";
                }

                nextPreList = ArticleBLL.SearchListRowNumber(" ClassID Like'%" + article.ClassId + "%' and RowNumber<" + thisArtInfo.RowNumber + " Order by RowNumber desc");
                if (nextPreList.Count > 0)
                {
                    PreNews = "<a href=\"/articledetail-I" + nextPreList[0].Id + ".html\" title=\"" + nextPreList[0].Title + "\">" + StringHelper.Substring(nextPreList[0].Title, 20) + "</a>";
                    PreNewsM = "<a href=\"/mobile/articledetail-I" + nextPreList[0].Id + ".html\" title=\"" + nextPreList[0].Title + "\">" + StringHelper.Substring(nextPreList[0].Title, 13) + "</a>";
                }
            }
           
            //SEO
            Title = article.Title;
            Keywords = (article.Keywords == string.Empty) ? article.Title : article.Keywords;
            Description = (article.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}
