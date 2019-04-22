using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IBase
    {
        int MaxOrderId(string table);
        void UpdatePart(string table, Dictionary<string, object> dict, int id);
    }
}