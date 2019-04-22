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
    /// 地区数据层说明。
    /// </summary>
    public sealed class RegionDAL : IRegion
    {

        /// <summary>
        /// 增加一条地区数据
        /// </summary>
        /// <param name="region">地区模型变量</param>
        public int AddRegion(RegionInfo region)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@fatherID",SqlDbType.Int),
				new SqlParameter("@orderID",SqlDbType.Int),
				new SqlParameter("@regionName",SqlDbType.NVarChar)
			};
            parameters[0].Value = region.FatherID;
            parameters[1].Value = region.OrderID;
            parameters[2].Value = region.RegionName;
            Object id = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddRegion", parameters);
            return (Convert.ToInt32(id));
        }


        /// <summary>
        /// 更新一条地区数据
        /// </summary>
        /// <param name="region">地区模型变量</param>
        public void UpdateRegion(RegionInfo region)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
				new SqlParameter("@fatherID",SqlDbType.Int),
				new SqlParameter("@orderID",SqlDbType.Int),
				new SqlParameter("@regionName",SqlDbType.NVarChar)
			};
            parameters[0].Value = region.ID;
            parameters[1].Value = region.FatherID;
            parameters[2].Value = region.OrderID;
            parameters[3].Value = region.RegionName;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateRegion", parameters);
        }


        /// <summary>
        /// 删除一条地区数据
        /// </summary>
        /// <param name="id">地区的主键值</param>
        public void DeleteRegion(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteRegion", parameters);
        }

        /// <summary>
        /// 准备地区模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="regionList">地区的数据列表</param>
        public void PrepareRegionModel(SqlDataReader dr, List<RegionInfo> regionList)
        {
            while (dr.Read())
            {
                RegionInfo region = new RegionInfo();
                region.ID = dr.GetInt32(0);
                region.FatherID = dr.GetInt32(1);
                region.OrderID = dr.GetInt32(2);
                region.RegionName = dr[3].ToString();
                regionList.Add(region);
            }
        }

        /// <summary>
        /// 获得地区所有数据列表
        /// </summary>
        /// <returns>地区的所有数据列表</returns>
        public List<RegionInfo> ReadRegionAllList()
        {
            List<RegionInfo> regionList = new List<RegionInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadRegionAllList"))
            {
                PrepareRegionModel(dr, regionList);
            }
            return regionList;
        }


        /// <summary>
        /// 上移分类
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void MoveUpRegion(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "MoveUpRegion", parameters);
        }


        /// <summary>
        /// 下移分类
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void MoveDownRegion(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "MoveDownRegion", parameters);
        }


    }
}