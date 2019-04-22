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
    public sealed class ShopConfig
    {
        private static string configCacheKey = CacheKey.ReadCacheKey("Config");
        /// <summary>
        /// 从缓存读取设置值
        /// </summary>
        /// <returns></returns>
        public static ShopConfigInfo ReadConfigInfo()
        {
            if (CacheHelper.Read(configCacheKey) == null)
            {
                RefreshConfigCache();
            }
            return (ShopConfigInfo)CacheHelper.Read(configCacheKey);
        }
        /// <summary>
        /// 刷新全局设置
        /// </summary>
        public static void RefreshConfigCache()
        {
            string fileName = ServerHelper.MapPath("~/Config/ShopConfig.config");
            ShopConfigInfo config = ConfigHelper.ReadPropertyFromXml<ShopConfigInfo>(fileName);
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(configCacheKey, config, cd);
            RefreshApp();
        }
        /// <summary>
        /// 更新全局设置
        /// </summary>
        /// <param name="config"></param>
        public static void UpdateConfigInfo(ShopConfigInfo config)
        {
            string fileName = ServerHelper.MapPath("~/Config/ShopConfig.config");
            ConfigHelper.UpdatePropertyToXml<ShopConfigInfo>(fileName, config);
        }
        /// <summary>
        /// 应用程序相关设置刷新
        /// </summary>
        public static void RefreshApp()
        {
            //地址重写
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(".aspx", ".ashx");
            dic.Add(".html", ".ashx");
            URLRewriterModule.ReplaceFileTypeDic = dic;
            URLRewriterModule.Path = "/Ashx/" + ShopConfig.ReadConfigInfo().TemplatePath;
            URLRewriterModule.ForbidFolder = "|/Admin/|/Admin|/Plugins/|/Install/|/Manage/|/Manage|";
            //验证码
            CheckCode.CodeDot = ShopConfig.ReadConfigInfo().CodeDot;
            CheckCode.CodeLength = ShopConfig.ReadConfigInfo().CodeLength;
            CheckCode.CodeType = (CodeType)ShopConfig.ReadConfigInfo().CodeType;
            CheckCode.Key = ShopConfig.ReadConfigInfo().SecureKey;
        }
    }
}
