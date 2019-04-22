using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IFavorableActivityGift
    {
        int Add(FavorableActivityGiftInfo entity);
        void Update(FavorableActivityGiftInfo entity);
        void Delete(int[] ids);
        FavorableActivityGiftInfo Read(int id);
        List<FavorableActivityGiftInfo> SearchList(FavorableActivityGiftSearchInfo searchInfo);
        List<FavorableActivityGiftInfo> SearchList(int currentPage, int pageSize, FavorableActivityGiftSearchInfo searchInfo, ref int count);
    }
}