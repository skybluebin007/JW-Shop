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
    public class HelpDetail : CommonBasePage
    {
      
        protected int id = 0;        
        protected ArticleInfo article = new ArticleInfo();
        protected List<ArticleInfo> hotHelpList = new List<ArticleInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 8;
           

            id = RequestHelper.GetQueryString<int>("id");
            if (id <= 0) Response.Redirect("/HelpCenter.html");
            article = ArticleBLL.Read(id);
            int count = int.MinValue;
            hotHelpList = ArticleBLL.SearchList(1, 6, new ArticleSearchInfo { IsTop = 1, ClassId = "|" + ArticleClass.Help + "|" }, ref count);

            Title = article.Title;
            Keywords = string.IsNullOrEmpty(article.Keywords) ? article.Title : article.Keywords;
            Description = string.IsNullOrEmpty(article.Summary) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}