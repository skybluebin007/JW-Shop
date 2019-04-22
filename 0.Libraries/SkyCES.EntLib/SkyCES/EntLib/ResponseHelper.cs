namespace SkyCES.EntLib
{
    using System;
    using System.Web;

    public sealed class ResponseHelper
    {
        public static void End()
        {
            HttpContext.Current.Response.End();
        }

        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }

        public static void Write(string content)
        {
            HttpContext.Current.Response.Write(content);
        }
    }
}

