using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.IO;

namespace SocoShop.Web.Admin
{
    public partial class EditPhoto : JWShop.Page.AdminBasePage
    {
        protected string targetPhoto;
        protected int tableID;
        protected string targetID;
        protected int makeNail = 0;
        protected int targetType = 0;//默认为0表示封面图，1表示多图
        protected int ProductID =0;
        protected int ProStyle = 0;
        protected int ProductPhotoID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            targetPhoto = RequestHelper.GetQueryString<string>("Photo");
            tableID = RequestHelper.GetQueryString<int>("TableID");
            if (RequestHelper.GetQueryString<int>("MakeNail") > 0) makeNail = RequestHelper.GetQueryString<int>("MakeNail");
            if (RequestHelper.GetQueryString<int>("TargetType") > 0) targetType = RequestHelper.GetQueryString<int>("TargetType");
            if (RequestHelper.GetQueryString<int>("ProductID") > 0) ProductID = RequestHelper.GetQueryString<int>("ProductID");
            if (RequestHelper.GetQueryString<int>("ProStyle") > 0) ProStyle = RequestHelper.GetQueryString<int>("ProStyle");
            if (RequestHelper.GetQueryString<int>("ProductPhotoID") > 0) ProductPhotoID = RequestHelper.GetQueryString<int>("ProductPhotoID");    
            targetID = RequestHelper.GetQueryString<string>("TargetID");
            if (targetPhoto == string.Empty)
            {
                target.ImageUrl = "/Admin/Images/test.jpg";
                preview.ImageUrl = "/Admin/Images/test.jpg";
                himg.Value = "/Admin/Images/test.jpg";
            }
            else
            {
                target.ImageUrl = targetPhoto;
                preview.ImageUrl = targetPhoto;
                himg.Value = targetPhoto;
            }
        }

