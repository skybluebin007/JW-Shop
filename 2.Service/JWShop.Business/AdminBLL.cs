using System;
using System.Xml;
using System.Web;
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
    /// 管理员业务逻辑。
    /// </summary>
    public sealed class AdminBLL : BaseBLL
    {
        private static readonly IAdmin dal = FactoryHelper.Instance<IAdmin>(Global.DataProvider, "AdminDAL");

        public static int Add(AdminInfo entity)
        {
            entity.Id = dal.Add(entity);
            AdminGroupBLL.ChangeCount(entity.GroupId, ChangeAction.Plus);
            return entity.Id;
        }

        public static void Update(AdminInfo entity)
        {
            AdminInfo tempAdmin = Read(entity.Id);
            dal.Update(entity);
            if (entity.GroupId != tempAdmin.GroupId)
            {
                AdminGroupBLL.ChangeCount(tempAdmin.GroupId, ChangeAction.Minus);
                AdminGroupBLL.ChangeCount(entity.GroupId, ChangeAction.Plus);
            }
        }

        public static void Delete(int[] ids)
        {
            AdminLogBLL.DeleteByAdminIds(ids);
            AdminGroupBLL.ChangeCountByGeneral(ids, ChangeAction.Minus);
            dal.Delete(ids);
        }

        public static void DeleteByGroupIds(int[] groupIds)
        {
            AdminLogBLL.DeleteByGroupIds(groupIds);
            dal.DeleteByGroupIds(groupIds);
        }

        public static AdminInfo Read(int id)
        {
            return dal.Read(id);
        }
        public static AdminInfo Read(string name)
        {
            return dal.Read(name);
        }
        public static List<AdminInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, ref count);
        }

        public static List<AdminInfo> ReadList(int groupId, int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(groupId, currentPage, pageSize, ref count);
        }

        public static void ChangePassword(int id, string newPassword)
        {
            dal.ChangePassword(id, newPassword);
        }

        public static void ChangePassword(int id, string oldPassword, string newPassword)
        {
            dal.ChangePassword(id, oldPassword, newPassword);
        }

        public static void UpdateLogin(int id, DateTime lastLoginTime, string lastLoginIP)
        {
            dal.UpdateLogin(id, lastLoginTime, lastLoginIP);
        }

        public static void UpdateLogin(string name, DateTime lastLoginTime, string lastLoginIP, int maxErrorTimes)
        {
            dal.UpdateLogin(name, lastLoginTime, lastLoginIP, maxErrorTimes);
        }

        public static AdminInfo CheckLogin(string loginName, string loginPass)
        {
            return dal.CheckLogin(loginName, loginPass);
        }
        /// <summary>
        /// 更改用户的的安全码（找回密码使用）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="safeCode"></param>
        /// <param name="findDate"></param>
        public static void ChangeAdminSafeCode(int adminID, string safeCode, DateTime findDate)
        {
            dal.ChangeAdminSafeCode(adminID, safeCode, findDate);
        }
        /// <summary>
        /// 管理员状态解锁，错误次数清零
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateStatus(int id)
        {
            dal.UpdateStatus(id);
        }
        /// <summary>
        /// 不能删除的管理员 
        /// </summary>
        /// <param name="isCreater"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string NoDelete(object isCreater, object id)
        {
            int tempIsCreater = Convert.ToInt32(isCreater);
            int tempID = Convert.ToInt32(id);
            string strIsCreater = string.Empty;
            if (tempIsCreater != (int)BoolType.True && tempID != Cookies.Admin.GetAdminID(false))
            {
                strIsCreater = "<input type=\"checkbox\" name=\"SelectID\" value=\"" + id.ToString() + "\" ig-bind=\"list\" />";
            }
            return strIsCreater;
        }
        /// <summary>
        /// 不能删除的管理员 
        /// </summary>
        /// <param name="isCreater"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string NoDelete(object isCreater, object id, int showType)
        {
            int tempIsCreater = Convert.ToInt32(isCreater);
            int tempID = Convert.ToInt32(id);
            string strIsCreater = string.Empty;
            if (tempIsCreater != (int)BoolType.True && tempID != Cookies.Admin.GetAdminID(false))
            {
                if (showType == 1)
                {
                    strIsCreater = "<label class=\"ig-checkbox\"><input type=\"checkbox\" name=\"SelectID\" value=\"" + id.ToString() + "\" ig-bind=\"list\"/></label>";
                }
                else
                {
                    strIsCreater = " | <a href=\"?Action=Del&ID=" + id + "\" onclick=\"return confirm('确认删除？')\"> 删除</a>";
                }
            }
            return strIsCreater;
        }
        /// <summary>
        /// 不能修改密码的管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string NoPasswordAdd(object id, object isCreater)
        {
            int tempIsCreater = Convert.ToInt32(isCreater);
            int tempID = Convert.ToInt32(id);
            string passwordAdd = string.Empty;
            if (tempID == Cookies.Admin.GetAdminID(true) || tempIsCreater != (int)BoolType.True)
            {
                passwordAdd = " <a href=\"PasswordAdd.aspx?ID=" + id.ToString() + "\">修改密码</a> ";
            }
            return passwordAdd;
        }
        /// <summary>
        /// 不能修改的管理员
        /// </summary>
        /// <param name="isCreater">是否系统自带（admin）</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string NoUpdate(object isCreater, object id)
        {
            int tempIsCreater = Convert.ToInt32(isCreater);
            int tempID = Convert.ToInt32(id);
            string noDelete = string.Empty;
            //if (tempIsCreater != (int)BoolType.True && tempID != Cookies.Admin.GetAdminID(false))
            if (tempIsCreater != (int)BoolType.True)
            {
                noDelete = " <a href=\"AdminAdd.aspx?ID=" + id.ToString() + "\">修改</a> ";
            }
            return noDelete;
        }
        /// <summary>
        /// 检查Email唯一性,true:唯一
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UniqueEmail(string email, int id = 0) {
            return dal.UniqueEmail(email, id);
        }
    }
}