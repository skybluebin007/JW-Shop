using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IShippingRegion
    {
        int Add(ShippingRegionInfo entity);
        void Update(ShippingRegionInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        ShippingRegionInfo Read(int id);
        List<ShippingRegionInfo> ReadList(int shippingId);
        List<ShippingRegionInfo> ReadList(int[] shippingIds);
    }
}