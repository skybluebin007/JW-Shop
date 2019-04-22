using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 链接业务逻辑。
    /// </summary>
    public sealed class LinkBLL
    {

        private static readonly ILink dal = FactoryHelper.Instance<ILink>(Global.DataProvider, "LinkDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Link");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("Link");

        /// <summary>
        /// 增加一条链接数据
        /// </summary>
        /// <param name="link">链接模型变量</param>
        public static int AddLink(LinkInfo link)
        {
            link.ID = dal.AddLink(link);
            UploadBLL.UpdateUpload(TableID, 0, link.ID, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
            return link.ID;
        }



        /// <summary>
        /// 更新一条链接数据
        /// </summary>
        /// <param name="link">链接模型变量</param>
        public static void UpdateLink(LinkInfo link)
        {
            dal.UpdateLink(link);
            UploadBLL.UpdateUpload(TableID, 0, link.ID, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
        }


        /// <summary>
        /// 删除多条链接数据
        /// </summary>
        /// <param name="strID">链接的主键值,以,号分隔</param>
        public static void DeleteLink(string strID)
        {
            UploadBLL.DeleteUploadByRecordID(TableID, strID);
            dal.DeleteLink(strID);
            CacheHelper.Remove(cacheKey);
        }




        /// <summary>
        /// 从缓存读取一条链接数据
        /// </summary>
        /// <param name="id">链接的主键值</param>
        /// <returns>链接数据模型</returns>
        public static LinkInfo ReadLinkCache(int id)
        {
            LinkInfo link = new LinkInfo();
            List<LinkInfo> linkList = ReadLinkCacheList();
            foreach (LinkInfo temp in linkList)
            {
                if (temp.ID == id)
                {
                    link = temp;
                    break;
                }
            }
            return link;
        }


        /// <summary>
        /// 从缓存中读取数据链接列表
        /// </summary>
        /// <returns>链接数据列表</returns>
        public static List<LinkInfo> ReadLinkCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadLinkAllList());
            }
            return (List<LinkInfo>)CacheHelper.Read(cacheKey);
        }

        /// <summary>
        /// 通过分类从缓存中读取数据链接列表
        /// </summary>
        /// <param name="classID">分类ID</param>
        /// <returns>链接数据列表</returns>
        public static List<LinkInfo> ReadLinkCacheListByClass(int classID)
        {
            List<LinkInfo> result = new List<LinkInfo>();
            List<LinkInfo> linkList = ReadLinkCacheList();
            foreach (LinkInfo temp in linkList)
            {
                if (temp.LinkClass == classID)
                {
                    result.Add(temp);
                }
            }
            return result;
        }


        /// <summary>
        /// 移动数据链接排序
        /// </summary>
        /// <param name="action">改变动作,向上:ChangeAction.Up;向下:ChangeActon.Down</param>
        /// <param name="id">当前记录的主键值</param>
        public static void ChangeLinkOrder(ChangeAction action, int id)
        {
            dal.ChangeLinkOrder(action, id);
            CacheHelper.Remove(cacheKey);
        }


        /// <summary>
        /// 读取友情链接的显示
        /// </summary>
        /// <param name="display"></param>
        /// <param name="linkClass"></param>
        /// <returns></returns>
        public static string ReadLinkDisplay(object display, object linkClass)
        {
            string strDisplay = display.ToString();
            if (Convert.ToInt32(linkClass) == (int)LinkType.Picture)
            {
                strDisplay = "<img src=\"" + strDisplay + "\" width=\"88\" height=\"31\"/>";
            }
            return strDisplay;
        }


    }
}