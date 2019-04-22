using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Login.Weixin
{
    public partial class Login : System.Web.UI.Page
    {     

        protected void Page_Load(object sender, EventArgs e)
        {

            LoginConfig loginConfig = new LoginConfig();
            string app_id = loginConfig.AppKey;
            string app_secret = loginConfig.AppSecret;

            //成功授权后的回调地址
            string redirect_url = Server.UrlEncode("http://" + Request.Url.Host + "/Plugins/Login/Weixin/WeixinLogin.aspx");
            string _state = new Random().Next(0, 9999).ToString();
            //微信授权登录 scope:snsapi_userinfo
            //微信网页应用 scope:snsapi_login
            string weixin_url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + app_id + "&redirect_uri=" + redirect_url + "&response_type=code&scope=snsapi_login&state=" + _state + "#wechat_redirect";


            Response.Write("<script> location.href='" + weixin_url + "'</script>");
                Response.End();       
        
        }

    }
}
