using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace JWShop.Page.Admin
{
   public class ShopQualify : AdminBase
    {
        protected override void PageLoad()
        {
            base.PageLoad();
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
                    upload.Path = "/Upload/WaterPhoto/"+ RequestHelper.DateNow.ToString("yyyyMM") + "/";
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
                #region shopconfig设置
                ShopConfigInfo config = ShopConfig.ReadConfigInfo();
                config.Qualification = originalFile;
                ShopConfig.UpdateConfigInfo(config);
                #endregion
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
