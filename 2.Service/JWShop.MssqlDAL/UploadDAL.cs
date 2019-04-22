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
    /// 上传数据层说明。
    /// </summary>
    public sealed class UploadDAL : IUpload
    {

        /// <summary>
        /// 增加一条上传数据
        /// </summary>
        /// <param name="upload">上传模型变量</param>
        public void AddUpload(UploadInfo upload)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@tableID",SqlDbType.Int),
				new SqlParameter("@classID",SqlDbType.Int),
				new SqlParameter("@recordID",SqlDbType.Int),
				new SqlParameter("@uploadName",SqlDbType.NVarChar),
				new SqlParameter("@otherFile",SqlDbType.NVarChar),
				new SqlParameter("@size",SqlDbType.Int),
				new SqlParameter("@fileType",SqlDbType.NVarChar),
				new SqlParameter("@randomNumber",SqlDbType.NVarChar),
				new SqlParameter("@date",SqlDbType.DateTime),
				new SqlParameter("@iP",SqlDbType.NVarChar)
			};
            parameters[0].Value = upload.TableID;
            parameters[1].Value = upload.ClassID;
            parameters[2].Value = upload.RecordID;
            parameters[3].Value = upload.UploadName;
            parameters[4].Value = upload.OtherFile;
            parameters[5].Value = upload.Size;
            parameters[6].Value = upload.FileType;
            parameters[7].Value = upload.RandomNumber;
            parameters[8].Value = upload.Date;
            parameters[9].Value = upload.IP;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "AddUpload", parameters);
        }

        /// <summary>
        /// 更新多条上传数据
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="classID">分类ID</param>
        /// <param name="recordID">记录ID</param>
        /// <param name="randomNumber">随机数字</param>
        public void UpdateUpload(int tableID, int classID, int recordID, string randomNumber)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@tableID",SqlDbType.Int),
				new SqlParameter("@classID",SqlDbType.Int),
				new SqlParameter("@recordID",SqlDbType.Int),
				new SqlParameter("@randomNumber",SqlDbType.NVarChar)
			};
            parameters[0].Value = tableID;
            parameters[1].Value = classID;
            parameters[2].Value = recordID;
            parameters[3].Value = randomNumber;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateUpload", parameters);
        }

        /// <summary>
        /// 准备上传模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="uploadList">上传的数据列表</param>
        public void PrepareUploadModel(SqlDataReader dr, List<UploadInfo> uploadList)
        {
            while (dr.Read())
            {
                UploadInfo upload = new UploadInfo();
                upload.ID = dr.GetInt32(0);
                upload.TableID = dr.GetInt32(1);
                upload.ClassID = dr.GetInt32(2);
                upload.RecordID = dr.GetInt32(3);
                upload.UploadName = dr[4].ToString();
                upload.OtherFile = dr[5].ToString();
                upload.Size = dr.GetInt32(6);
                upload.FileType = dr[7].ToString();
                upload.RandomNumber = dr[8].ToString();
                upload.Date = dr.GetDateTime(9);
                upload.IP = dr[10].ToString();
                uploadList.Add(upload);
            }
        }

        /// <summary>
        /// 根据RecordID删除上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strRecordID</param>
        public void DeleteUploadByRecordID(int tableID, string strRecordID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@tableID",SqlDbType.Int),
				new SqlParameter("@strRecordID",SqlDbType.NVarChar)		
			};
            parameters[0].Value = tableID;
            parameters[1].Value = strRecordID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteUploadByRecordID", parameters);
        }

        /// <summary>
        /// 根据RecordID读取上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strRecordID</param>
        /// <returns>上传上传数据</returns>	
        public List<UploadInfo> ReadUploadByRecordID(int tableID, string strRecordID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@tableID",SqlDbType.Int),
				new SqlParameter("@strRecordID",SqlDbType.NVarChar)			
			};
            parameters[0].Value = tableID;
            parameters[1].Value = strRecordID;
            List<UploadInfo> uploadList = new List<UploadInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadUploadByRecordID", parameters))
            {
                PrepareUploadModel(dr, uploadList);
            }
            return uploadList;
        }

        /// <summary>
        /// 根据ClassID删除上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strClassID">strClassID</param>
        public void DeleteUploadByClassID(int tableID, string strClassID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@tableID",SqlDbType.Int),
				new SqlParameter("@strClassID",SqlDbType.NVarChar)		
			};
            parameters[0].Value = tableID;
            parameters[1].Value = strClassID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteUploadByClassID", parameters);
        }

        /// <summary>
        /// 根据ClassID读取上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strClassID">strClassID</param>
        /// <returns>上传上传数据</returns>	
        public List<UploadInfo> ReadUploadByClassID(int tableID, string strClassID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@tableID",SqlDbType.Int),
				new SqlParameter("@strClassID",SqlDbType.NVarChar)			
			};
            parameters[0].Value = tableID;
            parameters[1].Value = strClassID;
            List<UploadInfo> uploadList = new List<UploadInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadUploadByClassID", parameters))
            {
                PrepareUploadModel(dr, uploadList);
            }
            return uploadList;
        }
    }
}