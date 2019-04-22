using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IThemeActivity
    {
        int Add(ThemeActivityInfo entity);
        void Update(ThemeActivityInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        ThemeActivityInfo Read(int id);
        List<ThemeActivityInfo> ReadList();
    }
}