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

namespace JWShop.Page.Mobile
{
    public class About : CommonBasePage
    {
        protected ArticleInfo article = new ArticleInfo();
        protected override void PageLoad()
        {
            base.PageLoad();

            int count = int.MinValue;
            List<ArticleInfo> tempList = ArticleBLL.SearchList(1, 1, new ArticleSearchInfo { ClassId = "|3|", IsTop = 1 }, ref count);
            if (tempList.Count > 0) article = tempList[0];

            Title = "公司简介";
        }
    }
}
