using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 邮件发送记录数据层说明。
    /// </summary>
    public sealed class EmailSendRecordDAL : IEmailSendRecord
    {

      
		/// <summary>
		/// 增加一条邮件发送记录数据
		/// </summary>
		/// <param name="emailSendRecord">邮件发送记录模型变量</param>
		public int AddEmailSendRecord(EmailSendRecordInfo emailSendRecord)
		{
            SqlParameter[] parameters ={
				new SqlParameter("@title",SqlDbType.NVarChar),
				new SqlParameter("@content",SqlDbType.NText),
				new SqlParameter("@isSystem",SqlDbType.Int),
				new SqlParameter("@emailList",SqlDbType.NText),
				new SqlParameter("@openEmailList",SqlDbType.NText),
				new SqlParameter("@isStatisticsOpendEmail",SqlDbType.Int),
				new SqlParameter("@sendStatus",SqlDbType.Int),
				new SqlParameter("@note",SqlDbType.NVarChar),
				new SqlParameter("@addDate",SqlDbType.DateTime),
				new SqlParameter("@sendDate",SqlDbType.DateTime)
			};
            parameters[0].Value = emailSendRecord.Title;
            parameters[1].Value = emailSendRecord.Content;
            parameters[2].Value = emailSendRecord.IsSystem;
            parameters[3].Value = emailSendRecord.EmailList;
            parameters[4].Value = emailSendRecord.OpenEmailList;
            parameters[5].Value = emailSendRecord.IsStatisticsOpendEmail;
            parameters[6].Value = emailSendRecord.SendStatus;
            parameters[7].Value = emailSendRecord.Note;
            parameters[8].Value = emailSendRecord.AddDate;
            parameters[9].Value = emailSendRecord.SendDate;
            Object id = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddEmailSendRecord", parameters);
            return (Convert.ToInt32(id));
        }


        /// <summary>
        /// 删除多条邮件发送记录数据
        /// </summary>
        /// <param name="strID">邮件发送记录的主键值,以,号分隔</param>
        public void DeleteEmailSendRecord(string strID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar)
			};
            parameters[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteEmailSendRecord", parameters);
        }





        /// <summary>
        /// 读取一条邮件发送记录数据
        /// </summary>
        /// <param name="id">邮件发送记录的主键值</param>
        /// <returns>邮件发送记录数据模型</returns>
        public EmailSendRecordInfo ReadEmailSendRecord(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id", SqlDbType.Int)
			};
            parameters[0].Value = id;
            EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadEmailSendRecord", parameters))
            {
                if (dr.Read())
                {
                    emailSendRecord.ID = dr.GetInt32(0);
                    emailSendRecord.Title = dr[1].ToString();
                    emailSendRecord.Content = dr[2].ToString();
                    emailSendRecord.IsSystem = dr.GetInt32(3);
                    emailSendRecord.EmailList = dr[4].ToString();
                    emailSendRecord.OpenEmailList = dr[5].ToString();
                    emailSendRecord.IsStatisticsOpendEmail = dr.GetInt32(6);
                    emailSendRecord.SendStatus = dr.GetInt32(7);
                    emailSendRecord.Note = dr[8].ToString();
                    emailSendRecord.AddDate = dr.GetDateTime(9);
                    emailSendRecord.SendDate = dr.GetDateTime(10);
                }
            }
            return emailSendRecord;
        }

        /// <summary>
        /// 准备邮件发送记录模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="emailSendRecordList">邮件发送记录的数据列表</param>
        public void PrepareEmailSendRecordModel(SqlDataReader dr, List<EmailSendRecordInfo> emailSendRecordList)
        {
            while (dr.Read())
            {
                EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                emailSendRecord.ID = dr.GetInt32(0);
                emailSendRecord.Title = dr[1].ToString();
                emailSendRecord.Content = dr[2].ToString();
                emailSendRecord.IsSystem = dr.GetInt32(3);
                emailSendRecord.EmailList = dr[4].ToString();
                emailSendRecord.OpenEmailList = dr[5].ToString();
                emailSendRecord.IsStatisticsOpendEmail = dr.GetInt32(6);
                emailSendRecord.SendStatus = dr.GetInt32(7);
                emailSendRecord.Note = dr[8].ToString();
                emailSendRecord.AddDate = dr.GetDateTime(9);
                emailSendRecord.SendDate = dr.GetDateTime(10);
                emailSendRecordList.Add(emailSendRecord);
            }
        }


        /// <summary>
        /// 搜索邮件发送记录数据列表
        /// </summary>
        /// <param name="emailSendRecordSearch">EmailSendRecordSearchInfo模型变量</param>
        /// <returns>邮件发送记录数据列表</returns>
        public List<EmailSendRecordInfo> SearchEmailSendRecordList(EmailSendRecordSearchInfo emailSendRecordSearch)
        {
            string condition = PrepareCondition(emailSendRecordSearch).ToString();
            List<EmailSendRecordInfo> emailSendRecordList = new List<EmailSendRecordInfo>();
            SqlParameter[] parameters ={
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = condition;
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchEmailSendRecordList", parameters))
            {
                PrepareEmailSendRecordModel(dr, emailSendRecordList);
            }
            return emailSendRecordList;
        }

        /// <summary>
        /// 搜索邮件发送记录数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="emailSendRecordSearch">EmailSendRecordSearchInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>邮件发送记录数据列表</returns>
        public List<EmailSendRecordInfo> SearchEmailSendRecordList(int currentPage, int pageSize, EmailSendRecordSearchInfo emailSendRecordSearch, ref int count)
        {
            List<EmailSendRecordInfo> emailSendRecordList = new List<EmailSendRecordInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = ShopMssqlHelper.TablePrefix + "EmailSendRecord";
            pc.Fields = "[ID],[Title],[Content],[IsSystem],[EmailList],[OpenEmailList],[IsStatisticsOpendEmail],[SendStatus],[Note],[AddDate],[SendDate]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[ID]";
            pc.OrderType = OrderType.Desc;
            pc.MssqlCondition = PrepareCondition(emailSendRecordSearch);
            pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareEmailSendRecordModel(dr, emailSendRecordList);
            }
            return emailSendRecordList;
        }


        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="emailSendRecordSearch">EmailSendRecordSearchInfo模型变量</param>
        public MssqlCondition PrepareCondition(EmailSendRecordSearchInfo emailSendRecordSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[IsSystem]", emailSendRecordSearch.IsSystem, ConditionType.Equal);

            return mssqlCondition;
        }

        /// <summary>
        ///  记录用户打开的邮件
        /// </summary>
        /// <param name="email"></param>
        public void RecordOpenedEmailRecord(string email, int id)
        {
            SqlParameter[] parameters ={
				    new SqlParameter("@email",SqlDbType.NVarChar),
                	new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = email;
            parameters[1].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "RecordOpenedEmailRecord", parameters);
        }

        /// <summary>
        /// 保存发送完成状态
        /// </summary>
        /// <param name="emailSendRecord">邮件发送模型变量</param>
        public void SaveEmailSendRecordStatus(EmailSendRecordInfo emailSendRecord)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
                new SqlParameter("@sendStatus",SqlDbType.Int),
				new SqlParameter("@sendDate",SqlDbType.DateTime),
			};
            parameters[0].Value = emailSendRecord.ID;
            parameters[1].Value = emailSendRecord.SendStatus;
            parameters[2].Value = emailSendRecord.SendDate;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "SaveEmailSendRecordStatus", parameters);
        }

	}
}