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
    /// 用户留言业务逻辑。
    /// </summary>
    public sealed class UserMessageBLL : BaseBLL
    {
        private static readonly IUserMessage dal = FactoryHelper.Instance<IUserMessage>(Global.DataProvider, "UserMessageDAL");

        public static int Add(UserMessageInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(UserMessageInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids, int userId = 0)
        {
            dal.Delete(ids, userId);
        }

        public static UserMessageInfo Read(int id, int userId = 0)
        {
            return dal.Read(id, userId);
        }

        public static List<UserMessageInfo> SearchList(UserMessageSeachInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<UserMessageInfo> SearchList(int currentPage, int pageSize, UserMessageSeachInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
        /// <summary>
        /// 按分类删除用户留言数据
        /// </summary>
        /// <param name="strUserID">分类ID,以,号分隔</param>
        public static void DeleteUserMessageByUserID(int[] strUserID)
        {
            dal.DeleteUserMessageByUserID(strUserID);
        }
        /// <summary>
        /// 读取留言类型
        /// </summary>
        public static string ReadMessageType(int messageType)
        {
            string result = string.Empty;
            switch (messageType)
            {
                case (int)JWShop.Entity.MessageType.Message:
                    result = "留言";
                    break;
                case (int)JWShop.Entity.MessageType.Complain:
                    result = "投诉";
                    break;
                case (int)JWShop.Entity.MessageType.Inquire:
                    result = "询问";
                    break;
                case (int)JWShop.Entity.MessageType.AfterSale:
                    result = "售后";
                    break;
                case (int)JWShop.Entity.MessageType.DemandBuy:
                    result = "求购";
                    break;
                case 6:
                    result="在线咨询";
                    break;
                case 7:
                    result = "在线预约";
                    break;
                case 8:
                    result = "申请";
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}