using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace JWShop.Page.Admin
{
   public class ShopBannerAdd:AdminBase
    {
        protected AdImageInfo adImage = new AdImageInfo();
        protected int flashId = 0;
        protected override void PageLoad()
        {
            base.PageLoad();
            //flashId 默认 11，店铺首页Banner
            flashId = RequestHelper.GetQueryString<int>("flashId")<=0?11: RequestHelper.GetQueryString<int>("flashId");
            int id = RequestHelper.GetQueryString<int>("id");
            adImage = AdImageBLL.Read(id);
            topNav = 2;
            if (RequestHelper.GetQueryString<string>("Action") == "UploadPhoto")
            {
                UploadPhoto();
            }
        }
        /// <summary>
        /// ajaxfileUpload 上传图片
        /// </summary>
        /// <returns></returns>
        protected void UploadPhoto()
        {
            var flag = true;
            string originalFile = string.Empty;
            if (HttpContext.Current.Request.Files[0].FileName != string.Empty)
            {
                try
                {
                    //上传文件
                    UploadHelper upload = new UploadHelper();
                    upload.Path = "/Upload/WaterPhoto/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                    upload.FileType = ShopConfig.ReadConfigInfo().UploadFile;
                    FileInfo file = upload.SaveAs();
                    //生成处理
                    originalFile = upload.Path + file.Name;
                    string otherFile = string.Empty;
                    string makeFile = string.Empty;
                    //Dictionary<int, int> dic = new Dictionary<int, int>();
                    //dic.Add(70, 70);
                    //dic.Add(190, 190);
                    //foreach (KeyValuePair<int, int> kv in dic)
                    //{
                    //    makeFile = originalFile.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                    //    otherFile += makeFile + "|";
                    //    ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.InBox);
                    //}
                    //otherFile = otherFile.Substring(0, otherFile.Length - 1);
                    ////保存数据库
                    //UploadInfo tempUpload = new UploadInfo();
                    //tempUpload.TableID = UserBLL.TableID;
                    //tempUpload.ClassID = 0;
                    //tempUpload.RecordID = 0;
                    //tempUpload.UploadName = originalFile;
                    //tempUpload.OtherFile = otherFile;
                    //tempUpload.Size = Convert.ToInt32(file.Length);
                    //tempUpload.FileType = file.Extension;
                    //tempUpload.Date = RequestHelper.DateNow;
                    //tempUpload.IP = ClientHelper.IP;
                    //UploadBLL.AddUpload(tempUpload);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ProcessException(ex, false);
                }
            }
            if (!string.IsNullOrEmpty(originalFile))
            {
                flag = true;             
            }
            else
            {
                flag = false;
            }
            //return originalFile;
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { flag = flag, imgPath = originalFile }));
            Response.End();
        }
    }
}
