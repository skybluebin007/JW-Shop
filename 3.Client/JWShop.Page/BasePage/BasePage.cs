using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Diagnostics;
using System.Web.SessionState;
using SkyCES.EntLib;
using JWShop.Common;
using JWShop.Business;

namespace JWShop.Page
{
    public class BasePage : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
        private HttpRequest request;
        public HttpRequest Request
        {
            get
            {
                return this.request;
            }
        }
        private HttpResponse response;
        public HttpResponse Response
        {
            get
            {
                return this.response;
            }
        }
        private HttpServerUtility server;
        public HttpServerUtility Server
        {
            get
            {
                return this.server;
            }
        }
        private HttpSessionState session;
        public HttpSessionState Session
        {
            get
            {
                return this.session;
            }
        }
        private HttpContext context;
        public HttpContext Context
        {
            get
            {
                return this.context;
            }
        }
        /// <summary>
        /// 页面执行时间
        /// </summary>
        protected double processTime = 0;
        public void ProcessRequest(HttpContext context)
        {
            PageInit(context);
            string action = RequestHelper.GetForm<string>("Action");
            if (action == "PostBack")
            {
                PostBack();
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();         
                PageLoad();
                sw.Stop();
                processTime = sw.Elapsed.TotalSeconds;

                ShowPage(); 
            }          
        }
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="context"></param>
        private void PageInit(HttpContext context)
        {
            this.request = context.Request;
            this.server = context.Server;
            this.response = context.Response;
            this.session = context.Session;
            this.context = context;
            if (needUserCookies)
            {
                ReadUserCookies();
            }
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        protected virtual void PageLoad(){ }
        /// <summary>
        /// 显示页面
        /// </summary>
        protected virtual void ShowPage() { }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected virtual void PostBack() { }

        private bool needUserCookies = true;
        /// <summary>
        /// 是否需要检查用户cookies信息，获取用户信息
        /// </summary>
        public bool NeedUserCookies
        {
            set 
            {
                this.needUserCookies = value;
            }
        }
        protected int UserId = 0;
        protected string UserName = string.Empty;
        protected string isMobile = "";
        protected int GradeID = 0;
        /// <summary>
        /// 检查cookies
        /// </summary>
        /// <returns></returns>
        private void ReadUserCookies()
        {
            string cookiesName = ShopConfig.ReadConfigInfo().UserCookies;
            string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
            if (cookiesValue != string.Empty)
            {
                try
                {
                    string[] strArray = cookiesValue.Split(new char[] { '|' });
                    string sign = strArray[0];
                    string userID = strArray[1];
                    string userName = strArray[2];
                    string gradeID = strArray[3];
                    //if (FormsAuthentication.HashPasswordForStoringInConfigFile(userID + HttpContext.Current.Server.UrlEncode(userName) + gradeID.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == sign.ToLower())
                    if (FormsAuthentication.HashPasswordForStoringInConfigFile(userID + userName + gradeID.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == sign.ToLower())
                    {
                        UserId = Convert.ToInt32(userID);
                        UserName =HttpContext.Current.Server.UrlDecode(userName);
                        GradeID = Convert.ToInt32(gradeID);
                    }
                    else
                    {
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                }
                catch
                {
                    CookiesHelper.DeleteCookie(cookiesName);
                }
            }
            if (GradeID == 0)
            {
                GradeID = UserGradeBLL.ReadByMoney(0).Id;
            }
        }
        ///<summary>
        /// 验证用户是否登陆
        ///</summary>
        protected void CheckUserLogin(int t=0)
        {
            if (t == 0)
            {
                if (UserId == 0)
                {
                    ResponseHelper.Write("<script language='javascript'>window.location.href='/user/login.html?RedirectUrl=" + Request.RawUrl + "';</script>");
                    ResponseHelper.End();
                }
            }
            else
            {
                if (UserId == 0)
                {
                    ResponseHelper.Write("<script language='javascript'>window.location.href='/Mobile/login.html?RedirectUrl=" + Request.RawUrl + "';</script>");
                    ResponseHelper.End();
                }
            }
        }
    }
}
