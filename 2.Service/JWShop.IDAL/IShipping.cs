using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IShipping
    {
        int Add(ShippingInfo entity);
        void Update(ShippingInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        ShippingInfo Read(int id);
        List<ShippingInfo> ReadList();

        void Move(ChangeAction action, int id);
    }
}