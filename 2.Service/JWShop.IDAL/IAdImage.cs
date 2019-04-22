using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IAdImage
    {
        int Add(AdImageInfo entity);
        void Update(AdImageInfo entity);
        void Delete(int id);
        void DeleteByAdType(int adType);
        AdImageInfo Read(int id);
        List<AdImageInfo> ReadList();
        List<AdImageInfo> ReadList(int adType);
    }
}