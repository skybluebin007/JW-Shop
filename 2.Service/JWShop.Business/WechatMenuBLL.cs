using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    /// <summary>
    /// 微信菜单业务逻辑。
    /// </summary>
    public sealed class WechatMenuBLL:BaseBLL
    {
        private static readonly IWechatMenu dal = FactoryHelper.Instance<IWechatMenu>(Global.DataProvider, "WechatMenuDAL");
        public static int Add(WechatMenuInfo entity)
        {
            return dal.Add(entity);
        }
        public static void Update(WechatMenuInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static WechatMenuInfo Read(int id)
        {
            return dal.Read(id);
        }
        public static List<WechatMenuInfo> ReadList()
        {
            return dal.ReadList();
        }
        public static List<WechatMenuInfo> ReadChildList(int fatherid)
        {
            return dal.ReadChildList(fatherid);
        }
        public static List<WechatMenuInfo> ReadRootList()
        {
            return ReadList().Where(k => k.FatherId == 0).ToList();
        }
        /// <summary>
        /// 上移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveUpWechatMenu(int id)
        {
            dal.MoveUpWechatMenu(id);
        }


        /// <summary>
        /// 下移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveDownWechatMenu(int id)
        {
            dal.MoveDownWechatMenu(id);
        }
    }
}
