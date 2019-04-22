namespace SkyCES.EntLib
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Web;

    public sealed class ServerHelper
    {
        public static object CopyClass(object objClass)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0L;
                formatter.Serialize(stream, objClass);
                stream.Position = 0L;
                return formatter.Deserialize(stream);
            }
        }

        public static string MapPath(string filePath)
        {
            string str = string.Empty;
            try
            {
                str = HttpContext.Current.Server.MapPath(filePath);
            }
            catch (Exception exception)
            {
                ResponseHelper.Write(exception.ToString());
                ResponseHelper.End();               
            }
            return str;
        }

        public static string Path
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}

