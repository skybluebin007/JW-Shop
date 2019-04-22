using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IUserAddress
    {
        int Add(UserAddressInfo entity);
        void Update(UserAddressInfo entity);
        void Delete(int id, int userId);
        UserAddressInfo Read(int id, int userId);
        List<UserAddressInfo> ReadList(int userId);
        void SetDefault(int id, int userId);
    }
}