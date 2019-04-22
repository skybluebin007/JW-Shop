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
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

public class Upload : IHttpHandler
{
	private HttpContext context;

	public void ProcessRequest(HttpContext context)
	{
        String aspxUrl = "/" + ShopConfig.ReadConfigInfo().FilePath+"/";
        
		//文件保存目录路径
        String savePath = aspxUrl;

		//文件保存目录URL
        String saveUrl = aspxUrl;

		//定义允许上传的文件扩展名
		Hashtable extTable = new Hashtable();
		extTable.Add("image", "gif,jpg,jpeg,png,bmp");
		extTable.Add("flash", "swf,flv");
		extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
		extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2,pdf");

		//最大文件大小
        //context.Response.Write(ShopConfig.ReadConfigInfo().Title);
        //context.Response.End();
        int maxSize = Convert.ToInt32(ShopConfig.ReadConfigInfo().Filesize);
		this.context = context;

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
			showError("上传文件大小超过限制。");
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

		String newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
		String filePath = dirPath + newFileName;

		imgFile.SaveAs(filePath);

        /*-------------------自动压缩图片，最大宽度740px--------------------------------*/
        int imageHeight = 0;
        string oldFileName = newFileName;
        newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
        using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
        {
            imageHeight = image.Height;
        }
        if (imageHeight > 0)
        {
            ImageHelper.MakeThumbnailImage(context.Server.MapPath(saveUrl + oldFileName), context.Server.MapPath(saveUrl + newFileName), 740, imageHeight, ThumbnailType.InBox);
        }

        //删除原图
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        /*------------------------------------------------------------------------------*/


        /*-------------------图片水印---------------------------------------------------*/
        /*
        int waterType = ShopConfig.ReadConfigInfo().WaterType;
        int waterPossition = ShopConfig.ReadConfigInfo().WaterPossition;
        string text = ShopConfig.ReadConfigInfo().Text;
        string textFont = ShopConfig.ReadConfigInfo().TextFont;
        int textSize = ShopConfig.ReadConfigInfo().TextSize;
        string textColor = ShopConfig.ReadConfigInfo().TextColor;
        string waterPhoto = context.Server.MapPath(ShopConfig.ReadConfigInfo().WaterPhoto);
        if (waterType == 2 || waterType == 3)
        {
            //添加水印
            newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            string newPath = System.IO.Path.Combine(dirPath, newFileName);
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
        }*/
        /*------------------------------------------------------------------------------*/

        String fileUrl = saveUrl + newFileName;

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
