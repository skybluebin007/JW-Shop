using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    /// <summary>
    /// 菜单业务逻辑。
    /// </summary>
    public sealed class MenuBLL
    {
        private static readonly IMenu dal = FactoryHelper.Instance<IMenu>(Global.DataProvider, "MenuDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("Menu");

        /// <summary>
        /// 增加一条菜单数据
        /// </summary>
        /// <param name="Menu">菜单模型变量</param>
        public static int AddMenu(MenuInfo menu)
        {
            menu.ID = dal.AddMenu(menu);
            CacheHelper.Remove(cacheKey);
            return menu.ID;
        }

        /// <summary>
        /// 更新一条菜单数据
        /// </summary>
        /// <param name="Menu">菜单模型变量</param>
        public static void UpdateMenu(MenuInfo menu)
        {
            dal.UpdateMenu(menu);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 删除一条菜单数据
        /// </summary>
        /// <param name="id">菜单的主键值</param>
        public static void DeleteMenu(int id)
        {
            dal.DeleteMenu(id);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 从缓存中读取菜单所有数据列表
        /// </summary>
        /// <returns>菜单数据模型</returns>
        public static List<MenuInfo> ReadMenuCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadMenuAllList());
            }
            return (List<MenuInfo>)CacheHelper.Read(cacheKey);
        }

        /// <summary>
        /// 从缓存中读取一条菜单数据
        /// </summary>
        /// <param name="id">菜单的主键值</param>
        /// <returns>菜单数据模型</returns>
        public static MenuInfo ReadMenuCache(int id)
        {
            MenuInfo menu = new MenuInfo();
            List<MenuInfo> menuList = ReadMenuCacheList();
            foreach (MenuInfo temp in menuList)
            {
                if (temp.ID == id)
                {
                    menu = temp;
                    break;
                }
            }
            return menu;
        }

        /// <summary>
        /// 读取第一级菜单分类列表
        /// </summary>
        /// <returns>菜单数据列表</returns>
        public static List<MenuInfo> ReadMenuRootList()
        {
            List<MenuInfo> result = new List<MenuInfo>();
            List<MenuInfo> menuList = ReadMenuCacheList();
            foreach (MenuInfo menu in menuList)
            {
                if (menu.FatherID == 0)
                {
                    result.Add(menu);
                }
            }
            return result;
        }

        /// <summary>
        /// 读取某大类的二级子分类
        /// </summary>
        /// <param name="fatherID">父类ID</param>
        /// <returns>菜单数据列表</returns>
        public static List<MenuInfo> ReadMenuChildList(int fatherID)
        {
            List<MenuInfo> result = new List<MenuInfo>();
            List<MenuInfo> menuList = ReadMenuCacheList();
            foreach (MenuInfo menu in menuList)
            {
                if (menu.FatherID == fatherID)
                {
                    result.Add(menu);
                }
            }
            return result;
        }

        /// <summary>
        /// 读取某大类的二级子分类
        /// </summary>
        /// <param name="fatherID">父类ID</param>
        /// <param name="depth">层级</param>
        /// <returns>菜单数据列表</returns>
        private static List<MenuInfo> ReadMenuChildList(int fatherID, int depth)
        {
            List<MenuInfo> result = new List<MenuInfo>();
            List<MenuInfo> menuList = ReadMenuCacheList();
            foreach (MenuInfo menu in menuList)
            {
                if (menu.FatherID == fatherID)
                {
                    MenuInfo temp = (MenuInfo)ServerHelper.CopyClass(menu);
                    string tempString = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        tempString += HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    temp.MenuName = tempString + temp.MenuName;
                    result.Add(temp);
                    result.AddRange(ReadMenuChildList(temp.ID, depth + 1));
                }
            }
            return result;
        }

        /// <summary>
        /// 读取名字已经缩进好的分类列表
        /// </summary>
        /// <returns>菜单数据列表</returns>
        public static List<MenuInfo> ReadMenuNamedList()
        {
            List<MenuInfo> result = new List<MenuInfo>();
            List<MenuInfo> menuList = ReadMenuRootList();
            foreach (MenuInfo menu in menuList)
            {
                result.Add(menu);
                result.AddRange(ReadMenuChildList(menu.ID, 2));
            }
            return result;
        }

        /// <summary>
        /// 读取一级分类的所有子类（列表管理）
        /// </summary>
        /// <param name="fatherID">父类ID</param>
        /// <returns>菜单数据列表</returns>
        public static List<MenuInfo> ReadMenuAllNamedChildList(int fatherID)
        {
            List<MenuInfo> result = new List<MenuInfo>();
            List<MenuInfo> menuList = ReadMenuNamedList();
            bool record = false;
            foreach (MenuInfo menu in menuList)
            {
                if (menu.FatherID == fatherID)
                {
                    record = true;
                }
                if (menu.FatherID == 0)
                {
                    record = false;
                }
                if (record)
                {
                    result.Add(menu);
                }
            }
            return result;
        }

        /// <summary>
        /// 上移菜单分类
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveUpMenu(int id)
        {
            dal.MoveUpMenu(id);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 下移菜单分类
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveDownMenu(int id)
        {
            dal.MoveDownMenu(id);
            CacheHelper.Remove(cacheKey);
        }
    }
}