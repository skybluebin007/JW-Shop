using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class ReceiveMessageBLL:BaseBLL
    {
        private static readonly IReceiveMessage dal = FactoryHelper.Instance<IReceiveMessage>(Global.DataProvider, "ReceiveMessageDAL");

        public static int Add(ReceiveMessageInfo entity)
        {

            return dal.Add(entity);
        }
        public static void Update(ReceiveMessageInfo entity)
        {

            dal.Update(entity);
        }
        public static void Delete(int id)
        {

            dal.Delete(id);
        }
        public static void Delete(int[] ids) {
            dal.Delete(ids);
        }
        public static ReceiveMessageInfo Read(int id)
        {
            return dal.Read(id);
        }
        public static List<ReceiveMessageInfo> SearchList(ReceiveMessageSearchInfo searchEntity)
        {
            return dal.SearchList(searchEntity);
        }
        public static List<ReceiveMessageInfo> SearchList(int currentPage, int pageSize, ReceiveMessageSearchInfo searchEntity, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchEntity, ref count);
        }
    }
}
