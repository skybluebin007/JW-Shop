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
using System.Linq;


namespace JWShop.Web.Admin
{
    public partial class SendMessageAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("AddMessage", PowerCheckType.Single);
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("AddMessage", PowerCheckType.Single);
            string sendUser = RequestHelper.GetForm<string>("RelationUser");
            if (sendUser != string.Empty)
            {

                foreach (string user in sendUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int userID = 0;
                    int.TryParse(user.Split('|')[0], out userID);
                    string userName = user.Split('|')[1];
                    ReceiveMessageInfo sendMessage = new ReceiveMessageInfo();
                    sendMessage.Title = Title.Text;
                    sendMessage.Content = Content.Text;
                    sendMessage.Date = RequestHelper.DateNow;
                    sendMessage.UserID = userID;
                    sendMessage.UserName = userName;
                    sendMessage.IsAdmin = (int)BoolType.True;
                    int msgId = ReceiveMessageBLL.Add(sendMessage);
                    AdminLogBLL.Add(string.Format("给用户(ID:{0})发送消息(ID:{1})", userID, msgId));
                }

                ScriptHelper.Alert("发送成功", "SendMessage.aspx");
            }
            else
            {
                ScriptHelper.Alert("请选择接收用户");
               
            }
          
        }
    }
}