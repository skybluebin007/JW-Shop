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

namespace JWShop.Page.Lab
{
    public class Help : CommonBasePage
    {
        protected int id = 0;
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected List<ArticleClassInfo> helpClassList = new List<ArticleClassInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            helpClassList = ArticleClassBLL.ReadArticleClassChildList(ArticleClass.Help);

            id = RequestHelper.GetQueryString<int>("id");
            if (id == int.MinValue && helpClassList.Count > 0)
            {
                id = ArticleClassBLL.ReadArticleClassChildList(helpClassList[0].ID)[0].ID;
            }

            if (id > 0)
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.ClassID = "|" + id.ToString() + "|";
                articleList = ArticleBLL.SearchArticleList(articleSearch);
            }

            Title = "帮助中心";
        }
    }
}
