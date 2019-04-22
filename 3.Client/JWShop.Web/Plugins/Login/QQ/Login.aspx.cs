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

namespace JWShop.Login.QQ
{
    public partial class Login : System.Web.UI.Page
    {
        //private string qq_oauth_token_secret_cookiesName = "qq_oauth_token_secret";


        private static Random RndSeed = new Random();

        public string GenerateRndNonce()
        {
            return (RndSeed.Next(1, 0x5f5e0ff).ToString("00000000") + RndSeed.Next(1, 0x5f5e0ff).ToString("00000000") +
                    RndSeed.Next(1, 0x5f5e0ff).ToString("00000000") + RndSeed.Next(1, 0x5f5e0ff).ToString("00000000"));
        }

        public string file_get_contents(string url, Encoding encode)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    int readc;
                    byte[] buffer = new byte[1024];
                    while ((readc = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, readc);
                    }
                }
                return encode.GetString(ms.ToArray());
            }
        }

        NameValueCollection ParseJson(string json_code)
        {
            NameValueCollection mc = new NameValueCollection();
            Regex regex = new Regex(@"(\s*\""?([^""]*)\""?\s*\:\s*\""?([^""]*)\""?\,?)");
            json_code = json_code.Trim();
            if (json_code.StartsWith("{"))
            {
                json_code = json_code.Substring(1, json_code.Length - 2);
            }
            foreach (Match m in regex.Matches(json_code))
            {
                mc.Add(m.Groups[2].Value, m.Groups[3].Value);
                //Response.Write(m.Groups[2].Value + "=" + m.Groups[3].Value + "<br/>");
            }
            return mc;
        }
        NameValueCollection ParseUrlParameters(string str_params)
        {
            NameValueCollection nc = new NameValueCollection();
            foreach (string p in str_params.Split('&'))
            {
                string[] p_s = p.Split('=');
                nc.Add(p_s[0], p_s[1]);
            }
            return nc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            LoginConfig loginConfig = new LoginConfig();
            string app_id = loginConfig.AppKey;
            string app_secret = loginConfig.AppSecret;

            //成功授权后的回调地址
            string my_url = "http://" + Request.Url.Host + RequestHelper.RawUrl;

            //Step1：获取Authorization Code
            //session_start();
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回
                string state = GenerateRndNonce();//md5(uniqid(rand(), TRUE)); 
                string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(state + ShopConfig.ReadConfigInfo().SecureKey, "MD5");
                //拼接URL     
                string dialog_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + state + "|" + sign;
                //Response.Write(dialog_url );
                Response.Write("<script> location.href='" + dialog_url + "'</script>");
                Response.End();
            }

            //Step2：通过Authorization Code获取Access Token
            string[] stateCode = Request["state"].ToString().Split('|');

            if (stateCode.Length == 2 && stateCode[1].Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(stateCode[0] + ShopConfig.ReadConfigInfo().SecureKey, "MD5")))
            {
                //拼接URL   
                string token_url = "https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&"
                + "client_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&client_secret=" + app_secret + "&code=" + code;
                string response = file_get_contents(token_url, Encoding.UTF8);
                NameValueCollection msg;
                if (response.IndexOf("callback") != -1)
                {
                    int lpos = response.IndexOf("(");
                    int rpos = response.IndexOf(")");
                    response = response.Substring(lpos + 1, rpos - lpos - 1);
                    msg = ParseJson(response);

                    if (!string.IsNullOrEmpty(msg["error"]))
                    {
                        Response.Write("<h3>error:</h3>" + msg["error"].ToString());
                        Response.Write("<h3>msg  :</h3>" + msg["error_description"]);
                        Response.End();
                        return;
                    }
                }
                //Response.Write(response);
                //Step3：使用Access Token来获取用户的OpenID
                NameValueCollection ps = ParseUrlParameters(response);
                //*parse_str($response, $params);
                string graph_url = "https://graph.qq.com/oauth2.0/me?access_token=" + ps["access_token"];
                string str = file_get_contents(graph_url, Encoding.Default);
                if (str.IndexOf("callback") != -1)
                {
                    int lpos = str.IndexOf("(");
                    int rpos = str.IndexOf(")");
                    str = str.Substring(lpos + 1, rpos - lpos - 1);
                }
                NameValueCollection user = ParseJson(str);
                if (!string.IsNullOrEmpty(user["error"]))
                {
                    Response.Write("<h3>error:</h3>" + user["error"]);
                    Response.Write("<h3>msg  :</h3>" + user["error_description"]);
                    Response.End();
                }           
            
                string openID = user["openid"];
                if (!string.IsNullOrEmpty(openID))
                {
                    #region Step4：使用Access Token, APPID, OpenID来获取基本信息(get_user_info)
                    string getuserinfo_url = "https://graph.qq.com/user/get_user_info?access_token=" + ps["access_token"] + "&oauth_consumer_key=" + app_id + "&openid=" + user["openid"];
                     str = file_get_contents(getuserinfo_url, Encoding.UTF8);
                    if (str.IndexOf("callback") != -1)
                    {
                        int lpos = str.IndexOf("(");
                        int rpos = str.IndexOf(")");
                        str = str.Substring(lpos + 1, rpos - lpos - 1);
                    }
                    NameValueCollection user_info = ParseJson(str);
                    //Response.Write(user_info["ret"]);
                    //Response.End();
                   
                    if (!string.IsNullOrEmpty(user_info["ret"]) && Convert.ToInt32(user_info["ret"].Replace(",",""))<0)
                    {
                        Response.Write("<h3>error:</h3>" + user_info["ret"]);
                        Response.Write("<h3>msg  :</h3>" + user_info["msg"]);
                        Response.End();
                    }
                    #endregion
                    openID = "qq-" + openID;
                    //如果没有用户添加用户
                    int userID = UserBLL.Read(openID).Id;
                    UserInfo userInfo = new UserInfo();
                    if (userID <= 0)
                    {
                        userInfo.UserName = !string.IsNullOrEmpty(user_info["nickname"]) ?  Server.UrlDecode(user_info["nickname"]).Replace(",", "") : openID;
                        userInfo.UserPassword = StringHelper.Password(Guid.NewGuid().ToString(), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                        userInfo.Email = "";
                        userInfo.Photo = user_info["figureurl_qq_1"].Replace(",", "").Replace("\\/","/");
                        //性别
                        string gender = user_info["gender"].Replace(",", "");
                        if (gender == "男") { userInfo.Sex = 1; }
                        else if (gender == "女") { userInfo.Sex = 2; }
                        else { userInfo.Sex = 3; }
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
                    //页面跳转
                    string url = RequestHelper.GetQueryString<string>("preurl");
                    if (url == string.Empty)
                    {
                        if (RequestHelper.RawUrl.ToLower().IndexOf("/mobile/") >= 0 || Request.Url.Host.ToLower().IndexOf("m") == 0 || RequestHelper.UserAgent())
                        {
                            url = "/mobile/user/index.html";
                        }
                        else
                        {
                            url = "/user/index.html";
                        }
                    }
                    ResponseHelper.Redirect(url);


                }
                else
                {

                    Response.Write("不存在该用户");
                }
            }
            else
            {
                Response.Write("验证失败");
            }

            Response.End();
        }

        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <returns></returns>
        private string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <returns></returns>
        private string GenerateNonce()
        {
            Random random = new Random();
            return random.Next(123400, 9999999).ToString();
        }
        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        private string UrlEncode(string value)
        {
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            StringBuilder result = new StringBuilder();
            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }
            return result.ToString();
        }
    }
}
