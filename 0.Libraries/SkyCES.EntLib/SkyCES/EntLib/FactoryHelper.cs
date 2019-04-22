namespace SkyCES.EntLib
{
    using System;
    using System.Reflection;

    public sealed class FactoryHelper
    {
        private static object CreateObject(string path, string className)
        {
            className = path + "." + className;
            object cacheValue = CacheHelper.Read(className);
            if (cacheValue == null)
            {
                try
                {
                    cacheValue = Assembly.Load(path).CreateInstance(className);
                    CacheHelper.Write(className, cacheValue);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return cacheValue;
        }

        public static T Instance<T>(string path, string className)
        {
            return (T) CreateObject(path, className);
        }
    }
}

