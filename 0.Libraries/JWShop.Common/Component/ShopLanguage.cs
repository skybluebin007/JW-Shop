using System;
using System.Xml;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// 语言包
    /// </summary>
    public sealed class ShopLanguage
    {
        private static string languageCacheKey = CacheKey.ReadCacheKey("Language");
        /// <summary>
        /// 从缓存读取语言包
        /// </summary>
        /// <returns></returns>
        public static string ReadLanguage(string key)
        {
            if (CacheHelper.Read(languageCacheKey) == null)
            {
                RefreshLanguageCache();
            }
            return ((Dictionary<string, string>)CacheHelper.Read(languageCacheKey))[key];
        }
        /// <summary>
        /// 刷新全局语言包
        /// </summary>
        public static void RefreshLanguageCache()
        {
            string fileName = ServerHelper.MapPath("~/Config/ShopLanguage.config");
            Dictionary<string, string> language = new Dictionary<string, string>();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                foreach (XmlNode xn in xh.ReadNode("Language").ChildNodes)
                {
                    language.Add(xn.Attributes["key"].Value, xn.InnerText);
                }
            }
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(languageCacheKey, language, cd);
        }
    }
}
