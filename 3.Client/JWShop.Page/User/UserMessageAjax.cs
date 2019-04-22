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
    public class UserMessageAjax : UserAjaxBasePage
    {
        /// <summary>
        /// 留言列表
        /// </summary>
        protected List<UserMessageInfo> userMessageList = new List<UserMessageInfo>();
        /// <summary>
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "AddUserMessage")
            {
                AddUserMessage();
            }
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 15;
            int count = 0;
            UserMessageSeachInfo userMessageSeach = new UserMessageSeachInfo();
            userMessageSeach.UserId = base.UserId;
            userMessageList = UserMessageBLL.SearchList(currentPage, pageSize, userMessageSeach, ref count);
            ajaxPagerClass.CurrentPage = currentPage;
            ajaxPagerClass.PageSize = pageSize;
            ajaxPagerClass.Count = count;
            ajaxPagerClass.FirstPage = "<<首页";
            ajaxPagerClass.PreviewPage = "<<上一页";
            ajaxPagerClass.NextPage = "下一页>>";
            ajaxPagerClass.LastPage = "末页>>";
            ajaxPagerClass.ListType = false;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.PrenextType = true;
          

        }
        /// <summary>
        /// 添加留言
        /// </summary>
        protected void AddUserMessage()
        {
            string result = string.Empty;
            int messageClass = RequestHelper.GetQueryString<int>("MessageClass");
            string title = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Title"));
            string content = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Content"));
            if (content == string.Empty || content == string.Empty)
            {
                result = "请填写标题和内容";
            }
            else
            {
                UserMessageInfo userMessage = new UserMessageInfo();
                userMessage.MessageClass = messageClass;
                userMessage.Title = title;
                userMessage.Content = content;
                userMessage.UserIP = ClientHelper.IP;
                userMessage.PostDate = RequestHelper.DateNow;
                userMessage.IsHandler = (int)BoolType.False;
                userMessage.AdminReplyContent = string.Empty;
                userMessage.AdminReplyDate = RequestHelper.DateNow;
                userMessage.UserId = base.UserId;
                userMessage.UserName = base.UserName;
                UserMessageBLL.Add(userMessage);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}
