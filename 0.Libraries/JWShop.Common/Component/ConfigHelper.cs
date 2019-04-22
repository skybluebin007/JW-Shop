using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// 系统设置类
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 读取类的属性,更新Xml文件
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="fileName">Xml文件路径</param>
        /// <returns></returns>
        public static void UpdatePropertyToXml<T>(string fileName, T t)
        {
            PropertyInfo[] pi = typeof(T).GetProperties();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                foreach (PropertyInfo p in pi)
                {
                    object oj = p.GetValue(t, null);
                    if (oj == null)
                    {
                        continue;
                    }
                    try
                    {
                        xh.UpdateAttribute("Config/" + p.Name, "Value", oj.ToString());
                    }
                    catch
                    {
                        xh.InsertElement("Config", p.Name, "Value", oj.ToString(), "");
                    }
                }
                xh.Save();
            }
        }

        /// <summary>
        /// 从Xml中读取值，并赋给类的属性
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="fileName">Xml文件路径</param>
        /// <returns></returns>
        public static T ReadPropertyFromXml<T>(string fileName) where T : new()
        {
            T result = new T();
            PropertyInfo[] pi = typeof(T).GetProperties();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                foreach (PropertyInfo p in pi)
                {
                    object innerText = xh.ReadAttribute("Config/" + p.Name, "Value");
                    if (p.PropertyType == typeof(System.Int32))
                    {
                        p.SetValue(result, Convert.ToInt32(innerText), null);
                    }
                    else if (p.PropertyType == typeof(System.DateTime))
                    {
                        p.SetValue(result, Convert.ToDateTime(innerText), null);
                    }
                    else if (p.PropertyType == typeof(System.Decimal))
                    {
                        p.SetValue(result, Convert.ToDecimal(innerText), null);
                    }
                    else if (p.PropertyType == typeof(System.Double))
                    {
                        p.SetValue(result, Convert.ToDouble(innerText), null);
                    }
                    else
                    {
                        p.SetValue(result, innerText, null);
                    }
                }
            }
            return result;
        }
    }
}
