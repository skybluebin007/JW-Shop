namespace SkyCES.EntLib
{
    using System;
    using System.IO;
    using System.Net;

    public sealed class HttpHelper
    {
        public static string WebRequestGet(string url)
        {
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            string str = WebResponseGet(webRequest);
            webRequest = null;
            return str;
        }

        public static string WebRequestPost(string url, string postData)
        {
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            StreamWriter writer = new StreamWriter(webRequest.GetRequestStream());
            try
            {
                writer.Write(postData);
            }
            catch
            {
                throw;
            }
            finally
            {
                writer.Close();
                writer = null;
            }
            string str = WebResponseGet(webRequest);
            webRequest = null;
            return str;
        }

        private static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader reader = null;
            string str = string.Empty;
            try
            {
                reader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                str = reader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                reader.Close();
                reader = null;
            }
            return str;
        }
    }
}

