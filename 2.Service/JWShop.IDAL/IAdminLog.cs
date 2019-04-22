using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 管理员日志接口层说明。
    /// </summary>
    public interface IAdminLog
    {
        int Add(AdminLogInfo entity);
        void Update(AdminLogInfo entity);
        void Delete(int[] ids, int adminId);
        void DeleteByAdminIds(int[] adminIds);
        void DeleteByGroupIds(int[] groupIds);
        AdminLogInfo Read(int id, int adminId);
        List<AdminLogInfo> ReadList(int currentPage, int pageSize, ref int count, int adminId);
        List<AdminLogInfo> ReadList(int currentPage, int pageSize, ref int count, string logType, DateTime startAddDate, DateTime endAddDate, int adminId);

    }
}