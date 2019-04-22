using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Collections;
using Microsoft.AspNet.SignalR;

namespace JWShop.Pay.WxPay
{
    public partial class NotifyPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResultNotify resultNotify = new ResultNotify(this);
            resultNotify.ProcessNotify();

            //如果付款操作，发送signalr消息
            if (resultNotify.notifyResult)
            {
                IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<JWShop.PushHub>();
                chat.Clients.All.sendMessage(1);
            }
        }
    }
}