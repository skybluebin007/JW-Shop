using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 菜单接口层说明。
	/// </summary>
	public interface IMenu
	{
		/// <summary>
		/// 增加一条菜单数据
		/// </summary>
		/// <param name="Menu">菜单模型变量</param>
		int AddMenu(MenuInfo Menu);

		/// <summary>
		/// 更新一条菜单数据
		/// </summary>
		/// <param name="Menu">菜单模型变量</param>
		void UpdateMenu(MenuInfo Menu); 

		/// <summary>
		/// 删除一条菜单数据
		/// </summary>
		/// <param name="id">菜单的主键值</param>
		void DeleteMenu(int id);

		/// <summary>
		/// 获得菜单所有数据
		/// </summary>
		/// <returns>菜单数据列表</returns>
		List<MenuInfo> ReadMenuAllList();


		/// <summary>
		/// 上移菜单数据
		/// </summary>
		/// <param name="id">要移动的id</param>
		void MoveUpMenu(int id);

		/// <summary>
		/// 下移菜单数据
		/// </summary>
		/// <param name="id">要移动的id</param>
		void MoveDownMenu(int id);

	}
}