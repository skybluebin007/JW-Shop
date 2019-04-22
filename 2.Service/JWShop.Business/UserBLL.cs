using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    public sealed class UserBLL : BaseBLL
    {
        private static readonly IUser dal = FactoryHelper.Instance<IUser>(Global.DataProvider, "UserDAL");
        //public static readonly int TableID = UploadTable.ReadTableID("User");
        public static readonly int TableID = 8;

        public static int Add(UserInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(UserInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static UserInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static UserInfo Read(string loginName)
        {
            return dal.Read(loginName);
        }

        public static UserInfo Read(string loginName, string password)
        {
            return dal.Read(loginName, password);
        }

        public static List<UserInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<UserInfo> ReadList(UserType userType)
        {
            return dal.ReadList(userType);
        }

        public static List<UserInfo> SearchList(UserSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }
        public static List<UserInfo> SearchList(int currentPage, int pageSize, UserSearchInfo searchModel, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchModel, ref count);
        }
    
        /// <summary>
        /// 获取userlist 包括usergrade
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public static List<UserInfo> SearchListAndUserGrade(UserSearchInfo searchInfo)
        {
            return dal.SearchListAndUserGrade(searchInfo);
        }
        public static List<UserInfo> SearchListAndUserGrade(int currentPage, int pageSize, UserSearchInfo searchModel, ref int count)
        {
            return dal.SearchListAndUserGrade(currentPage, pageSize, searchModel, ref count);
        }
        /// <summary>
        /// 验证用户唯一性 true :唯一  false：不唯一
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="usrId"></param>
        /// <returns></returns>
        public static bool UniqueUser(string loginName, int usrId = 0)
        {
            return dal.UniqueUser(loginName, usrId);
        }

        public static void ChangePassword(int id, string oldPassword, string newPassword)
        {
            dal.ChangePassword(id, oldPassword, newPassword);
        }

        public static void ChangePassword(int id, string newPassword)
        {
            dal.ChangePassword(id, newPassword);
        }

        public static int CountProvider()
        {
            return dal.CountProvider();
        }

        public static void UserLoginInit(UserInfo usr)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("LastLoginDate", RequestHelper.DateNow);
            dict.Add("LastLoginIP", ClientHelper.IP);
            dict.Add("LoginTimes", usr.LoginTimes + 1);
            UserBLL.UpdatePart(UserInfo.TABLENAME, dict, usr.Id);

            AddUserCookieWeekly(usr);
            CartBLL.CookiesImportDataBase(usr.Id, usr.UserName);
        }
/// <summary>
/// 不设过期时间，关闭浏览器清除
/// </summary>
/// <param name="usr"></param>
        public static void AddUserCookie(UserInfo usr)
        {
            int gradeID = UserGradeBLL.ReadByMoney(usr.MoneyUsed).Id;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(usr.Id.ToString() + HttpContext.Current.Server.UrlEncode(usr.UserName) + gradeID.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
            string value = sign + "|" + usr.Id.ToString() + "|" + HttpContext.Current.Server.UrlEncode(usr.UserName) + "|" + gradeID;
            CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().UserCookies, value);
            CookiesHelper.AddCookie("UserPhoto", usr.Photo);
            CookiesHelper.AddCookie("UserEmail", usr.Email);
        }
        /// <summary>
        /// 设置一周的过期时间
        /// </summary>
        /// <param name="usr"></param>
        public static void AddUserCookieWeekly(UserInfo usr)
        {
            int gradeID = UserGradeBLL.ReadByMoney(usr.MoneyUsed).Id;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(usr.Id.ToString() + HttpContext.Current.Server.UrlEncode(usr.UserName) + gradeID.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
            string value = sign + "|" + usr.Id.ToString() + "|" + HttpContext.Current.Server.UrlEncode(usr.UserName) + "|" + gradeID;
            CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().UserCookies, value,7,TimeType.Day);
            CookiesHelper.AddCookie("UserPhoto", usr.Photo,7, TimeType.Day);
            CookiesHelper.AddCookie("UserEmail", usr.Email,7,TimeType.Day);
        }
        public static string ReadUserPhoto()
        {
            string userPhoto = CookiesHelper.ReadCookieValue("UserPhoto");
            if (string.IsNullOrEmpty(userPhoto))
            {
                userPhoto = "/Admin/images/nophoto.jpg";
            }
            return userPhoto;
        }

        #region 统计
        /// <summary>
        /// 用户中心首页统计
        /// </summary>
        public static DataTable UserIndexStatistics(int userId)
        {
            return dal.UserIndexStatistics(userId);
        }
        /// <summary>
        /// 供应商用户中心首页统计
        /// </summary>
        public static DataTable ShopIndexStatistics(int shopId)
        {
            return dal.ShopIndexStatistics(shopId);
        }
        /// <summary>
        /// 用户活跃度分析
        /// </summary>
        public static DataTable StatisticsUserActive(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField)
        {
            return dal.StatisticsUserActive(currentPage, pageSize, userSearch, ref count, orderField);
        }
        /// <summary>
        /// 用户消费分析
        /// </summary>
        public static DataTable StatisticsUserConsume(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField, DateTime startDate, DateTime endDate)
        {
            return dal.StatisticsUserConsume(currentPage, pageSize, userSearch, ref count, orderField, startDate, endDate);
        }
        /// <summary>
        /// 统计用户状态
        /// </summary>
        public static DataTable StatisticsUserStatus(UserSearchInfo userSearch)
        {
            return dal.StatisticsUserStatus(userSearch);
        }
        /// <summary>
        /// 统计用户数量
        /// </summary>
        public static DataTable StatisticsUserCount(UserSearchInfo userSearch, DateType dateType)
        {
            return dal.StatisticsUserCount(userSearch, dateType);
        }
        #endregion

        /// <summary>
        /// 读取一条用户数据(包含用户的统计信息)
        /// </summary>
        /// <param name="id">用户的主键值</param>
        /// <returns>用户数据模型</returns>
        public static UserInfo ReadUserMore(int id)
        {
            return dal.ReadUserMore(id);
        }

        public static int CheckUserName(string userName)
        {
            return dal.CheckUserName(userName);
        }

        /// <summary>
        /// 检查E-mail是否被占用
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>真/假</returns>
        public static bool CheckEmail(string email)
        {
            return dal.CheckEmail(email);
        }
        /// <summary>
        /// 检查E-mail是否被占用
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>真/假</returns>
        public static bool CheckEmail(string email, int userId)
        {
            return dal.CheckEmail(email, userId);
        }
        /// <summary>
        /// 检查Mobile是否被占用
        /// </summary>
        /// <param name="email">email</param>
        ///  /// <param name="userId">userId</param>
        /// <returns>真/假</returns>
        public static bool CheckMobile(string mobile, int userId)
        {
            return dal.CheckMobile(mobile, userId);
        }
        /// <summary>
        /// 更改用户的的安全码（找回密码使用）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="safeCode"></param>
        /// <param name="findDate"></param>
        public static void ChangeUserSafeCode(int userID, string safeCode, DateTime findDate)
        {
            dal.ChangeUserSafeCode(userID, safeCode, findDate);
        }

        // 根据userNames[]集合检查是否有重复 true:无重复 false:有重复
        public static bool CheckUserNames(string[] userNames)
        {
            return dal.CheckUserNames(userNames);
        }
        // 根据mobiles[]集合检查是否有重复 true:无重复 false:有重复
        public static bool CheckMobiles(string[] mobiles)
        {
            return dal.CheckMobiles(mobiles);
        }
        // 根据emails[]集合检查是否有重复 true:无重复 false:有重复
        public static bool CheckEmails(string[] emails) {
            return dal.CheckEmails(emails);
        }
           //批量添加
        public static void AddBatch(List<object[]> entities) {
            dal.AddBatch(entities);
        }
        /// <summary>
        /// 用户归属分销商
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="distributorId">分销商Id</param>
        public static void ChangeUserToDistributor(int userId, int distributorId)
        {
            dal.ChangeUserToDistributor(userId, distributorId);
        }
    }
}