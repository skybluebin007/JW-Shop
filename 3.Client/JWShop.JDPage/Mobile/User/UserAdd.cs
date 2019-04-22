using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Text.RegularExpressions;

namespace JWShop.Page.Mobile
{
    public class UserAdd : UserBasePage
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 单一无限极分类
        /// </summary>
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();

            user = UserBLL.Read(base.UserId);
            //userGradeName = UserGradeBLL.Read(base.GradeID).Name;
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.ClassID = user.RegionId;
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            UserInfo user = UserBLL.Read(base.UserId);

            if (StringHelper.AddSafe(RequestHelper.GetForm<string>("file_code")) == "1")
            {
                string userPhoto = UploadUserPhoto();
                if (userPhoto != string.Empty)
                {
                    user.Photo = userPhoto;
                    CookiesHelper.AddCookie("UserPhoto", userPhoto);
                }
            }
            else
            {
                //user.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
                user.Sex = RequestHelper.GetForm<int>("Sex");
                user.Birthday = StringHelper.AddSafe(RequestHelper.GetForm<string>("Birthday"));
                user.MSN = StringHelper.AddSafe(RequestHelper.GetForm<string>("MSN"));
                user.QQ = StringHelper.AddSafe(RequestHelper.GetForm<string>("QQ"));
                user.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
                //user.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
                user.RegionId = singleUnlimitClass.ClassID;
                user.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("Address"));
                user.Introduce = StringHelper.AddSafe(RequestHelper.GetForm<string>("Introduce"));
                //CookiesHelper.AddCookie("UserEmail", user.Email);
            }
             Regex reg = new Regex("^[1-9]\\d{4,12}$");
            if (!string.IsNullOrEmpty(user.QQ) && !reg.IsMatch(user.QQ))
            { ScriptHelper.AlertFront("QQ号输入错误", RequestHelper.RawUrl); }
            else if (!string.IsNullOrEmpty(user.Tel) && !new Regex("^(([\\d]{3,4}-?)?[\\d]{7,8})$").IsMatch(user.Tel))
            { ScriptHelper.AlertFront("固定电话输入错误", RequestHelper.RawUrl); }
            else
            {
                UserBLL.Update(user);
                ScriptHelper.AlertFront("修改成功", RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <returns></returns>
        protected string UploadUserPhoto()
        {
            string originalFile = string.Empty;
            if (HttpContext.Current.Request.Files[0].FileName != string.Empty)
            {
                try
                {
                    //上传文件
                    UploadHelper upload = new UploadHelper();
                    upload.Path = "/Upload/UserPhoto/Original/";
                    upload.FileType = ShopConfig.ReadConfigInfo().UploadFile;
                    FileInfo file = upload.SaveAs();
                    //生成处理
                    originalFile = upload.Path + file.Name;
                    string otherFile = string.Empty;
                    string makeFile = string.Empty;
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    dic.Add(70, 70);
                    dic.Add(190, 190);
                    foreach (KeyValuePair<int, int> kv in dic)
                    {
                        makeFile = originalFile.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                        otherFile += makeFile + "|";
                        ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.InBox);
                    }
                    otherFile = otherFile.Substring(0, otherFile.Length - 1);
                    //保存数据库
                    UploadInfo tempUpload = new UploadInfo();
                    tempUpload.TableID = UserBLL.TableID;
                    tempUpload.ClassID = 0;
                    tempUpload.RecordID = 0;
                    tempUpload.UploadName = originalFile;
                    tempUpload.OtherFile = otherFile;
                    tempUpload.Size = Convert.ToInt32(file.Length);
                    tempUpload.FileType = file.Extension;
                    tempUpload.Date = RequestHelper.DateNow;
                    tempUpload.IP = ClientHelper.IP;
                    UploadBLL.AddUpload(tempUpload);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ProcessException(ex, false);
                }
            }
            return originalFile;
        }
    }
}