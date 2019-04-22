namespace SkyCES.EntLib
{
    using System;
    using System.IO;
    using System.Web;

    public class DownFile
    {
        private string fileName = string.Empty;
        public DownFinishedHandler OnDownFinished;

        public void Down()
        {
            FileInfo info = new FileInfo(this.fileName);
            if (info.Exists)
            {
                HttpContext.Current.Response.Clear();
                byte[] buffer = new byte[0x19000L];
                FileStream stream = File.OpenRead(this.fileName);
                long length = stream.Length;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(this.fileName));
                while ((length > 0L) && HttpContext.Current.Response.IsClientConnected)
                {
                    int count = stream.Read(buffer, 0, Convert.ToInt32((long) 0x19000L));
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, count);
                    HttpContext.Current.Response.Flush();
                    length -= count;
                }
                if (this.OnDownFinished != null)
                {
                    this.OnDownFinished(this, new EventArgs());
                }
                HttpContext.Current.Response.Close();
                stream.Close();
            }
            else
            {
                ResponseHelper.Write("未找到该文件");
            }
            ResponseHelper.End();
        }

        public string FileName
        {
            set
            {
                this.fileName = value;
            }
        }
    }
}

