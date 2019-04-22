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
    /// 登录插件
    /// </summary>
    public sealed class LoginPlugins
    {
        private static string path = ServerHelper.MapPath("/Plugins/Login");
        private static string loginPluginsCacheKey = CacheKey.ReadCacheKey("LoginPlugins");
        /// <summary>
        /// 读取登录插件列表
        /// </summary>
        /// <returns></returns>
        public static List<LoginPluginsInfo> ReadLoginPluginsList()
        {
            if (CacheHelper.Read(loginPluginsCacheKey) == null)
            {
                RefreshLoginPluginsCache();
            }
            List<LoginPluginsInfo> loginPluginsList = (List<LoginPluginsInfo>)CacheHelper.Read(loginPluginsCacheKey);
            return loginPluginsList;
        }
        /// <summary>
        /// 刷新登录插件缓存
        /// </summary>
        public static void RefreshLoginPluginsCache()
        {
            List<LoginPluginsInfo> loginPluginsList = new List<LoginPluginsInfo>();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    LoginPluginsInfo loginPlugins = new LoginPluginsInfo();
                    loginPlugins.Key = xh.ReadAttribute("Login/Key", "Value");
                    loginPlugins.Name = xh.ReadAttribute("Login/Name", "Value");
                    loginPlugins.EName = xh.ReadAttribute("Login/EName", "Value");
                    loginPlugins.Photo = xh.ReadAttribute("Login/Photo", "Value");
                    loginPlugins.Description = xh.ReadAttribute("Login/Description", "Value");
                    loginPlugins.IsEnabled = Convert.ToInt32(xh.ReadAttribute("Login/IsEnabled", "Value"));
                    loginPluginsList.Add(loginPlugins);
                }
            }
            CacheHelper.Write(loginPluginsCacheKey, loginPluginsList);
        }
        /// <summary>
        /// 读取一条登录插件的普通信息
        /// </summary>
        /// <returns></returns>
        public static LoginPluginsInfo ReadLoginPlugins(string key)
        {
            LoginPluginsInfo loginPlugins = new LoginPluginsInfo();
            foreach (LoginPluginsInfo temp in ReadLoginPluginsList())
            {
                if (temp.Key == key)
                {
                    loginPlugins = temp;
                    break;
                }
            }
            return loginPlugins;
        }
        /// <summary>
        /// 读取一条登录插件的可配置信息
        /// </summary>
        /// <returns></returns>
        public static void ReadCanChangeLoginPlugins(string key, ref Dictionary<string, string> nameDic, ref Dictionary<string, string> valueDic, ref Dictionary<string, string> selectValueDic)
        {
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (xh.ReadAttribute("Login/Key", "Value") == key)
                    {
                        XmlNodeList nodeList = xh.ReadNode("Login").ChildNodes;
                        foreach (XmlNode xn in nodeList)
                        {
                            if (Convert.ToInt32(xn.Attributes["IsSystem"].Value) == 0)
                            {
                                nameDic.Add(xn.Name, xn.Attributes["Name"].Value);
                                valueDic.Add(xn.Name, xn.Attributes["Value"].Value);
                                if (xn.Attributes["SelectValue"] == null)
                                {
                                    selectValueDic.Add(xn.Name, string.Empty);
                                }
                                else
                                {
                                    selectValueDic.Add(xn.Name, xn.Attributes["SelectValue"].Value);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 修改一条登录插件的信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configDic"></param>
        public static void UpdateLoginPlugins(string key, Dictionary<string, string> configDic)
        {
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (xh.ReadAttribute("Login/Key", "Value") == key)
                    {
                        foreach (KeyValuePair<string, string> keyValue in configDic)
                        {
                            xh.UpdateAttribute("Login/" + keyValue.Key, "Value", keyValue.Value);
                        }
                        xh.Save();
                    }
                }
            }
            RefreshLoginPluginsCache();
        }
        /// <summary>
        /// 读取可以使用的登录插件列表
        /// </summary>
        /// <returns></returns>
        public static List<LoginPluginsInfo> ReadEnabledLoginPluginsList()
        {
            List<LoginPluginsInfo> loginPluginsList = new List<LoginPluginsInfo>();
            foreach (LoginPluginsInfo loginPlugins in ReadLoginPluginsList())
            {
                if (loginPlugins.IsEnabled == (int)BoolType.True)
                {
                    loginPluginsList.Add(loginPlugins);
                }
            }
            return loginPluginsList;
        }
    }
}
