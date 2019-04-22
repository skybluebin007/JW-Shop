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
    public class News : CommonBasePage
    {        
        protected int id = 0;
        /// <summary>
        /// 文章分类
        /// </summary>
        protected ArticleClassInfo thisClass = new ArticleClassInfo();
        /// <summary>
        /// 新闻分类
        /// </summary>
        protected List<ArticleClassInfo> newsClassList = new List<ArticleClassInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {

            base.PageLoad();
            id = RequestHelper.GetQueryString<int>("ID");
            if (id <= 0) id = 38;
            thisClass = ArticleClassBLL.Read(id);

            newsClassList = ArticleClassBLL.ReadChilds(id);

            Title = thisClass.Name;
            Keywords =  thisClass.Name ;
            Description = thisClass.Description;
        }
    }
}
