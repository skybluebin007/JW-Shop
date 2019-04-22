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
	/// 会员价格数据层说明。
	/// </summary>
	public sealed class MemberPriceDAL:IMemberPrice
	{

        /// <summary>
        /// 增加一条会员价格数据
        /// </summary>
        /// <param name="memberPrice">会员价格模型变量</param>
        public void AddMemberPrice(MemberPriceInfo memberPrice)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@productID",SqlDbType.Int),
				new SqlParameter("@gradeID",SqlDbType.Int),
				new SqlParameter("@price",SqlDbType.Decimal)
			};
            parameters[0].Value = memberPrice.ProductID;
            parameters[1].Value = memberPrice.GradeID;
            parameters[2].Value = memberPrice.Price;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "AddMemberPrice", parameters);
        }


        /// <summary>
        /// 按会员等级删除会员价格数据
        /// </summary>
        /// <param name="strGradeID">分类ID,以,号分隔</param>
        public void DeleteMemberPriceByGradeID(string strGradeID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strGradeID",SqlDbType.NVarChar)
			};
            parameters[0].Value = strGradeID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteMemberPriceByGradeID", parameters);
        }



        /// <summary>
        /// 按产品删除会员价格数据
        /// </summary>
        /// <param name="strProductID">分类ID,以,号分隔</param>
        public void DeleteMemberPriceByProductID(string strProductID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strProductID",SqlDbType.NVarChar)
			};
            parameters[0].Value = strProductID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix+"DeleteMemberPriceByProductID", parameters);
        }



        /// <summary>
        /// 准备会员价格模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="memberPriceList">会员价格的数据列表</param>
        public void PrepareMemberPriceModel(SqlDataReader dr, List<MemberPriceInfo> memberPriceList)
        {
            while (dr.Read())
            {
                MemberPriceInfo memberPrice = new MemberPriceInfo();
                memberPrice.ProductID = dr.GetInt32(0);
                memberPrice.GradeID = dr.GetInt32(1);
                memberPrice.Price = dr.GetDecimal(2);
                memberPriceList.Add(memberPrice);
            }
        }



        /// <summary>
        /// 按分类ID获得会员价格所有数据
        /// </summary>
        /// <param name="productID">分类ID</param>
        /// <returns>会员价格数据列表</returns>
        public List<MemberPriceInfo> ReadMemberPriceByProduct(int productID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@productID",SqlDbType.Int)
			};
            parameters[0].Value = productID;
            List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadMemberPriceByProduct", parameters))
            {
                PrepareMemberPriceModel(dr, memberPriceList);
            }
            return memberPriceList;
        }


        /// <summary>
        /// 按分类ID获得会员价格所有数据
        /// </summary>
        /// <param name="productID">分类ID</param>
        /// <returns>会员价格数据列表</returns>
        public List<MemberPriceInfo> ReadMemberPriceByProduct(string strProductID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strProductID",SqlDbType.NChar)
			};
            parameters[0].Value = strProductID;
            List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadMemberPriceByStrProduct", parameters))
            {
                PrepareMemberPriceModel(dr, memberPriceList);
            }
            return memberPriceList;
        }

        /// <summary>
        /// 按分类ID和会员等级获得会员价格所有数据
        /// </summary>
        /// <param name="strProductID">分类ID</param>
        /// <param name="gradeID">会员等级ID</param>
        /// <returns>会员价格数据列表</returns>
        public List<MemberPriceInfo> ReadMemberPriceByProductGrade(string strProductID, int gradeID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strProductID",SqlDbType.NChar),
                new SqlParameter("@gradeID",SqlDbType.Int)
			};
            parameters[0].Value = strProductID;
            parameters[1].Value = gradeID;
            List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadMemberPriceByProductGrade", parameters))
            {
                PrepareMemberPriceModel(dr, memberPriceList);
            }
            return memberPriceList;
        }
	}
}