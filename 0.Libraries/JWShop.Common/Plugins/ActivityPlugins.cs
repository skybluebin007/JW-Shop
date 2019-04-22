using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// 活动插件
    /// </summary>
    public class ActivityPlugins
    {
        private static string path = ServerHelper.MapPath("/Plugins/Activity");
        private static string activityCacheKey = CacheKey.ReadCacheKey("Activity");
        /// <summary>
        /// 读取活动插件列表
        /// </summary>
        /// <returns></returns>
        public static List<ActivityPluginsInfo> ReadActivityPluginsList()
        {
            if (CacheHelper.Read(activityCacheKey) == null)
            {
                RefreshActivityPluginsCache();
            }
            List<ActivityPluginsInfo> activityPluginsList = (List<ActivityPluginsInfo>)CacheHelper.Read(activityCacheKey);
            return activityPluginsList;
        }
        /// <summary>
        /// 读取可用的活动插件列表
        /// </summary>
        /// <returns></returns>
        public static List<ActivityPluginsInfo> ReadIsEnabledActivityPluginsList()
        {
            List<ActivityPluginsInfo> activityPluginsList = new List<ActivityPluginsInfo>();
            foreach (ActivityPluginsInfo activityPlugins in ReadActivityPluginsList())
            {
                if (activityPlugins.IsEnabled == (int)BoolType.True)
                {
                    activityPluginsList.Add(activityPlugins);
                }
            }
            return activityPluginsList;
        }
        /// <summary>
        /// 刷新活动插件缓存
        /// </summary>
        /// <returns></returns>
        public static void RefreshActivityPluginsCache()
        {
            List<ActivityPluginsInfo> activityPluginsList = new List<ActivityPluginsInfo>();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                if (file.FullName.ToLower().IndexOf("\\common.config") > -1)
                {
                    using (XmlHelper xh = new XmlHelper(file.FullName))
                    {
                        ActivityPluginsInfo activityPlugins = new ActivityPluginsInfo();
                        activityPlugins.Name = xh.ReadAttribute("Activity/Name", "Value");
                        activityPlugins.Key = xh.ReadAttribute("Activity/Key", "Value");                       
                        activityPlugins.Description = xh.ReadAttribute("Activity/Description", "Value");
                        activityPlugins.AdminUrl = xh.ReadAttribute("Activity/AdminUrl", "Value");
                        activityPlugins.ShowUrl = xh.ReadAttribute("Activity/ShowUrl", "Value");
                        activityPlugins.Photo = xh.ReadAttribute("Activity/Photo", "Value");
                        activityPlugins.IsEnabled = Convert.ToInt32(xh.ReadAttribute("Activity/IsEnabled", "Value"));
                        activityPlugins.ApplyVersion = xh.ReadAttribute("Activity/ApplyVersion", "Value");
                        activityPlugins.CopyRight = xh.ReadAttribute("Activity/CopyRight", "Value");
                        activityPluginsList.Add(activityPlugins);
                    }
                }
            }
            CacheHelper.Write(activityCacheKey, activityPluginsList);
        }
        /// <summary>
        /// 修改一条活动插件的信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configDic"></param>
        public static void UpdateActivityPlugins(string key, Dictionary<string, string> configDic)
        {
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                if (file.FullName.ToLower().IndexOf("\\common.config") > -1)
                {
                    using (XmlHelper xh = new XmlHelper(file.FullName))
                    {
                        if (xh.ReadAttribute("Activity/Key", "Value") == key)
                        {
                            foreach (KeyValuePair<string, string> keyValue in configDic)
                            {
                                xh.UpdateAttribute("Activity/" + keyValue.Key, "Value", keyValue.Value);
                            }
                            xh.Save();
                        }
                    }
                }
            }
            RefreshActivityPluginsCache();
        }
    }
}
