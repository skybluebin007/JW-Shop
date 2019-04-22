using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using NetDimension.Weibo;

namespace JWShop.Login.Sina
{
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginConfig loginConfig = new LoginConfig();
            string callBackUrl = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Login/Sina/Login.aspx?preurl="+RequestHelper.GetQueryString<string>("preurl");
            OAuth oauth = new OAuth(loginConfig.AppKey, loginConfig.AppSecret, callBackUrl);
            if (RequestHelper.GetQueryString<string>("code") != string.Empty)
            {
                string error = string.Empty;
                try
                {
                    string code = RequestHelper.GetQueryString<string>("code");
                    AccessToken accessToken = oauth.GetAccessTokenByAuthorizationCode(code);
                    oauth = new OAuth(loginConfig.AppKey, loginConfig.AppSecret, oauth.AccessToken, "");	//用Token实例化OAuth无需再次进入验证流程

                    TokenResult result = oauth.VerifierAccessToken();	//测试保存的AccessToken的有效性
                    if (result == TokenResult.Success)
                    {
                        Client Sina = new Client(oauth);
                        string uid = Sina.API.Entity.Account.GetUID();
                        var entity_userInfo = Sina.API.Entity.Users.Show(uid);
                        if (entity_userInfo.ScreenName != null)
                        {
                            string openID = "sina-" + entity_userInfo.ScreenName;
                            //如果没有用户添加用户
                            int userID = UserBLL.Read(openID).Id;
                            UserInfo user = new UserInfo();
                            if (userID == 0)
                            {
                                user.UserName = openID;
                                user.UserPassword = StringHelper.Password(Guid.NewGuid().ToString(), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                                user.Email = "";
                                user.RegisterIP = ClientHelper.IP;
                                user.RegisterDate = RequestHelper.DateNow;
                                user.LastLoginIP = ClientHelper.IP;
                                user.LastLoginDate = RequestHelper.DateNow;
                                user.FindDate = RequestHelper.DateNow;
                                user.Status = (int)UserStatus.Normal;
                                user.OpenId = openID;
                                userID = UserBLL.Add(user);
                            }
                            //当前用户登录
                            user = UserBLL.Read(userID);
                            UserBLL.UserLoginInit(user);
                            user.LastLoginIP = ClientHelper.IP;
                            user.LastLoginDate = RequestHelper.DateNow;
                            UserBLL.Update(user);
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
                            error = "不存在该用户";
                        }
                    }
                    else
                    {
                        error = String.Format("AccessToken无效！因为：{0}", result);
                    }
                }
                catch (WeiboException ex)
                {
                    error = "出错啦！" + ex.Message;
                }
                if (error != string.Empty)
                {

                    ScriptHelper.AlertWithScript(error, "windows.close();");
                }
            }
            else
            {
                OAuth o = new OAuth(loginConfig.AppKey, loginConfig.AppSecret, callBackUrl);

                string authorizeUrl = o.GetAuthorizeURL();
                //获取授权令牌
                ResponseHelper.Redirect(authorizeUrl);
            }
        }
    }
}
