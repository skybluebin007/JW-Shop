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
    public partial class UploadUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Upload(object sender, EventArgs e)
        {
            //取得传递值
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
                    upload.Path = "/Upload/attached/" + filePath + "/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                    upload.FileType = fileType;
                    upload.FileNameType = FileNameType.Guid;
                    FileInfo file = upload.SaveAs();

                    string originalFile = upload.Path + file.Name;

                    string script = "<script>window.parent.importdata('" + originalFile + "');";
                    script += "</script>";

                    ResponseHelper.Write(script);
                }
                catch (Exception ex)
                {
                    //ExceptionHelper.ProcessException(ex, false);
                    ResponseHelper.Write("<script>alert('"+ex.Message+"') </script>");
                }
            }
            else
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ErrorPathName"));
            }
        }
    }
}