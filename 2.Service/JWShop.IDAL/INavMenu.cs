using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface INavMenu
    {
        int Add(NavMenuInfo entity);
        void Update(NavMenuInfo entity);
        void Delete(int id);
        NavMenuInfo Read(int id);
         /// <summary>
        /// 修改导航排序
        /// </summary>
        /// <param name="id">要移动的id</param>
        void ChangeNavMenuOrder(int id, int orderId);
        List<NavMenuInfo> ReadList();

        List<NavMenuInfo> ReadList(bool isShow);

        void Move(int id, ChangeAction action);

    }
}

