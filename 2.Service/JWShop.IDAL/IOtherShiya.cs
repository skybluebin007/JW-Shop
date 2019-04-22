using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IOtherShiya
    {
        int Add(OtherShiyaInfo entity);
        void Update(OtherShiyaInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        OtherShiyaInfo Read(int id);

        List<OtherShiyaInfo> ReadList(int userid,int usertype);


        List<OtherShiyaInfo> SearchList(OtherShiyaSearchInfo searchInfo);
        List<OtherShiyaInfo> SearchList(int currentPage, int pageSize, OtherShiyaSearchInfo searchInfo, ref int count);
    }
}
