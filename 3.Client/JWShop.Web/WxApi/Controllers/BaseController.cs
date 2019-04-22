using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using JWShop.XcxApi.Pay;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace JWShop.XcxApi.Controllers
{
  public  class BaseController:Controller
    {
        /// <summary>
        /// 生成小程序码方法
        /// </summary>
        /// <param name="param"></param>
        public void CreateQrCode(WxPayData param, string httpUrl, ref string imageUrl)
        {
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(httpUrl);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                string paraUrlCoded = param.ToJson();
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();//返回图片数据流  
                byte[] tt = ShopCommon.StreamToBytes(s);//将数据流转为byte[]  

                //在文件名前面加上时间，以防重名  
                string imgName = "xcx_" + Guid.NewGuid().ToString() + ".png";
                //文件存储相对于当前应用目录的虚拟目录  
                string path = "/upload/qrcode/";
                //获取相对于应用的基目录,创建目录  
                string imgPath = System.AppDomain.CurrentDomain.BaseDirectory + path;     //通过此对象获取文件名  
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }
                //System.IO.File.WriteAllBytes(Server.MapPath(path + imgName), tt);//讲byte[]存储为图片  

                #region Png

                MemoryStream stream = new MemoryStream(tt);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save(Server.MapPath(path + imgName), System.Drawing.Imaging.ImageFormat.Png);
                #endregion

                imageUrl = path + imgName;
            }
            catch (Exception e)
            {
                Log.Error("CreateQrCode", e.ToString());

            }
        }

        /// <summary>
        /// 存储formid
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="uid"></param>
        public void SaveFormId(string formid, int uid)
        {
            if (!string.IsNullOrEmpty(formid))
            {
                WxFormIdBLL.Add(new WxFormIdInfo
                {
                    FormId = formid,
                    Used = 0,
                    UserId = uid,
                    AddDate = DateTime.Now
                });
            }
        }
    }
}
