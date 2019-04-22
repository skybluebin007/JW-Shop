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
    public class UserCollect : UserBasePage
    {
        protected List<ProductCollectInfo> ProductCollectList = new List<ProductCollectInfo>();
        protected CommonPagerClass pager = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Delete")
            {
                string deleteId = RequestHelper.GetForm<string>("id");
                ProductCollectBLL.Delete(Array.ConvertAll<string, int>(deleteId.Split(','), k => Convert.ToInt32(k)), base.UserId);
                ResponseHelper.Write("ok");
                ResponseHelper.End();
            }

            ProductCollectList = ProductCollectBLL.ReadListByUserId(base.UserId);
            int count = ProductCollectList.Count;
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            ProductCollectList = ProductCollectList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
        }
    }
}