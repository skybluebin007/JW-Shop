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
    /// 支付方式
    /// </summary>
    public class PayPlugins
    {
        private static string path = ServerHelper.MapPath("/Plugins/Pay");
        private static string payPluginsCacheKey = CacheKey.ReadCacheKey("PayPlugins");
        /// <summary>
        /// 读取支付方式列表
        /// </summary>
        /// <returns></returns>
        public static List<PayPluginsInfo> ReadPayPluginsList()
        {
            if (CacheHelper.Read(payPluginsCacheKey) == null)
            {
                RefreshPayPluginsCache();
            }
            List<PayPluginsInfo> payPluginsList = (List<PayPluginsInfo>)CacheHelper.Read(payPluginsCacheKey);
            return payPluginsList;
        }
        /// <summary>
        /// 刷新支付方式缓存
        /// </summary>
        public static void RefreshPayPluginsCache()
        {
            List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    PayPluginsInfo payPlugins = new PayPluginsInfo();
                    payPlugins.Key = xh.ReadAttribute("Pay/Key", "Value");
                    payPlugins.Name = xh.ReadAttribute("Pay/Name", "Value");
                    payPlugins.Photo = xh.ReadAttribute("Pay/Photo", "Value");
                    payPlugins.Description = xh.ReadAttribute("Pay/Description", "Value");
                    payPlugins.IsCod = Convert.ToInt32(xh.ReadAttribute("Pay/IsCod", "Value"));
                    payPlugins.IsOnline = Convert.ToInt32(xh.ReadAttribute("Pay/IsOnline", "Value"));
                    payPlugins.IsEnabled = Convert.ToInt32(xh.ReadAttribute("Pay/IsEnabled", "Value"));
                    payPluginsList.Add(payPlugins);
                }
            }
            CacheHelper.Write(payPluginsCacheKey, payPluginsList);
        }
        /// <summary>
        /// 读取一条支付方式的普通信息
        /// </summary>
        /// <returns></returns>
        public static PayPluginsInfo ReadPayPlugins(string key)
        {
            PayPluginsInfo payPlugins = new PayPluginsInfo();
            foreach(PayPluginsInfo temp in ReadPayPluginsList())
            {
                if (temp.Key == key)
                {
                    payPlugins = temp;
                    break;
                }
            }
            return payPlugins;
        }
        /// <summary>
        /// 读取一条支付方式的可配置信息
        /// </summary>
        /// <returns></returns>
        public static void ReadCanChangePayPlugins(string key,ref Dictionary<string, string> nameDic,ref Dictionary<string, string> valueDic,ref Dictionary<string,string> selectValueDic)
        {
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (xh.ReadAttribute("Pay/Key", "Value") == key)
                    {
                        XmlNodeList nodeList = xh.ReadNode("Pay").ChildNodes;
                        foreach (XmlNode xn in nodeList)
                        {
                            if (Convert.ToInt32(xn.Attributes["IsSystem"].Value) == 0)
                            {
                                nameDic.Add(xn.Name, xn.Attributes["Name"].Value);
                                valueDic.Add(xn.Name, xn.Attributes["Value"].Value);
                                if (xn.Attributes["SelectValue"]==null)
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
        /// 修改一条支付方式的信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configDic"></param>
        public static void UpdatePayPlugins(string key, Dictionary<string, string> configDic)
        {
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (xh.ReadAttribute("Pay/Key", "Value") == key)
                    {
                        foreach (KeyValuePair<string, string> keyValue in configDic)
                        {
                            xh.UpdateAttribute("Pay/" + keyValue.Key, "Value", keyValue.Value);
                        }
                        xh.Save();
                    }
                }
            }
            RefreshPayPluginsCache();
        }
        /// <summary>
        /// 读取在线充值支付方式列表
        /// </summary>
        /// <returns></returns>
        public static List<PayPluginsInfo> ReadRechargePayPluginsList()
        {
            List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
            foreach (PayPluginsInfo payPlugins in ReadPayPluginsList())
            {
                if (payPlugins.IsEnabled == (int)BoolType.True && payPlugins.IsOnline == (int)BoolType.True && payPlugins.IsCod==(int)BoolType.False)
                {
                    payPluginsList.Add(payPlugins);
                }
            }
            return payPluginsList;
        }
        /// <summary>
        /// 读取商品购买支付方式列表
        /// </summary>
        /// <returns></returns>
        public static List<PayPluginsInfo> ReadProductBuyPayPluginsList()
        {
            List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
            foreach (PayPluginsInfo payPlugins in ReadPayPluginsList())
            {
                if (payPlugins.IsEnabled == (int)BoolType.True)
                {
                    payPluginsList.Add(payPlugins);
                }
            }
            return payPluginsList;
        }
    }
}
