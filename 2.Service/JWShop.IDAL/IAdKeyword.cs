using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IAdKeyword
    {
        int Add(AdKeywordInfo entity);
        void Update(AdKeywordInfo entity);
        void Delete(int id);
        AdKeywordInfo Read(int id);
        List<AdKeywordInfo> ReadList();
    }
}