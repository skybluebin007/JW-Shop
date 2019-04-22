using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 管理组接口层说明。
    /// </summary>
    public interface IAdminGroup
    {
        int Add(AdminGroupInfo entity);
        void Update(AdminGroupInfo entity);
        void Delete(int[] ids);
        List<AdminGroupInfo> ReadList();

        void ChangeCount(int id, ChangeAction action);
        void ChangeCountByGeneral(int[] adminIds, ChangeAction action);
    }
}