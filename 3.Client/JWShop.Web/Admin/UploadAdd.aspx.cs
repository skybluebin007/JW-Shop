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
    public partial class UploadAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void UploadImage(object sender, EventArgs e)
        {
            //取得传递值
            string control = RequestHelper.GetQueryString<string>("Control");
            int tableID = RequestHelper.GetQueryString<int>("TableID");
            string filePath = RequestHelper.GetQueryString<string>("FilePath");
            string fileType = ShopConfig.ReadConfigInfo().UploadImage;
            if (FileHelper.SafeFullDirectoryName(filePath))
            {
                try
                {
                    //上传文件
                    UploadHelper upload = new UploadHelper();
                    upload.Path = "/Upload/" + filePath + "/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                    upload.FileType = fileType;
                    upload.FileNameType = FileNameType.Guid;
                    upload.MaxWidth = ShopConfig.ReadConfigInfo().AllImageWidth;//整站图片压缩开启后的压缩宽度
                    upload.AllImageIsNail = ShopConfig.ReadConfigInfo().AllImageIsNail;//整站图片压缩开关
                    int needNail = RequestHelper.GetQueryString<int>("NeedNail");
                    if (needNail == 0) upload.AllImageIsNail = 0;//如果页面有传值且值为0不压缩图片，以页面传值为准;
                    int curMaxWidth = RequestHelper.GetQueryString<int>("CurMaxWidth");//页面传值最大宽度
                    if (curMaxWidth > 0)
                    {
                        upload.AllImageIsNail = 1;
                        upload.MaxWidth = curMaxWidth;//如果有页面传值设置图片最大宽度，以页面传值为准
                    }
                    FileInfo file = null;
                    int waterType = ShopConfig.ReadConfigInfo().WaterType;

                    if (waterType == 2 || waterType == 3)
                    {
                        string needMark = RequestHelper.GetQueryString<string>("NeedMark");
                        if (needMark == string.Empty || needMark == "1")
                        {
                            int waterPossition = ShopConfig.ReadConfigInfo().WaterPossition;
                            string text = ShopConfig.ReadConfigInfo().Text;
                            string textFont = ShopConfig.ReadConfigInfo().TextFont;
                            int textSize = ShopConfig.ReadConfigInfo().TextSize;
                            string textColor = ShopConfig.ReadConfigInfo().TextColor;
                            string waterPhoto = Server.MapPath(ShopConfig.ReadConfigInfo().WaterPhoto);

                            file = upload.SaveAs(waterType, waterPossition, text, textFont, textSize, textColor, waterPhoto);
                        }
                        else if (needMark == "0")
                        {
                            file = upload.SaveAs();
                        }
                    }
                    else
                    {
                        file = upload.SaveAs();
                    }
                    //生成处理
                    string originalFile = upload.Path + file.Name;
                    string otherFile = string.Empty;
                    string makeFile = string.Empty;
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    if (tableID == ProductBLL.TableID)
                    {
                      
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.Product))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        if (!dic.ContainsKey(90)) dic.Add(90, 90);//后台商品列表默认使用尺寸(如果不存在则手动添加)
                    }
                    else if (tableID == ProductBrandBLL.TableID)
                    {
                        dic.Add(88, 31);
                    }                    
                    else if (tableID == ThemeActivityBLL.TableID)
                    {
                        dic.Add(300, 150);
                    }                    
                    else if (tableID == ArticleBLL.TableID)
                    {
                       
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.Article))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                    }
                    else if (tableID == FavorableActivityGiftBLL.TableID)
                    {
                        dic.Add(100, 100);
                    }
                    else
                    {

                    }
                    if (dic.Count > 0)
                    {
                        foreach (KeyValuePair<int, int> kv in dic)
                        {
                            makeFile = originalFile.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                            otherFile += makeFile + "|";
                            ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.InBox);
                        }
                        otherFile = otherFile.Substring(0, otherFile.Length - 1);
                    }
                    ResponseHelper.Write("<script>alert('上传成功');  window.parent.o('" + IDPrefix + control + "').value='" + originalFile + "';if(window.parent.o('img_" + control + "')){window.parent.o('img_" + control + "').src='" + originalFile + "';};if(window.parent.o('imgurl_" + control + "')){window.parent.o('imgurl_" + control + "').href='" + originalFile + "';window.parent.o('imgurl_" + control + "').target='_blank'}</script>");
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
                    //ExceptionHelper.ProcessException(ex, false);
                    ResponseHelper.Write("<script>alert('" + ex.Message + "');  </script>");
                }
            }
            else
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ErrorPathName"));
            }
        }
    }
}