        ///   <summary>  
        ///   从图片中截取部分生成新图  
        ///   </summary>  
        ///   <param   name="sFromFilePath">原始图片</param>  
        ///   <param   name="saveFilePath">生成新图</param>  
        ///   <param   name="width">截取图片宽度</param>  
        ///   <param   name="height">截取图片高度</param>  
        ///   <param   name="spaceX">截图图片X坐标</param>  
        ///   <param   name="spaceY">截取图片Y坐标</param>  
        public void CaptureImage(string sFromFilePath, string saveFilePath, int width, int height, int spaceX, int spaceY, string tarimg,string targetID)
        {
            //载入底图  
            System.Drawing.Image fromImage = System.Drawing.Image.FromFile(sFromFilePath);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            //创建作图区域  
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区  
            graphic.DrawImage(fromImage, new System.Drawing.Rectangle(0, 0, width, height), new System.Drawing.Rectangle(spaceX, spaceY, width, height), System.Drawing.GraphicsUnit.Pixel);
            //从作图区生成新图  
            System.Drawing.Image saveImage = System.Drawing.Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图象  
            saveImage.Save(saveFilePath, ImageFormat.Jpeg);
            bitmap.Dispose();
            graphic.Dispose();
            saveImage.Dispose();
            fromImage.Dispose();

            //string delTarget = tarimg.Replace("nail.", ".");
            //FileHelper.DeleteFile(new List<string> { delTarget });//删除原图
            FileHelper.DeleteFile(new List<string> { tarimg });//删除原图

            string delTarget = saveFilePath.Replace(Server.MapPath("/"), "");
            //FileInfo fileInfo = new FileInfo(saveFilePath);
            //fileInfo.MoveTo(sFromFilePath);

            if (makeNail == 1)//是否需要生成缩略图
            {
                string makeFile = string.Empty;
                //if (targetType == 0)
                //{                    
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
                        dic.Add(90, 90);//后台列表缩略图
                        dic.Add(200, 150);//前台列表缩略图
                    }
                    else if (tableID == ProductPhotoBLL.TableID) {                    
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.ProductPhoto))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        if (!dic.ContainsKey(75)) dic.Add(75, 75);//后台商品图集默认使用尺寸(如果不存在则手动添加)
                    }
                    if (dic.Count > 0)
                    {
                        foreach (KeyValuePair<int, int> kv in dic)
                        {
                            //string nailStr = delTarget.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                            string nailStr = tarimg.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                           
                            FileHelper.DeleteFile(new List<string> { nailStr });//删除原有缩略图
                            //makeFile = nailStr;
                            makeFile = delTarget.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                            ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(delTarget), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.InBox);
                        }
                    }
                //}
                //else
                //{

                //    Hashtable ht = new Hashtable();
                //    if (tableID == ProductPhotoBLL.TableID)
                //    {
                //        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.ProductPhoto))
                //        {
                //            ht.Add(phototype.Width, phototype.Height);
                //        }
                //        if (!ht.ContainsKey(75)) ht.Add(75, 75);//后台商品图集默认使用尺寸(如果不存在则手动添加)
                //    }
                //    else
                //    {
                //        ht.Add("75", "75");
                //        ht.Add("350", "350");
                //    }
                //    foreach (DictionaryEntry de in ht)
                //    {
                //        string nailStr = delTarget.Replace("Original", de.Key + "-" + de.Value);
                //        FileHelper.DeleteFile(new List<string> { nailStr });//删除原有缩略图
                //        makeFile = nailStr;
                //        ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(delTarget), ServerHelper.MapPath(makeFile), Convert.ToInt32(de.Key), Convert.ToInt32(de.Value), ThumbnailType.InBox);
                //    }
                //}
            }
            //parent.layer.close(index);
            string strp_img2 = saveFilePath.Substring(saveFilePath.LastIndexOf('/'), saveFilePath.Length - saveFilePath.LastIndexOf('/'));
            if (targetType == 1)//产品多图
            {
                if (ProductID <= 0)
                {//添加
                    //Response.Write("<script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name); parent.$('#" + targetID + ">img').attr('src','" + delTarget + "'); parent.layer.close(index);</script>");
                    string _str = "<script type='text/javascript' src='/Admin/Js/jquery-1.7.2.min.js'></script><script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.editProductPhoto('" + targetID + "', '" + delTarget + "', '" + ProductPhotoID + "');parent.layer.close(index);</script>";
                    //Response.Write("<script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.o('" + targetID + "').firstChild.src='" + delTarget + "';window.parent.o('" + targetID + "').getElementsByName('ProductPhoto')[0].value='" + _name + "|" + delTarget + "' ; parent.layer.close(index);</script>");
                    Response.Write("<script type='text/javascript' src='/Admin/Js/jquery-1.7.2.min.js'></script><script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.editProductPhoto('" + targetID + "', '" + delTarget + "', '" + ProductPhotoID + "');parent.layer.close(index);</script>");
                }
                else
                {
                    ProductPhotoInfo productPhoto = ProductPhotoBLL.Read(ProductPhotoID,ProStyle);
                    productPhoto.ImageUrl = delTarget;
                    ProductPhotoBLL.Update(productPhoto);
                    string _str = "<script type='text/javascript' src='/Admin/Js/jquery-1.7.2.min.js'></script><script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.editProductPhoto('" + targetID + "', '" + delTarget + "', '" + ProductPhotoID + "');parent.layer.close(index);</script>";

                    //Response.Write("<script type='text/javascript' src='/Admin/Js/jquery-1.7.2.min.js'></script><script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.o('" + targetID + "').firstChild.src='" + delTarget + "';$('#" + targetID + "').find('.cut').attr('href','javascript:loadCut(\"" + delTarget + "\",\"" + targetID + "\",\"" + ProductPhotoID + "\")');parent.layer.close(index);</script>");
                    Response.Write("<script type='text/javascript' src='/Admin/Js/jquery-1.7.2.min.js'></script><script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.editProductPhoto('" + targetID + "', '" + delTarget + "', '" + ProductPhotoID + "');parent.layer.close(index);</script>");
                    
                }
                
            }
            else
            {
                Response.Write("<script>alert('裁剪成功');var index = parent.layer.getFrameIndex(window.name);window.parent.o('" + targetID + "').value='" + delTarget + "';window.parent.o('firstPhoto').src='" + delTarget + "';parent.layer.close(index); </script>");
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {           
            string sFromFilePath, saveFilePath;
            int width, height, spaceX, spaceY;

            sFromFilePath = Server.MapPath("/") + himg.Value;
            string[] tarimg = himg.Value.Split('.');

            //saveFilePath = Server.MapPath("/") + tarimg[0] + "nail." + tarimg[1];
            saveFilePath = Server.MapPath("/") + tarimg[0].Substring(0, tarimg[0].LastIndexOf('/') + 1) +Guid.NewGuid().ToString() + "." + tarimg[1];
            
            width = Convert.ToInt32(Math.Round(Convert.ToDecimal(Request["w"])));
            height = Convert.ToInt32(Math.Round(Convert.ToDecimal(Request["h"])));

            spaceX = Convert.ToInt32(Math.Round(Convert.ToDecimal(Request["x1"]))); 
            spaceY = Convert.ToInt32(Math.Round(Convert.ToDecimal(Request["y1"])));

            if (width > 0 && height > 0)
            {
                //CaptureImage(sFromFilePath, saveFilePath, width, height, spaceX, spaceY, tarimg[0] + "nail." + tarimg[1], targetID);
                CaptureImage(sFromFilePath, saveFilePath, width, height, spaceX, spaceY, himg.Value, targetID);
            }
            else
            {
                ScriptHelper.Alert("请选择裁剪区域，如图片过小建议不进行裁剪", RequestHelper.RawUrl);
            }
        }
    }
}