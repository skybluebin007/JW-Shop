namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;
    using System.IO;

    public sealed class URLRewriterModule : IHttpModule
    {
        private string cacheKey = "Sky_URLRewriterModule";
        private static string configFileName = "/Config/URLRewriter.config";
        private static string fileType = "|.html|.aspx|";
        private static string forbidFolder = "|/admin/|/admin|";
        private static string path = string.Empty;
        private static Dictionary<string, string> replaceFileTypeDic = null;
        private List<URLInfo> urlList = new List<URLInfo>();

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
           context.BeginRequest += new EventHandler(this.URLRewriter_BeginRequest);
        }

        private bool IsRewriter(string url)
        {
            if (url.IndexOf('?') > -1)
            {
                url = url.Split(new char[] { '?' })[0];
            }
            //string str = FileHelper.GetFolderName(url).ToLower();
            string str = url.ToLower();
            string str2 = FileHelper.GetFileExtension(url).ToLower();
            bool flag = false;
            bool flag2 = true;
            string str3 = forbidFolder.ToLower();
            if (str3.Length > 1)
            {
                str3 = str3.Substring(1, str3.Length - 2);
            }
            foreach (string str4 in str3.Split(new char[] { '|' }))
            {
                if (str.IndexOf(str4) == 0)
                {
                    flag2 = false;
                    break;
                }
            }
            if ((str2==string.Empty ||(fileType.ToLower().IndexOf("|" + str2 + "|") > -1)) && flag2)
            {
                flag = true;
            }
            return flag;
        }

        private void ReadURLList()
        {
            if (CacheHelper.Read(this.cacheKey) == null)
            {
                this.RefreshURLCache();
            }
            this.urlList = (List<URLInfo>) CacheHelper.Read(this.cacheKey);
        }

        public void RefreshURLCache()
        {
            string xmlFile = ServerHelper.MapPath(configFileName);
            List<URLInfo> cacheValue = new List<URLInfo>();
            using (XmlHelper helper = new XmlHelper(xmlFile))
            {
                XmlNodeList childNodes = helper.ReadNode("URLRewriters").ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    URLInfo item = new URLInfo {
                        VitualPath = node.Attributes["VitualPath"].Value,
                        RealPath = node.Attributes["RealPath"].Value,
                        IsEffect = Convert.ToBoolean(node.Attributes["IsEffect"].Value)
                    };
                    cacheValue.Add(item);
                }
            }
            CacheDependency cd = new CacheDependency(xmlFile);
            CacheHelper.Write(this.cacheKey, cacheValue, cd);
        }

        private void URLRewriter_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication) sender).Context;
            string originalUrl = context.Request.RawUrl;
            string rawUrl = context.Request.RawUrl;
   
            if (this.IsRewriter(rawUrl))
            {
                this.ReadURLList();
                bool flag = false;
                foreach (URLInfo info in this.urlList)
                {
                    string vitualPath = info.VitualPath;
                    string realPath = info.RealPath;
                    if (info.IsEffect)
                    {
                        if (vitualPath.StartsWith("~"))
                        {
                            vitualPath = HttpContext.Current.Request.ApplicationPath + vitualPath.Substring(2);
                        }
                        if (realPath.StartsWith("~"))
                        {
                            realPath = HttpContext.Current.Request.ApplicationPath + realPath.Substring(2);
                        }

                        //有QueryString的URL，去掉后再匹配
                        if (rawUrl.IndexOf('?') > -1)
                        {
                            rawUrl = rawUrl.Split(new char[] { '?' })[0];
                        }
                        Match match = new Regex("^" + vitualPath + "$", RegexOptions.IgnoreCase).Match(rawUrl);

                        if (match.Success)
                        {
                            for (int i = match.Groups.Count; i >= 1; i--)
                            {
                                realPath = realPath.Replace("$" + i.ToString(), match.Groups[i].Value);
                            }
                            rawUrl = path + realPath;
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    rawUrl = path + rawUrl;
                }
                if (replaceFileTypeDic != null)
                {
                    foreach (KeyValuePair<string, string> pair in replaceFileTypeDic)
                    {
                        if (rawUrl.IndexOf("?") > -1)
                        {
                            rawUrl = StringHelper.ReplaceEx(rawUrl.Substring(0, rawUrl.IndexOf("?")), pair.Key, pair.Value) + rawUrl.Substring(rawUrl.IndexOf("?"));
                        }
                        else
                        {
                            rawUrl = StringHelper.ReplaceEx(rawUrl, pair.Key, pair.Value);
                        }
                    }
                }
                try
                {
                    if (!flag)
                    {
                        Dictionary<string, string> fileNameDic = new Dictionary<string, string>();
                        List<FileInfo> fileList = FileHelper.ListDirectory(ServerHelper.MapPath("/Plugins/Template/" + path.Replace("/Ashx/", "") + "/"), "|.htm|");
                        foreach (FileInfo file in fileList)
                        {
                            string tempFileName = file.FullName.Replace(ServerHelper.MapPath("\\Plugins\\Template\\" + path.Replace("/Ashx/", "") + "\\"), string.Empty).Replace("\\", "/");
                            fileNameDic.Add(tempFileName.ToLower(), tempFileName);
                        }
                        //如果没有对应文件，跳转到原始路径，不进行地址重写
                        if (rawUrl.IndexOf("?") > -1)
                        {
                            rawUrl = rawUrl.Substring(0, rawUrl.IndexOf("?"));
                        }
                        string afterUrl = rawUrl.Replace(path, "").Replace(".ashx", ".htm").ToLower();
                        afterUrl = afterUrl.Substring(1, afterUrl.Length - 1);//去除"/"
                        if (fileNameDic.ContainsKey(afterUrl))
                        {
                            context.RewritePath(rawUrl);
                        }
                        else
                        {
                            context.RewritePath(originalUrl);
                        }
                    }
                    else
                    {
                        context.RewritePath(rawUrl);
                    }

                }
                catch
                {
                    context.Response.Write("未找到该页面");
                }
            }
        }

        public static string ConfigFileName
        {
            get
            {
                return configFileName;
            }
            set
            {
                configFileName = value;
            }
        }

        public static string FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                fileType = value;
            }
        }

        public static string ForbidFolder
        {
            get
            {
                return forbidFolder;
            }
            set
            {
                forbidFolder = value;
            }
        }

        public static string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public static Dictionary<string, string> ReplaceFileTypeDic
        {
            get
            {
                return replaceFileTypeDic;
            }
            set
            {
                replaceFileTypeDic = value;
            }
        }
    }
}

