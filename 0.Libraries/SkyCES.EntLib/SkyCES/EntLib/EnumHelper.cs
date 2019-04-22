namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class EnumHelper
    {
        public static string ReadEnumChineseName<T>(int value)
        {
            string str = string.Empty;
            List<EnumInfo> list = ReadEnumList<T>();
            foreach (EnumInfo info in list)
            {
                if (info.Value == value)
                {
                    return info.ChineseName;
                }
            }
            return str;
        }

        public static string ReadEnumEnglishName<T>(int value)
        {
            string str = string.Empty;
            List<EnumInfo> list = ReadEnumList<T>();
            foreach (EnumInfo info in list)
            {
                if (info.Value == value)
                {
                    return info.EnglishName;
                }
            }
            return str;
        }

        public static List<EnumInfo> ReadEnumList<T>()
        {
            List<EnumInfo> list = new List<EnumInfo>();
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo info in fields)
            {
                EnumInfo item = new EnumInfo();
                if (info.GetCustomAttributes(typeof(EnumAttribute), false).Length > 0)
                {
                    item.ChineseName = ((EnumAttribute)info.GetCustomAttributes(typeof(EnumAttribute), false)[0]).ChineseName;
                }

                item.EnglishName = info.Name;
                item.Value = Convert.ToInt32(info.GetRawConstantValue());
                list.Add(item);
            }
            return list;
        }
    }
}

