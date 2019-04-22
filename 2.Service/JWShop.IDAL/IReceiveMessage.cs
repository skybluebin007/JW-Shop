using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IReceiveMessage
    {
        int Add(ReceiveMessageInfo entity);
        void Update(ReceiveMessageInfo entity);
        void Delete(int id);
        void Delete(int[] ids);
        ReceiveMessageInfo Read(int id);
       List<ReceiveMessageInfo> SearchList(ReceiveMessageSearchInfo searchEntity);
       List<ReceiveMessageInfo> SearchList(int currentPage, int pageSize, ReceiveMessageSearchInfo searchEntity, ref int count);
    }
}
