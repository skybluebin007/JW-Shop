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
    /// 上传表ID
    /// </summary>
    public sealed class UploadTable
    {
        private static string uploadTableCacheKey = CacheKey.ReadCacheKey("UploadTable");
        /// <summary>
        /// 从缓存读取上传表ID
        /// </summary>
        /// <returns></returns>
        public static int ReadTableID(string key)
        {
            if (CacheHelper.Read(uploadTableCacheKey) == null)
            {
                RefreshUploadTableCache();
            }
            return ((Dictionary<string, int>)CacheHelper.Read(uploadTableCacheKey))[key];
        }
        /// <summary>
        /// 刷新上传表ID
        /// </summary>
        public static void RefreshUploadTableCache()
        {
            string fileName = ServerHelper.MapPath("~/Config/UploadTable.config");
            Dictionary<string, int> uploadTable = new Dictionary<string, int>();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                foreach (XmlNode xn in xh.ReadNode("UploadTable").ChildNodes)
                {
                    uploadTable.Add(xn.Attributes["Key"].Value, Convert.ToInt32(xn.Attributes["Value"].Value));
                }
            }
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(uploadTableCacheKey, uploadTable, cd);
        }
    }
}
