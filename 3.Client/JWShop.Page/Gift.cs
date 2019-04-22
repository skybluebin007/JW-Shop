using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace JWShop.Page
{
    public class Gift : CommonBasePage
    {
        protected CommonPagerClass pager = new CommonPagerClass();
        protected List<PointProductInfo> GiftList = new List<PointProductInfo>();

        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
            int count = 0;

            GiftList = PointProductBLL.SearchList(currentPage, pageSize, new PointProductSearchInfo { }, ref count);

            pager.CurrentPage = currentPage;
            pager.PageSize = pageSize;
            pager.Count = count;

            pager.CurrentPage = currentPage;
            pager.PageSize = pageSize;
            pager.Count = count;
            if (!string.IsNullOrEmpty(isMobile))
            {
                pager.ShowType = 4;
                pager.PagerCSS = "page_number";
                pager.PreviewPage = "上一页";
                pager.NextPage = "下一页";
                pager.URL = "/mobile/Gift.aspx?Page=$Page";
            }
            else
            {
                pager.ShowType = 2;
                pager.PagerCSS = "pagin";
                pager.CurrentCSS = "current";
                pager.PreviewPage = "<<上一页";
                pager.NextPage = "下一页>>";
                pager.URL = "gift.html?Page=$Page";
            }

        }
    }
}