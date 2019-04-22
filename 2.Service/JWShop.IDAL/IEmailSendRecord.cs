using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 邮件发送记录接口层说明。
    /// </summary>
    public interface IEmailSendRecord
    {
        /// <summary>
        /// 增加一条邮件发送记录数据
        /// </summary>
        /// <param name="emailSendRecord">邮件发送记录模型变量</param>
        int AddEmailSendRecord(EmailSendRecordInfo emailSendRecord);

        /// <summary>
        /// 删除多条邮件发送记录数据
        /// </summary>
        /// <param name="strID">邮件发送记录的主键值,以,号分隔</param>
        void DeleteEmailSendRecord(string strID);


        /// <summary>
        /// 读取一条邮件发送记录数据
        /// </summary>
        /// <param name="id">邮件发送记录的主键值</param>
        /// <returns>邮件发送记录数据模型</returns>
        EmailSendRecordInfo ReadEmailSendRecord(int id);


        /// <summary>
        /// 搜索邮件发送记录数据列表
        /// </summary>
        /// <param name="emailSendRecord">EmailSendRecordSearchInfo模型变量</param>
        /// <returns>邮件发送记录数据列表</returns>
        List<EmailSendRecordInfo> SearchEmailSendRecordList(EmailSendRecordSearchInfo emailSendRecord);

        /// <summary>
        /// 搜索邮件发送记录数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="emailSendRecord">EmailSendRecordSearchInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>邮件发送记录数据列表</returns>
        List<EmailSendRecordInfo> SearchEmailSendRecordList(int currentPage, int pageSize, EmailSendRecordSearchInfo emailSendRecord, ref int count);


        /// <summary>
        ///  记录用户打开的邮件
        /// </summary>
        /// <param name="email"></param>
        void RecordOpenedEmailRecord(string email, int id);

        /// <summary>
        /// 保存发送完成状态
        /// </summary>
        /// <param name="emailSendRecord">邮件发送模型变量</param>
        void SaveEmailSendRecordStatus(EmailSendRecordInfo emailSendRecord);
	}
}