using System;
using System.Xml;
using System.Web.Caching;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// URL地址操作
    /// </summary>
    public sealed class URLClass
    {
        private static string fileName = ServerHelper.MapPath("/Config/URLRewriter.config");
        private static string urlCacheKey = CacheKey.ReadCacheKey("URL");

        /// <summary>
        /// 增加一条URL地址数据
        /// </summary>
        /// <param name="url">URL地址模型变量</param>
        public static int AddURL(URLInfo url)
        {
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                string[] attrib = { "ID", "VitualPath", "RealPath", "IsEffect" };
                int maxID = Convert.ToInt32(xh.ReadNode("URLRewriters").LastChild.Attributes["ID"].Value);
                string[] attribContent = { (maxID + 1).ToString(), url.VitualPath, url.RealPath, url.IsEffect.ToString() };
                xh.InsertElement("URLRewriters", "URLRewriter", attrib, attribContent, string.Empty);
                xh.Save();
                return maxID + 1;
            }
        }



        /// <summary>
        /// 更新一条URL地址数据
        /// </summary>
        /// <param name="url">URL地址模型变量</param>
        public static void UpdateURL(URLInfo url)
        {
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                XmlNodeList xdList = xh.ReadNode("URLRewriters").ChildNodes;
                foreach (XmlNode xn in xdList)
                {
                    if (url.ID == Convert.ToInt32(xn.Attributes["ID"].Value))
                    {
                        xn.Attributes["VitualPath"].Value = url.VitualPath;
                        xn.Attributes["RealPath"].Value = url.RealPath;
                        xn.Attributes["IsEffect"].Value = url.IsEffect.ToString();
                    }
                }
                xh.Save();
            }
        }


        /// <summary>
        /// 删除多条URL地址数据
        /// </summary>
        /// <param name="strID">URL地址的主键值,以,号分隔</param>
        public static void DeleteURL(string strID)
        {
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                XmlNodeList xdList = xh.ReadNode("URLRewriters").ChildNodes;
                foreach (XmlNode xn in xdList)
                {
                    if (strID.IndexOf(xn.Attributes["ID"].Value) > -1)
                    {
                        xh.ReadNode("URLRewriters").RemoveChild(xn);
                    }
                }
                xh.Save();
            }
        }

        /// <summary>
        /// 读取一条URL地址数据
        /// </summary>
        /// <param name="id">URL地址的主键值</param>
        /// <returns>URL地址数据模型</returns>
        public static URLInfo ReadURL(int id)
        {
            URLInfo url = new URLInfo();
            foreach (URLInfo tempURL in ReadURLList())
            {
                if (tempURL.ID == id)
                {
                    url = tempURL;
                    break;
                }
            }
            return url;
        }


        /// <summary>
        /// 读取URL地址数据列表
        /// </summary>
        /// <returns>URL地址数据列表</returns>
        public static List<URLInfo> ReadURLList()
        {
            if (CacheHelper.Read(urlCacheKey) == null)
            {
                RefreshURLLCache();
            }
            List<URLInfo> urlList = (List<URLInfo>)CacheHelper.Read(urlCacheKey);
            return urlList;
        }
        /// <summary>
        /// 刷新URL地址缓存
        /// </summary>
        public static void RefreshURLLCache()
        {
            List<URLInfo> urlList = new List<URLInfo>();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                XmlNodeList xdList = xh.ReadNode("URLRewriters").ChildNodes;
                foreach (XmlNode xn in xdList)
                {
                    URLInfo url = new URLInfo();
                    url.ID = Convert.ToInt32(xn.Attributes["ID"].Value);
                    url.VitualPath = xn.Attributes["VitualPath"].Value;
                    url.RealPath = xn.Attributes["RealPath"].Value;
                    url.IsEffect = Convert.ToBoolean(xn.Attributes["IsEffect"].Value);
                    urlList.Add(url);
                }
            }
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(urlCacheKey, urlList, cd);
        }
    }
}
