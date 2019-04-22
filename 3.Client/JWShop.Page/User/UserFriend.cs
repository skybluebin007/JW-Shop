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
    public class UserFriend : UserBasePage
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGradeName = string.Empty;
        /// <summary>
        /// 用户好友
        /// </summary>
        protected List<UserFriendInfo> userFriendList = new List<UserFriendInfo>();
        /// <summary>
        /// 用户列表
        /// </summary>
        protected List<UserInfo> userList = new List<UserInfo>();
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
            user = UserBLL.ReadUserMore(base.UserID);
            userGradeName = UserGradeBLL.ReadUserGradeCache(base.GradeID).Name;
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Delete":
                    string deleteID = RequestHelper.GetQueryString<string>("ID");
                    UserFriendBLL.DeleteUserFriend(deleteID, base.UserID);
                    ResponseHelper.Write("ok");
                    ResponseHelper.End();
                    break;
                default:
                    break;
            }

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 25;
            int count = 0;
            UserFriendSearchInfo userFriendSearch = new UserFriendSearchInfo();
            userFriendSearch.UserID = base.UserID;
            userFriendList = UserFriendBLL.SearchUserFriendList(currentPage, pageSize, userFriendSearch, ref count);
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

            string strUserID = string.Empty;
            foreach (UserFriendInfo userFriend in userFriendList)
            {
                if (strUserID == string.Empty)
                {
                    strUserID = userFriend.FriendID.ToString();
                }
                else
                {
                    strUserID += "," + userFriend.FriendID.ToString();
                }
            }
            if (strUserID != string.Empty)
            {
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InUserID = strUserID;
                userList = UserBLL.SearchUserList(userSearch);
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string footaddress = string.Empty;
        public string FootAddress
        {
            set { this.footaddress = value; }
            get
            {
                string temp = ShopConfig.ReadConfigInfo().FootAddress;
                if (this.footaddress != string.Empty)
                {
                    temp = this.footaddress + " - " + ShopConfig.ReadConfigInfo().FootAddress;
                }
                return temp;
            }
        }
    }
}
