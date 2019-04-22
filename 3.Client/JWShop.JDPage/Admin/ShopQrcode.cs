using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Web;
using Newtonsoft.Json;

namespace JWShop.Page.Admin
{
   public class ShopQrcode : AdminBase
    {
        protected override void PageLoad()
        {
            base.PageLoad();
           
        }


        protected override void PostBack()
        {        

            string imageUrl = "http://" + Request.Url.Host + ShopConfig.ReadConfigInfo().LittlePrgCode;
            string picName = Guid.NewGuid().ToString("N") + ".jpg";
            WriteResponse(picName, GetImageContent(imageUrl));
        }
       
        /// <summary>
        /// 把图片读取为byte数组
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private byte[] GetImageContent(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;

            WebProxy proxy = new WebProxy();
            proxy.BypassProxyOnLocal = true;
            proxy.UseDefaultCredentials = true;

            request.Proxy = proxy;

            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Byte[] buffer = new Byte[1024];
                    int current = 0;
                    while ((current = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        ms.Write(buffer, 0, current);
                    }
                    return ms.ToArray();
                }
            }
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="picName">图片下载显示的文件名</param>
        /// <param name="content">上一个方法得到的byte数组</param>
        private void WriteResponse(string picName, byte[] content)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(picName, Encoding.Default));
            Response.AppendHeader("Content-Length", content.Length.ToString());
            Response.BinaryWrite(content);
            Response.Flush();
            Response.End();
        }
    }
}
