using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Admin
{
   public class GroupSign:AdminBase
    {
        protected GroupBuyInfo group = new GroupBuyInfo();
        protected List<GroupSignInfo> dataList = new List<GroupSignInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 2;
            int groupId = RequestHelper.GetQueryString<int>("GroupId");
            group = GroupBuyBLL.Read(groupId);
            dataList = GroupSignBLL.SearchListByGroupId(groupId, 1, group.Quantity, ref Count);
            dataList.ForEach(k => k.UserName = System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
            Title = "参团记录["+group.ProductName+"]";
           
        }
    }
}
