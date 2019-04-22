using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Text.RegularExpressions;

namespace JWShop.Page.Mobile
{
   public class UpdateMobile:UserBasePage
    {
       protected int wait = 0;
       /// <summary>
       /// 页面加载
       /// </summary>
       protected override void PageLoad()
       {
           base.PageLoad();
       
           //短信验证码有效期
           string verify_send = CookiesHelper.ReadCookieValue("verify_send");
           DateTime tmNow = DateTime.Now;
           DateTime tm;
           try
           {
               tm = DateTime.Parse(verify_send);
           }
           catch { tm = tmNow.AddSeconds(-60); }
           wait = 60 - Convert.ToInt32((tmNow - tm).TotalSeconds);
           if (wait < 0) wait = 0;

       }
    }
}
