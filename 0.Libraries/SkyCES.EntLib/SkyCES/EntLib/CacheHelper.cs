namespace SkyCES.EntLib
{
    using System;
    using System.Web;
    using System.Web.Caching;

    public sealed class CacheHelper
    {
        public static object Read(string cacheKey)
        {
            if (cacheKey == string.Empty)
            {
                throw new Exception("未定义的缓存项");
            }
            return HttpRuntime.Cache[cacheKey];
        }

        public static void Remove(string cacheKey)
        {
            try
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
            catch
            {
            }
        }

        public static void Write(string cacheKey, object cacheValue)
        {
            if (HttpRuntime.Cache[cacheKey.ToString()] == null)
            {
                HttpRuntime.Cache.Add(cacheKey, cacheValue, null, DateTime.Now.AddYears(1), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                HttpRuntime.Cache.Insert(cacheKey, cacheValue, null, DateTime.Now.AddYears(1), TimeSpan.Zero);
            }
        }

        public static void Write(string cacheKey, object cacheValue, DateTime dateTime)
        {
            if (HttpRuntime.Cache[cacheKey.ToString()] == null)
            {
                HttpRuntime.Cache.Add(cacheKey, cacheValue, null, dateTime, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                HttpRuntime.Cache.Insert(cacheKey, cacheValue, null, dateTime, TimeSpan.Zero);
            }
        }

        public static void Write(string cacheKey, object cacheValue, CacheDependency cd)
        {
            if (HttpRuntime.Cache[cacheKey] == null)
            {
                HttpRuntime.Cache.Add(cacheKey, cacheValue, cd, DateTime.Now.AddYears(1), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                HttpRuntime.Cache.Insert(cacheKey, cacheValue, cd, DateTime.Now.AddYears(1), TimeSpan.Zero);
            }
        }
    }
}

