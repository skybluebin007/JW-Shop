using System;
using System.IO;
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
    public class MessageDetail : UserBasePage
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
        /// 发送短信
        /// </summary>
        protected SendMessageInfo sendMessage = new SendMessageInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserID);
            userGradeName = UserGradeBLL.ReadUserGradeCache(base.GradeID).Name;

            int id = RequestHelper.GetQueryString<int>("ID");
            sendMessage = SendMessageBLL.ReadSendMessage(id, base.UserID);
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
