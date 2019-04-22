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

namespace JWShop.Page.Lab
{
    public class About : CommonBasePage
    {
        protected int id;
        protected List<ArticleClassInfo> articleClassList = new List<ArticleClassInfo>();
        protected ArticleInfo article = new ArticleInfo();

        protected override void PageLoad()
        {
            base.PageLoad();

            id = RequestHelper.GetQueryString<int>("id");
            if (id < 1) id = ArticleClass.About;

            int count = 0;
            article = ArticleBLL.SearchArticleList(1, 1, new ArticleSearchInfo { ClassID = "|" + id + "|" }, ref count).FirstOrDefault() ?? new ArticleInfo();
            articleClassList = ArticleClassBLL.ReadArticleClassChildList(ArticleClass.About);

            if (id == ArticleClass.About) id = int.Parse(article.ClassID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Last());

            Title = article.Title;
            Keywords = string.IsNullOrEmpty(article.Keywords) ? article.Title : article.Keywords;
            Description = string.IsNullOrEmpty(article.Summary) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}