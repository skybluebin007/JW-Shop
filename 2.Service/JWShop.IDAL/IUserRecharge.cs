using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IUserRecharge
    {
        int Add(UserRechargeInfo entity);
        void Update(UserRechargeInfo entity);
        void Delete(int id, int userId);
        UserRechargeInfo Read(int id, int userId);
        UserRechargeInfo Read(string number, int userId);
        UserRechargeInfo Read(string number);
        List<UserRechargeInfo> ReadList(int userId);
        List<UserRechargeInfo> SearchList(int currentPage, int pageSize, UserRechargeSearchInfo searchInfo, ref int count);
    }
}