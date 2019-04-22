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
    /// 缓存键
    /// </summary>
    public sealed class CacheKey
    {
        private static string cacheKey = "SocoShop_CacheKey";
        /// <summary>
        /// 从缓存读取缓存键
        /// </summary>
        /// <returns></returns>
        public static string ReadCacheKey(string key)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                RefreshCacheKey();
            }
            Dictionary<string, string> cacheDic = (Dictionary<string, string>)CacheHelper.Read(cacheKey);
            if (cacheDic.ContainsKey(key))
            {
                return cacheDic[key];
            }
            else
            {
                return Guid.NewGuid().ToString();
            }
        }
        /// <summary>
        /// 刷新缓存键
        /// </summary>
        public static void RefreshCacheKey()
        {
            string fileName = ServerHelper.MapPath("~/Config/CacheKey.config");
            Dictionary<string, string> cacheDic = new Dictionary<string, string>();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                foreach (XmlNode xn in xh.ReadNode("CacheKeys").ChildNodes)
                {
                    cacheDic.Add(xn.Attributes["Key"].Value, xn.Attributes["Value"].Value);
                }
            }
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(cacheKey, cacheDic, cd);
        }
    }
}
