using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IOtherDuihuan
    {
        int Add(OtherDuihuanInfo entity);
        void Update(OtherDuihuanInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        OtherDuihuanInfo Read(int id);
        OtherDuihuanInfo Read_User(int userid);
        List<OtherDuihuanInfo> ReadList(int userid);
        List<OtherDuihuanInfo> SearchList(OtherDuihuanSearchInfo searchInfo);
        List<OtherDuihuanInfo> SearchList(int currentPage, int pageSize, OtherDuihuanSearchInfo searchInfo, ref int count);
    }
}
