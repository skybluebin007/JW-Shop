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
    /// 模板插件
    /// </summary>
    public class TemplatePlugins
    {
        private static string path = ServerHelper.MapPath("/Plugins/Template");
        private static string templateCacheKey = CacheKey.ReadCacheKey("Template");
        /// <summary>
        /// 读取模板插件列表
        /// </summary>
        /// <returns></returns>
        public static List<TemplatePluginsInfo> ReadTemplatePluginsList()
        {
            if (CacheHelper.Read(templateCacheKey) == null)
            {
                RefreshTemplateCache();
            }
            List<TemplatePluginsInfo> templatePluginsList = (List<TemplatePluginsInfo>)CacheHelper.Read(templateCacheKey);
            return templatePluginsList;
        }
        /// <summary>
        /// 刷新模板缓存
        /// </summary>
        /// <returns></returns>
        public static void RefreshTemplateCache()
        {
            List<TemplatePluginsInfo> templatePluginsList = new List<TemplatePluginsInfo>();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.xml|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    TemplatePluginsInfo templatePlugins = new TemplatePluginsInfo();
                    templatePlugins.Path = xh.ReadAttribute("Template/Path", "Value");
                    templatePlugins.Name = xh.ReadAttribute("Template/Name", "Value");
                    templatePlugins.Photo = xh.ReadAttribute("Template/Photo", "Value");
                    templatePlugins.DisCreateFile = xh.ReadAttribute("Template/DisCreateFile", "Value");
                    templatePlugins.CopyRight = xh.ReadAttribute("Template/CopyRight", "Value");
                    templatePlugins.PublishDate = xh.ReadAttribute("Template/PublishDate", "Value");
                    templatePluginsList.Add(templatePlugins);
                }
            }
            CacheHelper.Write(templateCacheKey, templatePluginsList);
        }
        /// <summary>
        /// 读取一条模板插件
        /// </summary>
        /// <returns></returns>
        public static TemplatePluginsInfo ReadTemplatePlugins(string path)
        {
            List<TemplatePluginsInfo> templatePluginsList = ReadTemplatePluginsList();
            TemplatePluginsInfo templatePlugins = new TemplatePluginsInfo();
            foreach (TemplatePluginsInfo temp in templatePluginsList)
            {
                if (temp.Path == path)
                {
                    templatePlugins = temp;
                    break;
                }
            }
            return templatePlugins;
        }
    }
}
