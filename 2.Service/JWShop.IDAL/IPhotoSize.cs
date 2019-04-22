using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Text;

namespace JWShop.IDAL
{
   public interface IPhotoSize
    {
        int Add(PhotoSizeInfo entity);
        void Update(PhotoSizeInfo entity);
        void Delete(int[] ids);
        void Delete(int id);
        PhotoSizeInfo Read(int id);
        List<PhotoSizeInfo> SearchList(int type);
        List<PhotoSizeInfo> SearchAllList();
        List<PhotoSizeInfo> SearchList(int type,int currentPage, int pageSize, ref int count);
    }
}
