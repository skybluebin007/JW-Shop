using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 用户留言接口层说明。
    /// </summary>
    public interface IUserMessage
    {
        int Add(UserMessageInfo entity);
        void Update(UserMessageInfo entity);
        void Delete(int[] ids, int userId = 0);
        UserMessageInfo Read(int id, int userId = 0);
        List<UserMessageInfo> SearchList(UserMessageSeachInfo searchInfo);
        List<UserMessageInfo> SearchList(int currentPage, int pageSize, UserMessageSeachInfo searchInfo, ref int count);
        /// <summary>
        /// 删除该类别下的用户留言数据
        /// </summary>
        /// <param name="strUserID">类别的主键值,以,号分隔</param>
        void DeleteUserMessageByUserID(int[] strUserID);
    }
}