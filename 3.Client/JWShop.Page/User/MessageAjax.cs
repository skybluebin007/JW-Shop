using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SocoShop.Common;
using SocoShop.Business;
using SocoShop.Entity;
using SkyCES.EntLib;

namespace SocoShop.Page
{
    public class MessageAjax : UserAjaxBasePage
    {
        /// <summary>
        /// 操作
        /// </summary>
        protected string action = string.Empty;
        /// <summary>
        /// 收信列表
        /// </summary>
        protected List<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
        /// <summary>
        /// 发信列表
        /// </summary>
        protected List<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
        /// <summary>
        /// 好友列表
        /// </summary>
        protected List<UserFriendInfo> userFriendList = new List<UserFriendInfo>();
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
            action = RequestHelper.GetQueryString<string>("Action");
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 8;
            int count = 0;
            switch (action)
            {
                case "ReceiveMessage":
                    ReceiveMessageSearchInfo receiveMessageSearch = new ReceiveMessageSearchInfo();
                    receiveMessageSearch.UserID = base.UserID;
                    receiveMessageList = ReceiveMessageBLL.SearchReceiveMessageList(currentPage, pageSize, receiveMessageSearch, ref count);
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
                    break;
                case "SendMessage":
                    SendMessageSearchInfo sendMessageSearch = new SendMessageSearchInfo();
                    sendMessageSearch.UserID = base.UserID;
                    sendMessageList = SendMessageBLL.SearchSendMessageList(currentPage, pageSize, sendMessageSearch, ref count);
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
                    break;
                case "WriteMessage":
                    UserFriendSearchInfo userFriendSearch = new UserFriendSearchInfo();
                    userFriendSearch.UserID = base.UserID;
                    userFriendList = UserFriendBLL.SearchUserFriendList(userFriendSearch);
                    break;
                case "SearchFriend":
                    SearchFriend();
                    break;
                case "SendUserMessage":
                    SendUserMessage();
                    break;
                case "DeleteReceiveMessage":
                    DeleteReceiveMessage();
                    break;
                case "DeleteSendMessage":
                    DeleteSendMessage();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 删除收件箱
        /// </summary>
        protected void DeleteReceiveMessage()
        {
            string selectID = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("SelectID"));
            ReceiveMessageBLL.DeleteReceiveMessage(selectID, base.UserID);
            ResponseHelper.Write("ok");
            ResponseHelper.End();
        }
        /// <summary>
        /// 删除发信箱
        /// </summary>
        protected void DeleteSendMessage()
        {
            string selectID = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("SelectID"));
            SendMessageBLL.DeleteSendMessage(selectID, base.UserID);
            ResponseHelper.Write("ok");
            ResponseHelper.End();
        }
        /// <summary>
        /// 搜索好友
        /// </summary>
        protected void SearchFriend()
        {
            UserFriendSearchInfo userFriendSearch = new UserFriendSearchInfo();
            userFriendSearch.FriendName =StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("FriendName"));
            userFriendSearch.UserID = base.UserID;
            List<UserFriendInfo> userFriendList = UserFriendBLL.SearchUserFriendList(userFriendSearch);
            string result = string.Empty;
            foreach (UserFriendInfo userFriend in userFriendList)
            {
                result += userFriend.FriendID + "|" + userFriend.FriendName + "||";
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 2);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 返送短信
        /// </summary>
        protected void SendUserMessage()
        {
            string result = string.Empty;
            string userIDList =  StringHelper.AddSafe(RequestHelper.GetQueryString<string>("UserIDList"));
            string userNameList = StringHelper.AddSafe( RequestHelper.GetQueryString<string>("UserNameList"));
            string title = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Title"));
            string content =  StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Content"));
            if (userNameList == string.Empty || title == string.Empty || content == string.Empty)
            {
                result = "请填写完整的信息";
            }
            else
            {
                //添加发件箱
                SendMessageInfo sendMessage = new SendMessageInfo();
                sendMessage.Title = title;
                sendMessage.Content = content;
                sendMessage.Date = RequestHelper.DateNow;
                sendMessage.ToUserID = userIDList;
                sendMessage.ToUserName = userNameList;
                sendMessage.UserID = base.UserID;
                sendMessage.UserName = base.UserName;
                sendMessage.IsAdmin = (int)BoolType.False;
                SendMessageBLL.AddSendMessage(sendMessage);
                //添加收件箱
                string[] userIDArray = userIDList.Split(',');
                string[] userNameArray = userNameList.Split(',');
                for (int i = 0; i < userIDArray.Length; i++)
                {
                    ReceiveMessageInfo receiveMessage = new ReceiveMessageInfo();
                    receiveMessage.Title = title;
                    receiveMessage.Content = content;
                    receiveMessage.Date = RequestHelper.DateNow;
                    receiveMessage.IsRead = (int)BoolType.False;
                    receiveMessage.IsAdmin = (int)BoolType.False;
                    receiveMessage.FromUserID = base.UserID;
                    receiveMessage.FromUserName = base.UserName;
                    receiveMessage.UserID = Convert.ToInt32(userIDArray[i]);
                    receiveMessage.UserName = userNameArray[i];
                    ReceiveMessageBLL.AddReceiveMessage(receiveMessage);
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}