using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 管理员接口层说明。
    /// </summary>
    public interface IAdmin
    {
        int Add(AdminInfo entity);
        void Update(AdminInfo entity);
        void Delete(int[] ids);
        void DeleteByGroupIds(int[] groupIds);
        AdminInfo Read(int id);
        AdminInfo Read(string name);
        List<AdminInfo> ReadList(int currentPage, int pageSize, ref int count);
        List<AdminInfo> ReadList(int groupId, int currentPage, int pageSize, ref int count);

        void ChangePassword(int id, string newPassword);
        void ChangePassword(int id, string oldPassword, string newPassword);

        void UpdateLogin(int id, DateTime lastLoginDate, string lastLoginIP);
        void UpdateLogin(string name, DateTime lastLoginDate, string lastLoginIP, int maxErrorTimes);
        AdminInfo CheckLogin(string loginName, string loginPass);
        /// <summary>
        /// 更改用户的的安全码（找回密码使用）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="safeCode"></param>
        /// <param name="findDate"></param>
        void ChangeAdminSafeCode(int adminID, string safeCode, DateTime findDate);

        /// <summary>
        /// 管理员状态解锁，错误次数清零
        /// </summary>
        /// <param name="id"></param>
        void UpdateStatus(int id);

        /// <summary>
        /// 检查Email唯一性,true:唯一
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UniqueEmail(string email, int id = 0);

    }
}