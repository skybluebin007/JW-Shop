using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Hosting;
using JWShop.Common;
using SkyCES.EntLib;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JWShop.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ShopConfig.RefreshApp();
            HostingEnvironment.RegisterVirtualPathProvider(new ShopPathProvider());

            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.MapHubs();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
#region 301重定向
            //HttpContext context = ((HttpApplication)sender).Context;
            //if (context.Request.Url.ToString().IndexOf("www") >= 0)
            //{
            //    Response.AddHeader("Location", "http://tiexincai.com/");
            //    Response.Status = "301 Moved Permanently";
            //}
#endregion

            //string q = "<div style='position:fixed;top:0px;width:100%;height:100%;background-color:white;color:red;font-weight:bold;border-bottom:5px solid #999;'><br>您的提交带有不合法参数！</div>";
            //if (Request.Cookies != null)
            //{
            //    if (UrlFilter.CookieData())
            //    {
            //        Response.Write(q);
            //        Response.End();
            //    }
            //}

            //if (Request.UrlReferrer != null)
            //{
            //    if (UrlFilter.referer())
            //    {
            //        Response.Write(q);
            //        Response.End();
                    //var context=HttpContext.Current;
                    //context.Response.Redirect(StringHelper.AddSafe(HttpUtility.HtmlEncode(Request.UrlReferrer)));

            //    }
            //}

            //if (Request.RequestType.ToUpper() == "POST")
            //{
            //    if (UrlFilter.PostData())
            //    {

            //        Response.Write(q);
            //        Response.End();
            //    }
            //}
            //if (Request.RequestType.ToUpper() == "GET")
            //{
            //    if (UrlFilter.GetData())
            //    {
            //        Response.Write(q);
            //        Response.End();
            //    }
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ////获取错误码 如404
            HttpContext context = ((HttpApplication)sender).Context;
            int statusCode = ((HttpException)context.Error).GetHttpCode();

            //获取Exception
            //Exception ex = this.Context.Server.GetLastError();
            //处理Exception
            //if (statusCode == 404)
            //{
            //    //清除当前的输出
            //this.Context.Response.Clear();

            //////如果非要跳转到目标页面（即地址栏也改变成Error.aspx的话）也不是不行，可以变通处理
            //this.Context.Response.Write("<script>top.location.href='/404.html';</script>");
            //Server.ClearError();//在Global.asax中调用Server.ClearError方法相当于是告诉Asp.Net系统抛出的异常已经被处理过了，不需要系统跳转到Asp.Net的错误黄页了。如果想在Global.asax外调用ClearError方法可以使用HttpContext.Current.ApplicationInstance.Server.ClearError()。             
            //this.Context.Response.End();
            //}
        
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}