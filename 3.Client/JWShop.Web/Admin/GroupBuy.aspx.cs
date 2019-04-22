using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.Common;
using Newtonsoft.Json;

namespace JWShop.Web.Admin
{
    public partial class GroupBuy :JWShop.Page.AdminBasePage
    {
        protected int status = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadGroupBuy", PowerCheckType.Single);
             

                status = RequestHelper.GetQueryString<int>("status")<-2?0:RequestHelper.GetQueryString<int>("status");
                GroupBuySearchInfo searchInfo = new GroupBuySearchInfo();
                searchInfo.Status = status;
                List<GroupBuyInfo> dataList = GroupBuyBLL.SearchList(CurrentPage, PageSize, searchInfo, ref Count);
                dataList.ForEach(k => k.GroupUserName = HttpUtility.UrlDecode(k.GroupUserName, System.Text.Encoding.UTF8));
                BindControl(dataList, RecordList, MyPager);
            
            }
        }

       
    }
}