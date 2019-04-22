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
    public partial class ProductPhotoAdd : JWShop.Page.AdminBasePage
    {

        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            //try
            //{
                //上传文件
                //UploadHelper upload = new UploadHelper();
                //upload.Path = "/Upload/ProductPhoto/Original/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                //upload.FileNameType = FileNameType.Guid;
                //upload.FileType = ShopConfig.ReadConfigInfo().UploadFile;
                //FileInfo file = upload.SaveAs();
                string filePath= "/Upload/ProductPhoto/Original/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                if (FileHelper.SafeFullDirectoryName(filePath))
                {
                    try
                    {
                        //上传文件
                        UploadHelper upload = new UploadHelper();
                        upload.Path = "/Upload/ProductPhoto/Original/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                        upload.FileType = ShopConfig.ReadConfigInfo().UploadFile;
                        upload.FileNameType = FileNameType.Guid;
                        upload.MaxWidth = ShopConfig.ReadConfigInfo().AllImageWidth;//整站图片压缩开启后的压缩宽度
                        upload.AllImageIsNail = ShopConfig.ReadConfigInfo().AllImageIsNail;//整站图片压缩开关
                        int needNail = RequestHelper.GetQueryString<int>("NeedNail");
                        if (needNail <= 0) upload.AllImageIsNail = 0;//如果页面传值不压缩图片，以页面传值为准;
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
                        //Hashtable ht = new Hashtable();
                        //ht.Add("60", "60");
                        //ht.Add("90", "60");
                        //ht.Add("240", "180");
                        //ht.Add("340", "340");
                        //ht.Add("418", "313");
                        Dictionary<int, int> dic = new Dictionary<int, int>();
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.ProductPhoto))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        if (!dic.ContainsKey(75)) dic.Add(75, 75);//后台商品图集默认使用尺寸(如果不存在则手动添加)

                        foreach (KeyValuePair<int, int> de in dic)
                        {
                            makeFile = originalFile.Replace("Original", de.Key + "-" + de.Value);
                            otherFile += makeFile + "|";
                            ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), Convert.ToInt32(de.Key), Convert.ToInt32(de.Value), ThumbnailType.AllFix);
                        }
                        otherFile = otherFile.Substring(0, otherFile.Length - 1);
                        int proStyle = RequestHelper.GetQueryString<int>("proStyle");
                        if (proStyle < 0)
                        {
                            proStyle = 0;
                        }
                        ResponseHelper.Write("<script>window.parent.addProductPhoto('" + originalFile.Replace("Original", "75-75") + "','" + Name.Text + "','" + proStyle + "');</script>");
                        //保存数据库
                        UploadInfo tempUpload = new UploadInfo();
                        tempUpload.TableID = ProductPhotoBLL.TableID;
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