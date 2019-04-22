using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Messaging;
using System.Diagnostics;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 邮件发送记录业务逻辑。
    /// </summary>
    public sealed class EmailSendRecordBLL
    {

        private static readonly IEmailSendRecord dal = FactoryHelper.Instance<IEmailSendRecord>(Global.DataProvider, "EmailSendRecordDAL");

        /// <summary>
        /// 增加一条邮件发送记录数据
        /// </summary>
        /// <param name="emailSendRecord">邮件发送记录模型变量</param>
        public static int AddEmailSendRecord(EmailSendRecordInfo emailSendRecord)
        {
            emailSendRecord.ID = dal.AddEmailSendRecord(emailSendRecord);
            return emailSendRecord.ID;
        }

        /// <summary>
        /// 删除多条邮件发送记录数据
        /// </summary>
        /// <param name="strID">邮件发送记录的主键值,以,号分隔</param>
        public static void DeleteEmailSendRecord(string strID)
        {
            dal.DeleteEmailSendRecord(strID);
        }


        /// <summary>
        /// 读取一条邮件发送记录数据
        /// </summary>
        /// <param name="id">邮件发送记录的主键值</param>
        /// <returns>邮件发送记录数据模型</returns>
        public static EmailSendRecordInfo ReadEmailSendRecord(int id)
        {
            return dal.ReadEmailSendRecord(id);
        }


        /// <summary>
        /// 搜索邮件发送记录数据列表
        /// </summary>
        /// <param name="emailSendRecord">EmailSendRecordSearchInfo模型变量</param>
        /// <returns>邮件发送记录数据列表</returns>
        public static List<EmailSendRecordInfo> SearchEmailSendRecordList(EmailSendRecordSearchInfo emailSendRecord)
        {
            return dal.SearchEmailSendRecordList(emailSendRecord);
        }

        /// <summary>
        /// 搜索邮件发送记录数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="emailSendRecord">EmailSendRecordSearchInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>邮件发送记录数据列表</returns>
        public static List<EmailSendRecordInfo> SearchEmailSendRecordList(int currentPage, int pageSize, EmailSendRecordSearchInfo emailSendRecord, ref int count)
        {
            return dal.SearchEmailSendRecordList(currentPage, pageSize, emailSendRecord, ref count);
        }

        /// <summary>
        ///  记录用户打开的邮件
        /// </summary>
        /// <param name="email"></param>
        public static void RecordOpenedEmailRecord(string email, int id)
        {
            dal.RecordOpenedEmailRecord(email, id);
        }

        /// <summary>
        /// 保存发送完成状态
        /// </summary>
        /// <param name="emailSendRecord">邮件发送模型变量</param>
        public static void SaveEmailSendRecordStatus(EmailSendRecordInfo emailSendRecord)
        {
            dal.SaveEmailSendRecordStatus(emailSendRecord);
        }
        /// <summary>
        /// 发送Email，保存发送状态，发送时间
        /// </summary>
        /// <param name="emailSendRecord"></param>
        public static EmailSendRecordInfo SendEmail(EmailSendRecordInfo emailSendRecord)
        {
            //发送
            foreach (string temp in emailSendRecord.EmailList.Split(','))
            {
                if (temp != string.Empty)
                {
                    MailInfo mail = new MailInfo();
                    mail.ToEmail = temp;
                    mail.Title = emailSendRecord.Title;
                    mail.Content = emailSendRecord.Content;
                    if (emailSendRecord.IsStatisticsOpendEmail == (int)BoolType.True)
                    {
                        mail.Content += "<img style=\"display:none\" src=\"http://" + HttpContext.Current.Request.ServerVariables["Http_Host"] + "/Admin/EmailCheckOpen.aspx?Email=" + temp + "&ID=" + emailSendRecord.ID + "\" >";
                    }
                    mail.UserName = ShopConfig.ReadConfigInfo().EmailUserName;
                    mail.Password = ShopConfig.ReadConfigInfo().EmailPassword;
                    mail.Server = ShopConfig.ReadConfigInfo().EmailServer;
                    mail.ServerPort = ShopConfig.ReadConfigInfo().EmailServerPort;
                    try
                    {
                        MailClass.SendEmail(mail);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.ProcessException(ex, true);
                    }
                }
            }
            //保存状态
            emailSendRecord.SendDate = RequestHelper.DateNow;
            emailSendRecord.SendStatus = (int)SendStatus.Finished;
            EmailSendRecordBLL.SaveEmailSendRecordStatus(emailSendRecord);
            return emailSendRecord;
        }
    }
}