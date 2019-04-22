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
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace JWShop.Login.Weixin
{
    public partial class WeixinLogin : System.Web.UI.Page
    {
        protected string code { get; set; }
        protected string Appid { get; set; }
        protected string Appsecret { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginConfig loginConfig = new LoginConfig();
            this.Appid = loginConfig.AppKey;
            this.Appsecret = loginConfig.AppSecret;
            this.code = RequestHelper.GetQueryString<string>("code");
            if (!string.IsNullOrEmpty(code))
            {
                #region 获取access_token
                Access_tokenResult access_tokenResult = new Access_tokenResult();
                //如果session有值
                if (Session["expires_in"] != null && Session["access_token"] != null && Session["openid"] != null)
                {//如果Session["access_token"]没过期
                    if (Convert.ToInt32(Session["expires_in"]) >= ShopCommon.ConvertDateTimeInt(DateTime.Now))
                    {
                        access_tokenResult.access_token = Session["access_token"].ToString();
                        access_tokenResult.openid = Session["openid"].ToString();
                    }
                    else//如果Session["access_token"]过期,根据refresh_token刷新access_token
                    {
                        if (Session["refresh_token"] != null) access_tokenResult = RefreshAccess_token(Session["refresh_token"].ToString());
                    }
                }
                else
                {
                    //根据code获取access_token
                    access_tokenResult = CodeGetOpenidAndAccess_token(this.code);
                    if (!string.IsNullOrEmpty(access_tokenResult.errcode) || string.IsNullOrEmpty(access_tokenResult.access_token) || string.IsNullOrEmpty(access_tokenResult.openid))
                    {
                        Response.Write("参数错误，请稍后重试");
                        Response.End();
                    }
                    else
                    {
                        //验证access_token是否有效,失效了重新获取
                        WeChatMsg _msg = Check_Access_token(access_tokenResult.access_token, access_tokenResult.openid);
                        if (_msg.errcode != "0")
                        {
                            Response.Write("参数错误，请稍后重试");
                            Response.End();
                        }
                        else
                        {//access_token有效，重新赋值session
                            Session["expires_in"] = ShopCommon.ConvertDateTimeInt(DateTime.Now) + 7000;
                            Session["access_token"] = access_tokenResult.access_token;
                            Session["refresh_token"] = access_tokenResult.refresh_token;
                            Session["openid"] = access_tokenResult.openid;
                        }
                    }
                }
                #endregion

                //获取userinfo
                Snsapi_userinfo snsapi_userinfo = GetUserinfo(access_tokenResult.access_token, access_tokenResult.openid);
                if (!string.IsNullOrEmpty(snsapi_userinfo.errcode) || string.IsNullOrEmpty(snsapi_userinfo.openid))
                {
                    Response.Write("参数错误，请稍后重试");
                    Response.End();
                }
                else
                {
                    string openID = snsapi_userinfo.openid;
                    //string openID = access_tokenResult.openid;                          
                    openID = "wx-" + openID;
                    //如果没有用户添加用户
                    int userID = UserBLL.Read(openID).Id;
                    UserInfo userInfo = new UserInfo();
                    if (userID <= 0)
                    {
                        userInfo.UserName = Server.UrlDecode(snsapi_userinfo.nickname);
                        userInfo.UserPassword = StringHelper.Password(Guid.NewGuid().ToString(), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                        userInfo.Photo = snsapi_userinfo.headimgurl;
                        int _sex = userInfo.Sex;
                        int.TryParse(snsapi_userinfo.sex, out _sex);
                        //微信sex：1 男，2 女， 0 未知
                        //本站sex：1 男，2 女， 3 未知
                        userInfo.Sex = _sex == 0 ? 3 : _sex;
                        userInfo.Email = "";
                        userInfo.RegisterIP = ClientHelper.IP;
                        userInfo.RegisterDate = RequestHelper.DateNow;
                        userInfo.LastLoginIP = ClientHelper.IP;
                        userInfo.LastLoginDate = RequestHelper.DateNow;
                        userInfo.FindDate = RequestHelper.DateNow;
                        userInfo.Status = (int)UserStatus.Normal;
                        userInfo.OpenId = openID;
                        userID = UserBLL.Add(userInfo);
                    }
                    //当前用户登录
                    userInfo = UserBLL.Read(userID);
                    UserBLL.UserLoginInit(userInfo);
                    userInfo.LastLoginIP = ClientHelper.IP;
                    userInfo.LastLoginDate = RequestHelper.DateNow;
                    UserBLL.Update(userInfo);
                    //跳转至会员中心
                    Response.Write("<script> location.href='/User/Index.html'</script>");
                    Response.End(); 
                    //ResponseHelper.Redirect("/User/Index.html");
                }
            }
        
        }
        /// 用Code换取Openid、Access_token
        public Access_tokenResult CodeGetOpenidAndAccess_token(string Code)
        {                                       
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Appid, Appsecret, Code);
            string ReText = WebRequestPostOrGet(url, "");//post/get方法获取信息 
            Access_tokenResult access_tokenResult = JsonConvert.DeserializeObject<Access_tokenResult>(ReText);
            return access_tokenResult;
        }
        /// <summary>
        /// 刷新access_token(超时刷新)
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public Access_tokenResult RefreshAccess_token(string refresh_token)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", Appid, refresh_token);
            string ReText = WebRequestPostOrGet(url, "");//post/get方法获取信息 
            Access_tokenResult access_tokenResult = JsonConvert.DeserializeObject<Access_tokenResult>(ReText);
            return access_tokenResult;
        }
        /// <summary>
        /// 验证access_token是否有效
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        protected WeChatMsg Check_Access_token(string access_token, string openid)
        {                                      
            string url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", access_token, openid);
            string ReText = WebRequestPostOrGet(url, "");//post/get方法获取信息 
            WeChatMsg msg = JsonConvert.DeserializeObject<WeChatMsg>(ReText);
            return msg;
        }

        /// <summary>
        /// 服务器端发起HTTP请求
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_method">GET  POST</param>
        /// <returns></returns>
        protected string WebRequestPostOrGet(string _url, string _method)
        {
            string responseString = string.Empty;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(_url);

            myRequest.Method = string.IsNullOrEmpty(_method) ? "GET" : "POST";
            myRequest.ContentType = "text/html;charset=utf-8";

            using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                responseString = sr.ReadToEnd();
            }
            return responseString;
        }
        /// <summary>
        /// 获取userinfo
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        protected Snsapi_userinfo GetUserinfo(string _access_token, string _openid)
        {                                        
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", _access_token, _openid);
            string ReText = WebRequestPostOrGet(url, "");//post/get方法获取信息 
            Snsapi_userinfo snsapi_userinfo = JsonConvert.DeserializeObject<Snsapi_userinfo>(ReText);
            return snsapi_userinfo;
        }
        /// <summary>
        /// 微信access_token返回json
        /// </summary>
        public class Access_tokenResult
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
            public string errcode { get; set; }
        }
        /// <summary>
        /// 微信Snsapi_userinfo返回json
        /// </summary>
        public class Snsapi_userinfo
        {
            public string openid { get; set; }
            public string nickname { get; set; }
            public string sex { get; set; }
            public string language { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string country { get; set; }
            public string headimgurl { get; set; }
            public string[] privilege { get; set; }
            public string unionid { get; set; }
            public string errcode { get; set; }
        }
        /// <summary>
        /// 微信返回验证信息
        /// </summary>
        public class WeChatMsg
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
        }
    }
}
