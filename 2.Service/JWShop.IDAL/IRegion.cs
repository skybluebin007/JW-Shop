using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 地区接口层说明。
	/// </summary>
	public interface IRegion
	{
		/// <summary>
		/// 增加一条地区数据
		/// </summary>
		/// <param name="region">地区模型变量</param>
		int AddRegion(RegionInfo region);

		/// <summary>
		/// 更新一条地区数据
		/// </summary>
		/// <param name="region">地区模型变量</param>
		void UpdateRegion(RegionInfo region); 

		/// <summary>
		/// 删除一条地区数据
		/// </summary>
		/// <param name="id">地区的主键值</param>
		void DeleteRegion(int id);

		/// <summary>
		/// 获得地区所有数据
		/// </summary>
		/// <returns>地区数据列表</returns>
		List<RegionInfo> ReadRegionAllList();


		/// <summary>
		/// 上移地区数据
		/// </summary>
		/// <param name="id">要移动的id</param>
		void MoveUpRegion(int id);

		/// <summary>
		/// 下移地区数据
		/// </summary>
		/// <param name="id">要移动的id</param>
		void MoveDownRegion(int id);

	}
}