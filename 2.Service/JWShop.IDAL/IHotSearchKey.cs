using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IHotSearchKey
    {
        int Add(HotSearchKeyInfo entity);
        void Update(HotSearchKeyInfo entity);
        void Delete(int id);
        HotSearchKeyInfo Read(int id);
        HotSearchKeyInfo Read(string name);
        List<HotSearchKeyInfo> ReadList();
       
    }
}
