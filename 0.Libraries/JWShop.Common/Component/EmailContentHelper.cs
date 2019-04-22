using System;
using System.IO;
using System.Xml;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// 邮件内容管理
    /// </summary>
    public class EmailContentHelper
    {
        private static string path = ServerHelper.MapPath("/EmailContent");
        private static string emailContentCacheKey = CacheKey.ReadCacheKey("EmailContent");
        /// <summary>
        /// 读取系统邮件内容列表
        /// </summary>
        /// <returns></returns>
        public static List<EmailContentInfo> ReadSystemEmailContentList()
        {
            if (CacheHelper.Read(emailContentCacheKey) == null)
            {
                RefreshEmailContentCache();
            }
            List<EmailContentInfo> emailContentList = (List<EmailContentInfo>)CacheHelper.Read(emailContentCacheKey);
            return emailContentList;
        }
        /// <summary>
        /// 读取普通邮件内容列表
        /// </summary>
        /// <returns></returns>
        public static List<EmailContentInfo> ReadCommonEmailContentList()
        {
            List<EmailContentInfo> emailContentList = new List<EmailContentInfo>();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (Convert.ToInt32(xh.ReadInnerText("EmailConfig/IsSystem")) == (int)BoolType.False)
                    {
                        EmailContentInfo emailContent = new EmailContentInfo();
                        emailContent.EmailTitle = xh.ReadInnerText("EmailConfig/EmailTitle");
                        emailContent.IsSystem = Convert.ToInt32(xh.ReadInnerText("EmailConfig/IsSystem"));
                        emailContent.Key = xh.ReadInnerText("EmailConfig/Key");
                        emailContent.EmailContent = xh.ReadInnerText("EmailConfig/EmailContent");
                        emailContent.Note = xh.ReadInnerText("EmailConfig/Note");
                        emailContentList.Add(emailContent);
                    }
                }
            }
            return emailContentList;
        }
        /// <summary>
        /// 读取一条系统邮件内容
        /// </summary>
        /// <returns></returns>
        public static EmailContentInfo ReadSystemEmailContent(string key)
        {
            EmailContentInfo emailContent = new EmailContentInfo();
            foreach (EmailContentInfo tempEmailContent in ReadSystemEmailContentList())
            {
                if (tempEmailContent.Key == key)
                {
                    emailContent = tempEmailContent;
                    break;
                }
            }
            return emailContent;
        }
        /// <summary>
        /// 读取一条普通邮件内容
        /// </summary>
        /// <returns></returns>
        public static EmailContentInfo ReadCommonEmailContent(string key)
        {
            EmailContentInfo emailContent = new EmailContentInfo();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (Convert.ToInt32(xh.ReadInnerText("EmailConfig/IsSystem")) == (int)BoolType.False && xh.ReadInnerText("EmailConfig/Key")==key)
                    {
                        emailContent.EmailTitle = xh.ReadInnerText("EmailConfig/EmailTitle");
                        emailContent.IsSystem = Convert.ToInt32(xh.ReadInnerText("EmailConfig/IsSystem"));
                        emailContent.Key = xh.ReadInnerText("EmailConfig/Key");
                        emailContent.EmailContent = xh.ReadInnerText("EmailConfig/EmailContent");
                        emailContent.Note = xh.ReadInnerText("EmailConfig/Note");
                        break;
                    }
                }
            }
            return emailContent;
        }
        /// <summary>
        /// 刷新系统邮件内容缓存
        /// </summary>
        /// <returns></returns>
        public static void RefreshEmailContentCache()
        {
            List<EmailContentInfo> emailContentList = new List<EmailContentInfo>();
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (Convert.ToInt32(xh.ReadInnerText("EmailConfig/IsSystem")) == (int)BoolType.True)
                    {
                        EmailContentInfo emailContent = new EmailContentInfo();
                        emailContent.EmailTitle = xh.ReadInnerText("EmailConfig/EmailTitle");
                        emailContent.IsSystem = Convert.ToInt32(xh.ReadInnerText("EmailConfig/IsSystem"));
                        emailContent.Key = xh.ReadInnerText("EmailConfig/Key");
                        emailContent.EmailContent = xh.ReadInnerText("EmailConfig/EmailContent");
                        emailContent.Note = xh.ReadInnerText("EmailConfig/Note");
                        emailContentList.Add(emailContent);
                    }
                }
            }
            CacheHelper.Write(emailContentCacheKey, emailContentList);
        }
        /// <summary>
        /// 修改一条邮件内容的信息
        /// </summary>
        /// <param name="emailContent"></param>
        public static void UpdateEmailContent(EmailContentInfo emailContent)
        {
            List<FileInfo> fileList = FileHelper.ListDirectory(path, "|.config|");
            foreach (FileInfo file in fileList)
            {
                using (XmlHelper xh = new XmlHelper(file.FullName))
                {
                    if (xh.ReadInnerText("EmailConfig/Key") == emailContent.Key)
                    {
                        xh.UpdateInnerText("EmailConfig/EmailTitle", emailContent.EmailTitle);
                        xh.UpdateInnerText("EmailConfig/EmailContent", emailContent.EmailContent);
                        xh.Save();
                        break;
                    }
                }
            }
            if (emailContent.IsSystem == (int)BoolType.True)
            {
                RefreshEmailContentCache();
            }
        }
        /// <summary>
        /// 添加一条邮件内容的信息
        /// </summary>
        /// <param name="emailContent"></param>
        public static void AddEmailContent(EmailContentInfo emailContent)
        {
           XmlDocument xd=new XmlDocument();
            xd.Load(ServerHelper.MapPath("/EmailContent/Template.config"));
            xd.SelectSingleNode("EmailConfig/EmailTitle").InnerText = emailContent.EmailTitle;
            xd.SelectSingleNode("EmailConfig/IsSystem").InnerText = ((int)BoolType.False).ToString();
            xd.SelectSingleNode("EmailConfig/Key").InnerText = emailContent.Key;
            xd.SelectSingleNode("EmailConfig/EmailContent").InnerText = emailContent.EmailContent;
            xd.Save(ServerHelper.MapPath("/EmailContent/"+emailContent.Key+".config"));
        }
        /// <summary>
        /// 删除一条邮件内容的信息
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteEmailContent(string key)
        {
            File.Delete(ServerHelper.MapPath("/EmailContent/" + key + ".config"));
        }
    }
}
