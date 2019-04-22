using System;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// 虚拟文件系统
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
    public class ShopPathProvider : VirtualPathProvider
    {
        private ShopVirtualFile shopVirtualFile;
        public ShopPathProvider()
            : base()
        {
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initialize()
        {
        }
        /// <summary>
        /// 是否虚拟目录
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        private bool IsPathVirtual(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/ashx", StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public override bool FileExists(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
            {
                shopVirtualFile = new ShopVirtualFile(virtualPath);
                return shopVirtualFile.Exists;
            }
            else
            {
                return Previous.FileExists(virtualPath);
            }
        }
        /// <summary>
        /// 获取虚拟文件
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
            {
                return shopVirtualFile;
            }
            else
            {
                return Previous.GetFile(virtualPath);
            }
        }
        /// <summary>
        /// 取得缓存依赖项
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="virtualPathDependencies"></param>
        /// <param name="utcStart"></param>
        /// <returns></returns>
        public override CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsPathVirtual(virtualPath))
            {
                string path1 = ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/");
                CacheDependency cd1 = new CacheDependency(path1);
                string path2 = ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/User/");
                CacheDependency cd2 = new CacheDependency(path2);
                string path3 = ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/Mobile/");
                CacheDependency cd3 = new CacheDependency(path3);
                string path4 = ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/Mobile/User/");
                CacheDependency cd4 = new CacheDependency(path4);
                string path5 = ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/MobileAdmin/");
                CacheDependency cd5 = new CacheDependency(path5);
                AggregateCacheDependency acd = new AggregateCacheDependency();
                acd.Add(cd1);
                acd.Add(cd2);
                acd.Add(cd3);
                acd.Add(cd4);
                acd.Add(cd5);

                return acd;
            }
            else
            {
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
        }
    }
}