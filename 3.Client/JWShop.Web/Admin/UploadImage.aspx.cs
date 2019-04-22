using System;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class UploadImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //UploadFile.Attributes.Add("onchange", "upchange(this)");
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Upload(object sender, EventArgs e)
        {
            //取得传递值
            string control = RequestHelper.GetQueryString<string>("Control");
            int tableID = RequestHelper.GetQueryString<int>("TableID");
            string filePath = RequestHelper.GetQueryString<string>("FilePath");
            string fileType = RequestHelper.GetQueryString<string>("FileType");
            if (!string.IsNullOrEmpty(fileType))
            {
                if (fileType == "Image")
                {
                    fileType = ShopConfig.ReadConfigInfo().UploadImage;
                }
                else
                {
                    fileType = ShopConfig.ReadConfigInfo().UploadFile;
                }
            }
            else
            {
                fileType = ShopConfig.ReadConfigInfo().UploadFile;
            }
            if (FileHelper.SafeFullDirectoryName(filePath))
            {
                try
                {
                    //上传文件
                    UploadHelper upload = new UploadHelper();
                    upload.Path = "/Upload/" + filePath + "/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                    upload.FileType = fileType;
                    upload.FileNameType = FileNameType.Guid;
                    FileInfo file = upload.SaveAs();
                    //生成处理
                    string originalFile = upload.Path + file.Name;
                    string otherFile = string.Empty;
                    string makeFile = string.Empty;
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    if (tableID == UserBLL.TableID)
                    {
                        dic.Add(150, 150);
                    }
                    if (dic.Count > 0)
                    {
                        foreach (KeyValuePair<int, int> kv in dic)
                        {
                            makeFile = originalFile.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                            otherFile += makeFile + "|";
                            ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.AllFix);
                        }
                        otherFile = otherFile.Substring(0, otherFile.Length - 1);
                    }

                    string script = "<script> window.parent.o('" + control + "').value='" + originalFile + "';var warningMessageObj = window.parent.o('" + control + "WarningMessage'); (typeof warningMessageObj.textContent == 'string') ? warningMessageObj.textContent='上传成功。' : warningMessageObj.innerText='上传成功。';window.parent.triggerValidate('" + control + "');";
                    if (RequestHelper.GetQueryString<string>("Required") == "1")
                    {
                        script += "window.parent.o('Check" + control + "').value='1';";
                    }
                    script += "</script>";

                    ResponseHelper.Write(script);
                    //保存数据库
                    UploadInfo tempUpload = new UploadInfo();
                    tempUpload.TableID = tableID;
                    tempUpload.ClassID = 0;
                    tempUpload.RecordID = 0;
                    tempUpload.UploadName = originalFile;
                    tempUpload.OtherFile = otherFile;
                    tempUpload.Size = Convert.ToInt32(file.Length);
                    tempUpload.FileType = file.Extension;
                    tempUpload.RandomNumber = Cookies.Admin.GetRandomNumber(false);
                    tempUpload.Date = RequestHelper.DateNow;
                    tempUpload.IP = ClientHelper.IP;
                    UploadBLL.AddUpload(tempUpload);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ProcessException(ex, false);
                }
            }
            else
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ErrorPathName"));
            }
        }
    }
}