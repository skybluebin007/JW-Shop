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
    public class UserProductReply : UserBasePage
    {
        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGradeName = string.Empty;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 用户回复
        /// </summary>
        protected List<ProductReplyInfo> productReplyList = new List<ProductReplyInfo>();
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserId);
            userGradeName = UserGradeBLL.Read(base.GradeID).Name;
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            productReplyList = ProductReplyBLL.ReadProductReplyList(currentPage, pageSize, ref count, base.UserId);
            commonPagerClass.CurrentPage = currentPage;
            commonPagerClass.PageSize = pageSize;
            commonPagerClass.Count = count;
            commonPagerClass.FirstPage = "<<首页";
            commonPagerClass.PreviewPage = "<<上一页";
            commonPagerClass.NextPage = "下一页>>";
            commonPagerClass.LastPage = "末页>>";
            commonPagerClass.ListType = false;
            commonPagerClass.DisCount = false;
            commonPagerClass.PrenextType = true;
        }         
    }
}
