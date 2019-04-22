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
            string userMessageCookies = CookiesHelper.ReadCookieValue("UserMessageCookies" + base.UserId.ToString());
            if (content == string.Empty || content == string.Empty)
            {
                result = "请填写标题和内容";
            }
            else
            {
                if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0  &&  !string.IsNullOrEmpty(userMessageCookies))
                {
                    string[] strArray = userMessageCookies.Split(new char[] { '|' });
                    string _userId = strArray[0];
                    string  _title= strArray[1];
                    string _content = strArray[2];
                    //如果该用户在限制时间内提交过相同title或content的内容，则不能再频繁提交
                    if (_userId == base.UserId.ToString() && (title == Server.UrlDecode(_title) || content == (Server.UrlDecode(_content))))
                    {
                        result = "请不要频繁提交相似留言";
                    }
                }
            }
            if(string.IsNullOrEmpty(result))
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
                    if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0)
                    {
                        string cookieValue = base.UserId + "|" + Server.UrlEncode(title) + "|" + Server.UrlEncode(content);
                        CookiesHelper.AddCookie("UserMessageCookies" + base.UserId.ToString(), cookieValue, ShopConfig.ReadConfigInfo().CommentRestrictTime, TimeType.Second);
                    }
                }
          
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}
