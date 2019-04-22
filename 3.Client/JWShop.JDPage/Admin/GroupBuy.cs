using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Admin
{
    public class GroupBuy:AdminBase
    {
        protected int status = 0;
        protected int pageSize = 6;
        protected List<GroupBuyInfo> dataList = new List<GroupBuyInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 2;
            status = RequestHelper.GetQueryString<int>("status") < -2 ? 0 : RequestHelper.GetQueryString<int>("status");
            GroupBuySearchInfo searchInfo = new GroupBuySearchInfo();
            searchInfo.Status = status;
            dataList = GroupBuyBLL.SearchList(1, pageSize, searchInfo, ref Count);
            dataList.ForEach(k => k.GroupUserName = System.Web.HttpUtility.UrlDecode(k.GroupUserName, System.Text.Encoding.UTF8));
        }
    }
}
