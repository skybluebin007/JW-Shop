using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SocoShop.Common;
using SocoShop.Business;
using SocoShop.Entity;
using SkyCES.EntLib;

namespace SocoShop.Page
{
    public class UserTags : UserBasePage
    {
        /// <summary>
        /// 用户标签
        /// </summary>
        protected List<TagsInfo> tagsList = new List<TagsInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            TagsSearchInfo tagsSearch = new TagsSearchInfo();
            tagsSearch.UserID = base.UserID;
            tagsList = TagsBLL.SearchTagsList(tagsSearch);
        }      
    }
}
