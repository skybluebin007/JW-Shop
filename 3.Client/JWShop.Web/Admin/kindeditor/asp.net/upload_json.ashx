<%@ webhandler Language="C#" class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;
using JWShop.Common;
using SkyCES.EntLib;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class Upload : IHttpHandler
{

    protected bool CookiesIsExsits(string key)
    {
        return (this.context.Request.Cookies[key] != null);
    }

    private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        this.context = context;
        //因火狐浏览器Cookie读取有问题会导致多图不能上传，先注释
        //if (Cookies.User.GetUserID(false) == 0 && Cookies.Admin.GetAdminID(false) == 0)
        //{
        //    showError("必须先登录才能上传文件");
        //}
        //String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

        //文件保存目录路径
        String savePath = "/upload/attached/";

        //文件保存目录URL
        //String saveUrl = "/upload/attached/"; 
        String saveUrl = "http://" + HttpContext.Current.Request.Url.Host + "/upload/attached/";

        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

        //最大文件大小KB
        int maxSize = 1024000;

        HttpPostedFile imgFile = context.Request.Files["imgFile"];
        if (imgFile == null)
        {
            showError("请选择文件。");
        }

        String dirPath = context.Server.MapPath(savePath);
        if (!Directory.Exists(dirPath))
        {
            showError("上传目录不存在。");
        }

        String dirName = context.Request.QueryString["dir"];
        if (String.IsNullOrEmpty(dirName)) {
            dirName = "image";
        }
        if (!extTable.ContainsKey(dirName)) {
            showError("目录名不正确。");
        }

        String fileName = imgFile.FileName;
        String fileExt = Path.GetExtension(fileName).ToLower();

        if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        {
            showError("上传的文件不能大于1M");
        }

        if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        }

        //创建文件夹
        dirPath += dirName + "/";
        saveUrl += dirName + "/";
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
        dirPath += ymd + "/";
        saveUrl += ymd + "/";
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }

        string tarname = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
        String newFileName = tarname + fileExt;
        String filePath = dirPath + newFileName;

        int waterType = ShopConfig.ReadConfigInfo().WaterType;
        string newPath = string.Empty;
        string sFileName = string.Empty;
        imgFile.SaveAs(filePath);

        var imgTypes=".jpg|.gif|.bmp|.png|.jpeg";
        if (imgTypes.IndexOf(fileExt) != -1)//必须是图片类型才能加压缩
        {
            if (ShopConfig.ReadConfigInfo().AllImageIsNail == 1)//如果整站设置需要压缩图片
            {
                Image image = Image.FromFile(filePath);
                int imageWidth = ShopConfig.ReadConfigInfo().AllImageWidth;
                if (image.Width > imageWidth)//如果图片超出设置的编辑器图片宽度，将图片压缩
                {
                    string nailPath = dirPath + tarname + "_nail" + fileExt;
                    ImageHelper.MakeThumbnailImage(filePath, nailPath, imageWidth, imageWidth, ThumbnailType.WidthFix);
                    image.Dispose();
                    System.IO.File.Delete(filePath);//删除原图
                    filePath = nailPath;//将图片路径更改为压缩过的图片
                    newFileName = tarname + "_nail" + fileExt;
                }
                image.Dispose();
            }
            if (waterType == 2 || waterType == 3)
            {
                string needMark = RequestHelper.GetQueryString<string>("NeedMark");
                if (needMark == string.Empty)
                {
                    int waterPossition = ShopConfig.ReadConfigInfo().WaterPossition;
                    string text = ShopConfig.ReadConfigInfo().Text;
                    string textFont = ShopConfig.ReadConfigInfo().TextFont;
                    int textSize = ShopConfig.ReadConfigInfo().TextSize;
                    string textColor = ShopConfig.ReadConfigInfo().TextColor;
                    string waterPhoto = System.Web.HttpContext.Current.Server.MapPath(ShopConfig.ReadConfigInfo().WaterPhoto);

                    sFileName = tarname + "_wm" + fileExt;
                    newPath = dirPath + sFileName;
                    if (waterType == 2)
                    {
                        ImageHelper.AddTextWater(filePath, newPath, waterPossition, text, textFont, textColor, textSize);
                    }
                    else
                    {
                        ImageHelper.AddImageWater(filePath, newPath, waterPossition, waterPhoto);
                    }
                    //删除没上水印的老图
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }

        String fileUrl = string.Empty;
        if (waterType == 2 || waterType == 3)
        {
            fileUrl = saveUrl + sFileName;
        }
        else
        {
            fileUrl = saveUrl + newFileName;
        }

        Hashtable hash = new Hashtable();
        hash["error"] = 0;
        hash["url"] = fileUrl;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
