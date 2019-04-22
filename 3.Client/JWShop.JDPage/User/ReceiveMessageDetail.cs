using System;
using System.Data;
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
namespace JWShop.Page
{
    public class ReceiveMessageDetail : UserBasePage
    {
        protected ReceiveMessageInfo theMsg = new ReceiveMessageInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("Id");
            if (id <= 0)
            {
                ScriptHelper.Alert("参数错误", "/User/ReceiveMessage.html");
            }
            else {
              var  tmpMsg = ReceiveMessageBLL.Read(id);
              tmpMsg.IsRead = (int)BoolType.True;
              ReceiveMessageBLL.Update(tmpMsg);
              theMsg = tmpMsg;
            }
        }
    }
}
