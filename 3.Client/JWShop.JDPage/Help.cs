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
    public class Help : CommonBasePage
    {
        protected int id = 0;
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected CommonPagerClass pagerclass = new CommonPagerClass();
        protected string fullClassId = string.Empty;
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 8;

           
            id = RequestHelper.GetQueryString<int>("id");
            if (id <= 0) id = ArticleClass.Help;//默认帮助中心 
            var chlidList = ArticleClassBLL.ReadChilds(ArticleClass.Help);
            if (id == ArticleClass.Help && chlidList.Count > 0) id = chlidList[0].Id;
            fullClassId = ArticleClassBLL.ReadArticleClassFullFatherID(id);
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
            int count = 0;
            articleList = ArticleBLL.SearchList(currentPage, pageSize, new ArticleSearchInfo { ClassId = "|" + id + "|" }, ref count);
            pagerclass.CurrentPage = currentPage;
            pagerclass.PageSize = pageSize;
            pagerclass.Count = count;
            pagerclass.FirstPage = "<<首页";
            pagerclass.PreviewPage = "<<上一页";
            pagerclass.NextPage = "下一页>>";
            pagerclass.LastPage = "末页>>";
            pagerclass.ListType = false;
            pagerclass.DisCount = false;
            pagerclass.PrenextType = true;
            pagerclass.URL = "/Help-C" + id + "-P$Page.html";

            Title = ArticleClassBLL.Read(id).Name;
            //Keywords = string.IsNullOrEmpty(article.Keywords) ? article.Title : article.Keywords;
            //Description = string.IsNullOrEmpty(article.Summary) ? StringHelper.Substring(StringHelper.KillHTML(article.Content), 200) : article.Summary;
        }
    }
}