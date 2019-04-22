using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 广告位接口层说明。
    /// </summary>
    public interface IFlash
    {
        int Add(FlashInfo entity);
        void Update(FlashInfo entity);
        void Delete(int[] ids);
        void Delete(int id);
        FlashInfo Read(int id);
        List<FlashInfo> SearchList();
        List<FlashInfo> SearchList(int currentPage, int pageSize, ref int count);
    }
}
