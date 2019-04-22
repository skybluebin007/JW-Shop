namespace SkyCES.EntLib
{
    using System;
    using System.Web;

    public sealed class ScriptHelper
    {
        public static void Alert(string message)
        {
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute =DateTime.Now.AddMinutes(-1);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            ResponseHelper.Write("<script language='javascript'>top.window.layer.alert('" + message.Replace("\r\n", "") + "', {skin: 'layui-layer-jw',closeBtn: 0});history.go(-1);;</script>");
            ResponseHelper.End();
        }
      
        public static void Alert(string message, string url)
        {
            ResponseHelper.Write("<script language='javascript'>top.window.layer.alert('" + message + "', {skin: 'layui-layer-jw',closeBtn: 0});window.location.href='" + url + "';</script>");
            ResponseHelper.End();
        }
        public static void AlertFront(string message)
        {
            ResponseHelper.Write("<script language='javascript'>alert('" + message + "');history.back(-1);</script>");
            ResponseHelper.End();
        }
        public static void AlertFrontApp(string message)
        {
            ResponseHelper.Write("<script>app.jMsg(" + message + ");history.back(-1);</script>");
            ResponseHelper.End();
        }
        public static void AlertFrontApp(string message,string url)
        {
            ResponseHelper.Write("<script>app.jMsg(" + message + ");window.location.href='" + url + "';</script>");
            ResponseHelper.End();
        }
        public static void AlertFront(string message, string url)
        {
            ResponseHelper.Write("<script language='javascript'>alert('" + message + "');window.location.href='" + url + "';</script>");
            ResponseHelper.End();
        }
        public static void AlertWithScript(string message, string script)
        {
            ResponseHelper.Write("<script language='javascript'>alert('" + message + "');" + script + "</script>");
            ResponseHelper.End();
        }
        
    }
}