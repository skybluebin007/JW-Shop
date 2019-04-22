using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// 虚拟文件
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ShopVirtualFile : VirtualFile
    {
        private string content;
        private Dictionary<string, string> fileNameDic = new Dictionary<string, string>();

        /// <summary>
        /// 虚拟文件是否存在
        /// </summary>
        public bool Exists
        {
            get { return (content != null); }
        }

        public ShopVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            GetRightFileName();
            GetData();
        }
        /// <summary>
        /// 获取文件内容
        /// </summary>
        protected void GetData()
        {
            try
            {
                string htmlFileName = this.VirtualPath.Replace("/Ashx/" + ShopConfig.ReadConfigInfo().TemplatePath + "/", string.Empty).Replace(".ashx", ".htm");
                SkyTemplateFile pt = new SkyTemplateFile(fileNameDic[htmlFileName.ToLower()], "/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/");
                pt.InheritsNameSpace = "JWShop.Page";
                pt.NameSpace = "JWShop.Web";

                //请求路径为手机站点时，重新设置数据来源命名空间
                string[] htmlFileNameSplit = htmlFileName.Split('/');
                if (htmlFileNameSplit.Length > 0)
                {
                    string siteName = htmlFileNameSplit[0].ToLower();
                    if (siteName == "mobile")
                    {
                        pt.InheritsNameSpace = "JWShop.Page.Mobile";
                    }
                    if (siteName == "mobileadmin")
                    {
                        pt.InheritsNameSpace = "JWShop.Page.Admin";
                    }
                }

                //解析<html:include>中包含表达式的文件

                content = pt.Process();
            }
            catch(Exception ex){
                ResponseHelper.Redirect("/404.html");
            }
        }
        /// <summary>
        /// 返回文件流
        /// </summary>
        /// <returns></returns>
        public override Stream Open()
        {
            //string message = "<div style=\\\"text-align:center; font-size:12px; margin-bottom:10px\\\"><a href=\\\"#\\\" target=\\\"_blank\\\" style=\\\"text-decoration:none; color:#4C5A62\\\">" + JWShop.Common.Global.ProductName + " " + JWShop.Common.Global.Version + "</a>&nbsp;&nbsp;<a href=\\\"http://www.yyjing.com\\\" target=\\\"_blank\\\" style=\\\"text-decoration:none; color:#4C5A62\\\">" + Global.CopyRight + "</a></div>";
            //content = content.Replace("</body>", message + "</body>");
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        /// <summary>
        /// 获取文件名的对应关系
        /// </summary>
        private void GetRightFileName()
        {
            string cacheKey = "ShopTemplateFile";
            if (CacheHelper.Read(cacheKey) == null)
            {
                List<FileInfo> fileList = FileHelper.ListDirectory(ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/"), "|.htm|");
                foreach (FileInfo file in fileList)
                {
                    string tempFileName = file.FullName.Replace(ServerHelper.MapPath("\\Plugins\\Template\\" + ShopConfig.ReadConfigInfo().TemplatePath + "\\"), string.Empty).Replace("\\", "/");
                    fileNameDic.Add(tempFileName.ToLower(), tempFileName);
                }
                string path = ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/");
                CacheDependency cd = new CacheDependency(path);
                CacheHelper.Write(cacheKey, fileNameDic, cd);
            }
            else
            {
                fileNameDic = (Dictionary<string, string>)CacheHelper.Read(cacheKey);
            }
        }
    }
}