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
    /// 管理员日志业务逻辑。
    /// </summary>
    public sealed class AdminLogBLL: BaseBLL
    {
        private static readonly IAdminLog dal = FactoryHelper.Instance<IAdminLog>(Global.DataProvider, "AdminLogDAL");

        public static void Add(string action)
        {
            Add(action, string.Empty, string.Empty);
        }
        public static void Add(string action, int id)
        {
            Add(action, string.Empty, id.ToString());
        }
        public static void Add(string action, string tableName, int id)
        {
            Add(action, tableName, id.ToString());
        }
        public static void Add(string action, string tableName, string strId)
        {
            if (action.IndexOf("$TableName") > -1)
            {
                action = action.Replace("$TableName", tableName);
            }
            if (action.IndexOf("$ID") > -1)
            {
                action = action.Replace("$ID", strId);
            }
            AdminLogInfo AdminLog = new AdminLogInfo();
            AdminLog.Action = action;
            AdminLog.AddDate = RequestHelper.DateNow;
            AdminLog.IP = ClientHelper.IP;
            AdminLog.AdminId = Cookies.Admin.GetAdminID(false);
            AdminLog.AdminName = Cookies.Admin.GetAdminName(false);
            Add(AdminLog);
        }
        public static void Add(string action, string fileName)
        {
            if (action.IndexOf("$FileName") > -1)
            {
                action = action.Replace("$FileName", fileName);
            }
            AdminLogInfo AdminLog = new AdminLogInfo();
            AdminLog.Action = action;
            AdminLog.AddDate = RequestHelper.DateNow;
            AdminLog.IP = ClientHelper.IP;
            AdminLog.AdminId = Cookies.Admin.GetAdminID(false);
            AdminLog.AdminName = Cookies.Admin.GetAdminName(false);
            Add(AdminLog);
        }
        private static void Add(AdminLogInfo entity)
        {
            dal.Add(entity);
        }

        public static void UpdateAdminLog(AdminLogInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids, int adminId)
        {
            dal.Delete(ids, adminId);
        }

        public static void DeleteByAdminIds(int[] adminIds)
        {
            dal.DeleteByAdminIds(adminIds);
        }

        public static void DeleteByGroupIds(int[] groupIds)
        {
            dal.DeleteByGroupIds(groupIds);
        }

        public static AdminLogInfo Read(int id, int adminId)
        {
            return dal.Read(id, adminId);
        }

        public static List<AdminLogInfo> ReadList(int currentPage, int pageSize, ref int count, int adminId)
        {
            return dal.ReadList(currentPage, pageSize, ref count, adminId);
        }

        public static List<AdminLogInfo> ReadList(int currentPage, int pageSize, ref int count, string logType, DateTime startAddDate, DateTime endAddDate, int adminId)
        {
            return dal.ReadList(currentPage, pageSize, ref count, logType, startAddDate, endAddDate, adminId);
        }

    }
}