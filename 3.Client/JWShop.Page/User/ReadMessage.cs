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
    public class ReadMessage : UserBasePage
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGradeName = string.Empty;
        protected ReceiveMessageInfo receiveMessage = new ReceiveMessageInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserID);
            userGradeName = UserGradeBLL.ReadUserGradeCache(base.GradeID).Name;
            int id = RequestHelper.GetQueryString<int>("ID");
            receiveMessage = ReceiveMessageBLL.ReadReceiveMessage(id, base.UserID);
            if (receiveMessage.ID > 0 && receiveMessage.IsRead == (int)BoolType.False)
            {
                receiveMessage.IsRead = (int)BoolType.True;
                ReceiveMessageBLL.UpdateReceiveMessage(receiveMessage);
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
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            int id = RequestHelper.GetForm<int>("ID");
            receiveMessage = ReceiveMessageBLL.ReadReceiveMessage(id, base.UserID);
            if (receiveMessage.ID > 0 && receiveMessage.IsAdmin == (int)BoolType.False)
            {
                // 发送信息
                SendMessageInfo sendMessage = new SendMessageInfo();
                sendMessage.Title = "回复：" + receiveMessage.Title;
                sendMessage.Content = StringHelper.AddSafe(RequestHelper.GetForm<string>("Content"));
                sendMessage.Date = RequestHelper.DateNow;
                sendMessage.ToUserID = receiveMessage.FromUserID.ToString();
                sendMessage.ToUserName = receiveMessage.FromUserName;
                sendMessage.UserID = base.UserID;
                sendMessage.UserName = base.UserName;
                sendMessage.IsAdmin = (int)BoolType.False;
                int sid = SendMessageBLL.AddSendMessage(sendMessage);
                //接受信息
                ReceiveMessageInfo tempReceiveMessage = new ReceiveMessageInfo();
                receiveMessage.ID = RequestHelper.GetQueryString<int>("ID");
                tempReceiveMessage.Title = sendMessage.Title;
                tempReceiveMessage.Content = sendMessage.Content;
                tempReceiveMessage.Date = sendMessage.Date;
                tempReceiveMessage.IsRead = (int)BoolType.False;
                tempReceiveMessage.IsAdmin = (int)BoolType.False;
                tempReceiveMessage.FromUserID = base.UserID;
                tempReceiveMessage.FromUserName = base.UserName;
                tempReceiveMessage.UserID = receiveMessage.FromUserID;
                tempReceiveMessage.UserName = receiveMessage.FromUserName;
                ReceiveMessageBLL.AddReceiveMessage(tempReceiveMessage);
                ScriptHelper.AlertFront("回复成功", "/User/MessageDetail.aspx?ID=" + sid);
            }
            else
            {
                ScriptHelper.AlertFront("出现错误", "/User/ReadMessage.aspx?ID=" + id);
            }
        }
    }
}
