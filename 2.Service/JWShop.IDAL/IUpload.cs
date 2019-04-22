using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 上传接口层说明。
    /// </summary>
    public interface IUpload
    {
        /// <summary>
        /// 增加一条上传数据
        /// </summary>
        /// <param name="upload">上传模型变量</param>
        void AddUpload(UploadInfo upload);

        /// <summary>
        /// 更新多条上传数据
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="classID">分类ID</param>
        /// <param name="recordID">记录ID</param>
        /// <param name="randomNumber">随机数字</param>
        void UpdateUpload(int tableID, int classID, int recordID, string randomNumber);

        /// <summary>
        /// 根据RecordID删除上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strRecordID</param>
        void DeleteUploadByRecordID(int tableID, string strRecordID);

        /// <summary>
        /// 根据RecordID读取上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strRecordID</param>
        /// <returns>上传上传数据</returns>
        List<UploadInfo> ReadUploadByRecordID(int tableID, string strRecordID);

        /// <summary>
        /// 根据ClassID删除上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strClassID</param>
        void DeleteUploadByClassID(int tableID, string strClassID);

        /// <summary>
        /// 根据ClassID读取上传上传文件
        /// </summary>
        /// <param name="tableID">表ID</param>
        /// <param name="strRecordID">strClassID</param>
        /// <returns>上传上传数据</returns>
        List<UploadInfo> ReadUploadByClassID(int tableID, string strClassID);

    }
}