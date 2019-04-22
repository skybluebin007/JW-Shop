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
    public class Help : CommonBasePage
    {
        protected int id = 0;
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected List<ArticleClassInfo> helpClassList = new List<ArticleClassInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 8;

            helpClassList = ArticleClassBLL.ReadChilds(ArticleClass.Help);

            id = RequestHelper.GetQueryString<int>("id");
            if (id == int.MinValue && helpClassList.Count > 0)
            {
                id = ArticleClassBLL.ReadChilds(helpClassList[0].Id)[0].Id;
            }

            if (id > 0)
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.ClassId = "|" + id.ToString() + "|";
                articleList = ArticleBLL.SearchList(articleSearch);
            }

            Title = "帮助中心";
        }
    }
}