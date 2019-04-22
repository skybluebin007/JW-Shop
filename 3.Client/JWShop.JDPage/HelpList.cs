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

namespace JWShop.JDPage
{
    public class HelpList : CommonBasePage
    {
        protected int artId = 0;
        protected int fatherID = 0;
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
                id = helpClassList[0].Id;
            }

            if (id > 0)
            {
                if (ArticleClassBLL.Read(id).ParentId == 2)
                {
                    fatherID = id;
                    artId = ArticleClassBLL.ReadChilds(id)[0].Id;
                }
                else
                {
                    fatherID = ArticleClassBLL.Read(id).ParentId;
                    artId = id;
                }
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.ClassId = "|" + artId.ToString() + "|";
                articleList = ArticleBLL.SearchList(articleSearch);
            }

            Title = "帮助中心";
        }
    }
}