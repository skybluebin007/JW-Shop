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
	/// 链接数据层说明。
	/// </summary>
	public sealed class LinkDAL:ILink
	{ 

		/// <summary>
		/// 增加一条链接数据
		/// </summary>
		/// <param name="link">链接模型变量</param>
		public int AddLink(LinkInfo link)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@linkClass",SqlDbType.Int),
				new SqlParameter("@display",SqlDbType.NVarChar),
				new SqlParameter("@uRL",SqlDbType.NVarChar),
				new SqlParameter("@orderID",SqlDbType.Int),
				new SqlParameter("@remark",SqlDbType.NVarChar)
			};
			parameters[0].Value = link.LinkClass;
			parameters[1].Value = link.Display;
			parameters[2].Value = link.URL;
			parameters[3].Value = link.OrderID;
			parameters[4].Value = link.Remark;
			Object id= ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix+"AddLink",parameters);
			return(Convert.ToInt32(id));
		}

		
		/// <summary>
		/// 更新一条链接数据
		/// </summary>
		/// <param name="link">链接模型变量</param>
		public void UpdateLink(LinkInfo link)
		{			 
			SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
				new SqlParameter("@linkClass",SqlDbType.Int),
				new SqlParameter("@display",SqlDbType.NVarChar),
				new SqlParameter("@uRL",SqlDbType.NVarChar),
				new SqlParameter("@remark",SqlDbType.NVarChar)
			};
			parameters[0].Value = link.ID;
			parameters[1].Value = link.LinkClass;
			parameters[2].Value = link.Display;
			parameters[3].Value = link.URL;
			parameters[4].Value = link.Remark;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"UpdateLink",parameters);
		}

		/// <summary>
		/// 删除多条链接数据
		/// </summary>
		/// <param name="strID">链接的主键值,以,号分隔</param>
		public void DeleteLink(string strID)
		{		
			SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar)
			};
			parameters[0].Value = strID;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteLink",parameters);
		}





        /// <summary>
        /// 准备链接模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="linkList">链接的数据列表</param>
        public void PrepareLinkModel(SqlDataReader dr, List<LinkInfo> linkList)
        {
            while (dr.Read())
            {
                LinkInfo link = new LinkInfo();
                link.ID = dr.GetInt32(0);
                link.LinkClass = dr.GetInt32(1);
                link.Display = dr[2].ToString();
                link.URL = dr[3].ToString();
                link.OrderID = dr.GetInt32(4);
                link.Remark = dr[5].ToString();
                linkList.Add(link);
            }
        }

        /// <summary>
        /// 获得链接的所有数据列表
        /// </summary>
        /// <returns>链接的所有数据列表</returns>
        public List<LinkInfo> ReadLinkAllList()
        {
            List<LinkInfo> linkList = new List<LinkInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadLinkAllList"))
            {
                PrepareLinkModel(dr, linkList);
            }
            return linkList;
        }

     

		/// <summary>
		/// 移动数据排序
		/// </summary>
		/// <param name="action">改变动作,向上:ChangeAction.Up;向下:ChangeActon.Down</param>
		/// <param name="id">当前记录的主键值</param>
		public void ChangeLinkOrder(ChangeAction action, int id)
		{
			SqlParameter[] parameters ={
				new SqlParameter("@action",SqlDbType.NVarChar),
				new SqlParameter("@id",SqlDbType.Int)
			};
			parameters[0].Value = action.ToString();
			parameters[1].Value = id;
			ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"ChangeLinkOrder",parameters);
		}


	}
}