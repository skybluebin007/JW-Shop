using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 上传业务逻辑。
    /// </summary>
    public sealed class UploadBLL
    {
        private static readonly IUpload dal = FactoryHelper.Instance<IUpload>(Global.DataProvider, "UploadDAL");

        /// <summary>
        /// 增加一条上传数据
        /// </summary>
        /// <param name="upload">上传模型变量</param>
        public static void AddUpload(UploadInfo upload)
        {
            dal.AddUpload(upload);
        }

        /// <summary>
        /// 更新多条上传数据
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="classID">分类ID</param>
        /// <param name="recordID">记录ID</param>
        /// <param name="randomNumber">随机数字</param>
        public static void UpdateUpload(int tableID, int classID, int recordID, string randomNumber)
        {
            dal.UpdateUpload(tableID, classID, recordID, randomNumber);
        }

        /// <summary>
        /// 根据RecordID删除上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strRecordID</param>
        public static void DeleteUploadByRecordID(int tableID, string strRecordID)
        {
            FileHelper.DeleteFile(ReadUploadByRecordID(tableID, strRecordID));
            dal.DeleteUploadByRecordID(tableID, strRecordID);
        }

        /// <summary>
        /// 根据RecordID读取上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strRecordID</param>
        /// <returns>上传上传数据</returns>
        public static List<string> ReadUploadByRecordID(int tableID, string strRecordID)
        {
            List<string> fileNameList = new List<string>();
            List<UploadInfo> uploadList = dal.ReadUploadByRecordID(tableID, strRecordID);
            foreach (UploadInfo upload in uploadList)
            {
                fileNameList.Add(upload.UploadName);
                if (upload.OtherFile != string.Empty)
                {
                    foreach (string temp in upload.OtherFile.Split('|'))
                    {
                        fileNameList.Add(temp);
                    }
                }
            }
            return fileNameList;
        }

        /// <summary>
        /// 根据ClassID删除上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strClassID">strClassID</param>
        public static void DeleteUploadByClassID(int tableID, string strClassID)
        {
            FileHelper.DeleteFile(ReadUploadByClassID(tableID, strClassID));
            dal.DeleteUploadByClassID(tableID, strClassID);
        }

        /// <summary>
        /// 根据ClassID读取上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strClassID">strClassID</param>
        /// <returns>上传上传数据</returns>
        public static List<string> ReadUploadByClassID(int tableID, string strClassID)
        {
            List<string> fileNameList = new List<string>();
            List<UploadInfo> uploadList = dal.ReadUploadByClassID(tableID, strClassID);
            foreach (UploadInfo upload in uploadList)
            {
                fileNameList.Add(upload.UploadName);
                if (upload.OtherFile != string.Empty)
                {
                    foreach (string temp in upload.OtherFile.Split('|'))
                    {
                        fileNameList.Add(temp);
                    }
                }
            }
            return fileNameList;
        }
    }
}