using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IUser
    {
        int Add(UserInfo entity);
        void Update(UserInfo entity);
        void Delete(int id);
        UserInfo Read(int id);
        UserInfo Read(string loginName);
        UserInfo Read(string loginName, string password);
        List<UserInfo> ReadList();
        List<UserInfo> ReadList(UserType userType);
        List<UserInfo> SearchList(UserSearchInfo searchInfo);
        List<UserInfo> SearchList(int currentPage, int pageSize, UserSearchInfo searchModel, ref int count);
        /// <summary>
        /// 获取userlist 包括usergrade
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        List<UserInfo> SearchListAndUserGrade(UserSearchInfo searchInfo);
        List<UserInfo> SearchListAndUserGrade(int currentPage, int pageSize, UserSearchInfo searchModel, ref int count);
        bool UniqueUser(string loginName, int usrId = 0);
        void ChangePassword(int id, string oldPassword, string newPassword);
        void ChangePassword(int id, string newPassword);
        /// <summary>
        /// 供应商数量
        /// </summary>
        int CountProvider();

        /// <summary>
        /// 用户中心首页统计
        /// </summary>
        DataTable UserIndexStatistics(int userId);
        /// <summary>
        /// 供应商用户中心首页统计
        /// </summary>
        DataTable ShopIndexStatistics(int shopId);
        /// <summary>
        /// 用户活跃度分析
        /// </summary>
        DataTable StatisticsUserActive(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField);
        /// <summary>
        /// 用户消费分析
        /// </summary>
        DataTable StatisticsUserConsume(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField, DateTime startDate, DateTime endDate);
        /// <summary>
        /// 统计用户状态
        /// </summary>
        DataTable StatisticsUserStatus(UserSearchInfo userSearch);
        /// <summary>
        /// 统计用户数量
        /// </summary>
        DataTable StatisticsUserCount(UserSearchInfo userSearch, DateType dateType);

        /// <summary>
        /// 读取一条用户数据(包含用户的统计信息)
        /// </summary>
        /// <param name="id">用户的主键值</param>
        /// <returns>用户数据模型</returns>
        UserInfo ReadUserMore(int id);

        /// <summary>
        /// 检查用户名是否被占用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        int CheckUserName(string userName);

        /// <summary>
        /// 检查E-mail是否被占用
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>真/假</returns>
        bool CheckEmail(string email);
        /// <summary>
        /// 检查E-mail是否被占用
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>真/假</returns>
        bool CheckEmail(string email, int userId);
        /// <summary>
        /// 检查Mobile是否被占用
        /// </summary>
        /// <param name="email">email</param>
        ///  /// <param name="userId">userId</param>
        /// <returns>真/假</returns>
        bool CheckMobile(string mobile, int userId);
        /// <summary>
        /// 更改用户的的安全码（找回密码使用）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="safeCode"></param>
        /// <param name="findDate"></param>
        void ChangeUserSafeCode(int userID, string safeCode, DateTime findDate);

       // 根据userNames[]集合检查是否有重复 true:无重复 false:有重复
       bool CheckUserNames(string[] userNames);
       
        // 根据mobiles[]集合检查是否有重复 true:无重复 false:有重复
        bool CheckMobiles(string[] mobiles);
        
        // 根据emails[]集合检查是否有重复 true:无重复 false:有重复
         bool CheckEmails(string[] emails);
           //批量添加
         void AddBatch(List<object[]> entities);
        /// <summary>
        /// 用户归属分销商
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="distributorId">分销商Id</param>
        void ChangeUserToDistributor(int userId, int distributorId);
    }
}