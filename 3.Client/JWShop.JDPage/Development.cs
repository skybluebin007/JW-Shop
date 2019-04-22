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
   public class Development:CommonBasePage
    {
        /// <summary>
        /// 文章详细
        /// </summary>
        protected ArticleInfo article = new ArticleInfo();
       /// <summary>
       /// 发展历程列表
       /// </summary>
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();

        protected ArticleClassInfo thisClass = new ArticleClassInfo();
        protected ArticleClassInfo topClass = new ArticleClassInfo();
       
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();

            articleList = ArticleBLL.SearchList(new ArticleSearchInfo {ClassId="|45|"});
          
            int articleID = RequestHelper.GetQueryString<int>("ID");          
            article = ArticleBLL.Read(articleID);
            if (articleID <= 0 && articleList.Count > 0) article = articleList[0];

            ArticleInfo tmp = article;
            tmp.ViewCount = tmp.ViewCount + 1;

            ArticleBLL.Update(tmp);

            thisClass = ArticleClassBLL.Read(ArticleClassBLL.GetLastClassID(article.ClassId));

            int topClassID = ArticleClassBLL.GetTopClassID(article.ClassId);
            topNav = topClassID;

            topClass = ArticleClassBLL.Read(topClassID);



            

            //SEO
            Title = article.Title;
            Keywords = (article.Keywords == string.Empty) ? article.Title : article.Keywords;
            Description = (article.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}
