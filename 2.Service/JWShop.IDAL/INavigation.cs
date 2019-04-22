using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface INavigation
    {
        void Add(NavigationInfo entity);
        void Update(NavigationInfo entity);
        void Delete(int id);
        NavigationInfo Read(int id);
        NavigationInfo Read(NavigationClassType classType, int classId);
        List<NavigationInfo> ReadList();
        List<NavigationInfo> ReadList(int navigationType);
        List<NavigationInfo> ReadChildList(int classId);
    }
}