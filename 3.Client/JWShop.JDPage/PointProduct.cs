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
    public class PointProduct : CommonBasePage
    {
        protected CommonPagerClass pager = new CommonPagerClass();
        protected List<PointProductInfo> pointProductList = new List<PointProductInfo>();
        protected UserInfo CurrentUser = new UserInfo();
       
        protected override void PageLoad()
        {
            base.PageLoad();
            if(base.UserId>0) CurrentUser=UserBLL.ReadUserMore(base.UserId);
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 12; int count = 0;

            pointProductList = PointProductBLL.SearchList(currentPage, pageSize, new PointProductSearchInfo { IsSale = (int)BoolType.True, ValidDate = DateTime.Now }, ref count);

            pager.Init(currentPage, pageSize, count, false);
            
            Title = "积分兑换商品";
        }
    }
}
