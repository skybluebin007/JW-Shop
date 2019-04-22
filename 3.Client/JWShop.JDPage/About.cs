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
    public class About : CommonBasePage
    {
        protected int id;
        protected List<ArticleClassInfo> articleClassList = new List<ArticleClassInfo>();
        protected ArticleInfo article = new ArticleInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 9;
            id = RequestHelper.GetQueryString<int>("id");
            if (id <=0) id =63;//关于我们

            int count = 0;
            article = ArticleBLL.SearchList(1, 1, new ArticleSearchInfo { ClassId = "|" + id + "|" }, ref count).FirstOrDefault() ?? new ArticleInfo();
            articleClassList = ArticleClassBLL.ReadChilds(63);

            if (id ==63) id = int.Parse(article.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Last());

            Title = article.Title;
            Keywords = string.IsNullOrEmpty(article.Keywords) ? article.Title : article.Keywords;
            Description = string.IsNullOrEmpty(article.Summary) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}