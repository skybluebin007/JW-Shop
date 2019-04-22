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
    public class ProductReplyAjax : AjaxBasePage
    {
        private string footaddress = string.Empty;
        /// <summary>
        /// 标题
        /// </summary>
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
        /// <summary>
        /// 回复列表
        /// </summary>
        protected List<ProductReplyInfo> productReplyList = new List<ProductReplyInfo>();
        /// <summary>
        /// 用户列表
        /// </summary>
        protected List<UserInfo> userList = new List<UserInfo>();
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
            switch (action)
            {
                case "Add":
                    PostProductReply();
                    break;
                default:
                    break;
            }
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 8;
            int count = 0;
            int commentID = RequestHelper.GetQueryString<int>("CommentID");
            productReplyList = ProductReplyBLL.ReadProductReplyList(commentID, currentPage, pageSize, ref count, int.MinValue);

            ajaxPagerClass.CurrentPage = currentPage;
            ajaxPagerClass.PageSize = pageSize;
            ajaxPagerClass.Count = count;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.ListType = false;

            string strUserID = string.Empty;
            foreach (ProductReplyInfo productReply in productReplyList)
            {
                if (strUserID == string.Empty)
                {
                    strUserID = productReply.UserID.ToString();
                }
                else
                {
                    strUserID += "," + productReply.UserID.ToString();
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
        /// 提交回复
        /// </summary>
        public void PostProductReply()
        {
            string result = "ok";
            if (ShopConfig.ReadConfigInfo().AllowAnonymousReply == (int)BoolType.False && base.UserID == 0)
            {
                result = "还未登录";
            }
            else
            {
                AddProductReply(ref result);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="result"></param>
        public void AddProductReply(ref string result)
        {
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            int commentID = RequestHelper.GetQueryString<int>("CommentID");
            string replytCookies = CookiesHelper.ReadCookieValue("ReplytCookies" + commentID.ToString());
            if (ShopConfig.ReadConfigInfo().ReplyRestrictTime > 0 && replytCookies != string.Empty)
            {
                result = "请不要频繁提交";
            }
            else
            {
                ProductReplyInfo productReply = new ProductReplyInfo();
                productReply.ProductID = productID;
                productReply.CommentID = commentID;
                productReply.Content = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Content"));
                productReply.UserIP = ClientHelper.IP;
                productReply.PostDate = RequestHelper.DateNow;
                productReply.UserID = base.UserID;
                productReply.UserName = base.UserName;
                ProductReplyBLL.AddProductReply(productReply);
                if (ShopConfig.ReadConfigInfo().ReplyRestrictTime > 0)
                {
                    CookiesHelper.AddCookie("ReplytCookies" + commentID.ToString(), "ReplytCookies" + commentID.ToString(), ShopConfig.ReadConfigInfo().ReplyRestrictTime, TimeType.Second);
                }
            }
        }
    }
}