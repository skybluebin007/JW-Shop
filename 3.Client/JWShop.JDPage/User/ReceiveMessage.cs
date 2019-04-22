using System;
using System.Data;
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
   public class ReceiveMessage : UserBasePage
    {
       protected List<ReceiveMessageInfo> msgList = new List<ReceiveMessageInfo>();
      
        protected CommonPagerClass pager = new CommonPagerClass();
        protected int isRead = int.MinValue;

        protected int allCount = int.MinValue;
        protected int readCount = int.MinValue;
       
        protected override void PageLoad()
        {
            base.PageLoad();
            isRead = RequestHelper.GetQueryString<int>("isread");
            int currentPage = RequestHelper.GetQueryString<int>("Page");
      
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
           int count = 0;
            ReceiveMessageSearchInfo searchInfo = new ReceiveMessageSearchInfo();
            searchInfo.UserID = base.UserId;
            searchInfo.IsRead = isRead;
            msgList = ReceiveMessageBLL.SearchList(currentPage, pageSize, searchInfo, ref count);
            
        
            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            //数量统计
            var msgAllList = ReceiveMessageBLL.SearchList(new ReceiveMessageSearchInfo {UserID = base.UserId });
            allCount = msgAllList.Count;
            readCount = msgAllList.Where(m => m.IsRead == 1).Count();

            Title = "我的消息";
        }
    }
}
