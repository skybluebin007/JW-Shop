using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
	/// <summary>
	/// 菜单数据层说明。
	/// </summary>
	public sealed class MenuDAL:IMenu
	{ 

		/// <summary>
		/// 增加一条菜单数据
		/// </summary>
		/// <param name="Menu">菜单模型变量</param>
		public int AddMenu(MenuInfo Menu)
		{
            SqlParameter[] parameters ={
				new SqlParameter("@fatherID",SqlDbType.Int),
				new SqlParameter("@orderID",SqlDbType.Int),
				new SqlParameter("@menuName",SqlDbType.NVarChar),
				new SqlParameter("@menuImage",SqlDbType.Int),
				new SqlParameter("@uRL",SqlDbType.NVarChar),
				new SqlParameter("@date",SqlDbType.DateTime),
				new SqlParameter("@iP",SqlDbType.NVarChar)
			};
            parameters[0].Value = Menu.FatherID;
            parameters[1].Value = Menu.OrderID;
            parameters[2].Value = Menu.MenuName;
            parameters[3].Value = Menu.MenuImage;
            parameters[4].Value = Menu.URL;
            parameters[5].Value = Menu.Date;
            parameters[6].Value = Menu.IP;
			Object id= ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix+"AddMenu",parameters);
			return(Convert.ToInt32(id));
	}


		/// <summary>
		/// 更新一条菜单数据
		/// </summary>
		/// <param name="Menu">菜单模型变量</param>
		public void UpdateMenu(MenuInfo Menu)
		{
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
				new SqlParameter("@fatherID",SqlDbType.Int),
				new SqlParameter("@orderID",SqlDbType.Int),
				new SqlParameter("@menuName",SqlDbType.NVarChar),
				new SqlParameter("@menuImage",SqlDbType.Int),
				new SqlParameter("@uRL",SqlDbType.NVarChar)
			};
            parameters[0].Value = Menu.ID;
            parameters[1].Value = Menu.FatherID;
            parameters[2].Value = Menu.OrderID;
            parameters[3].Value = Menu.MenuName;
            parameters[4].Value = Menu.MenuImage;
            parameters[5].Value = Menu.URL;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"UpdateMenu",parameters);
		}


		/// <summary>
		/// 删除一条菜单数据
		/// </summary>
		/// <param name="id">菜单的主键值</param>
		public void DeleteMenu(int id)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
			parameters[0].Value = id;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteMenu",parameters);
		}

		/// <summary>
		/// 准备菜单模型
		/// </summary>
		/// <param name="dr">Datareader</param>
		/// <param name="MenuList">菜单的数据列表</param>
		public void PrepareMenuModel(SqlDataReader dr,List<MenuInfo> MenuList)
		{
			while (dr.Read())
			{
                MenuInfo Menu = new MenuInfo();
                Menu.ID = dr.GetInt32(0);
                Menu.FatherID = dr.GetInt32(1);
                Menu.OrderID = dr.GetInt32(2);
                Menu.MenuName = dr[3].ToString();
                Menu.MenuImage = dr.GetInt32(4);
                Menu.URL = dr[5].ToString();
                Menu.Date = dr.GetDateTime(6);
                Menu.IP = dr[7].ToString();
                MenuList.Add(Menu);
			}
		} 

		/// <summary>
		/// 获得菜单所有数据列表
		/// </summary>
		/// <returns>菜单的所有数据列表</returns>
		public List<MenuInfo> ReadMenuAllList()
		{
			List<MenuInfo> MenuList = new List<MenuInfo>();	
			using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix+"ReadMenuAllList"))
			{
				PrepareMenuModel(dr,MenuList);
			}
			return MenuList;
		}


		/// <summary>
		/// 上移分类
		/// </summary>
		/// <param name="id">要移动的id</param>
		public void MoveUpMenu(int id)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
			parameters[0].Value = id;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"MoveUpMenu",parameters);	
		}


		/// <summary>
		/// 下移分类
		/// </summary>
		/// <param name="id">要移动的id</param>
		public void MoveDownMenu(int id)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
			parameters[0].Value = id;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"MoveDownMenu",parameters);	
		}


	}
}