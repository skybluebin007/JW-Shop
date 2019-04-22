using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IUserGrade
    {
        int Add(UserGradeInfo entity);
        void Update(UserGradeInfo entity);
        void Delete(int[] ids);
        UserGradeInfo Read(int id);
        List<UserGradeInfo> ReadList();
    }
}