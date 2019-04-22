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
	/// 会员价格业务逻辑。
	/// </summary>
	public sealed class MemberPriceBLL
	{
        
		private static readonly IMemberPrice dal = FactoryHelper.Instance<IMemberPrice>(Global.DataProvider,"MemberPriceDAL");

		/// <summary>
		/// 增加一条会员价格数据
		/// </summary>
		/// <param name="memberPrice">会员价格模型变量</param>
		public static void AddMemberPrice(MemberPriceInfo memberPrice)
		{
			dal.AddMemberPrice(memberPrice);

		}




        /// <summary>
        /// 按会员等级删除会员价格数据
        /// </summary>
        /// <param name="strGradeID">分类ID,以,号分隔</param>
        public static void DeleteMemberPriceByGradeID(string strGradeID)
        {
            dal.DeleteMemberPriceByGradeID(strGradeID);
        }



        /// <summary>
        /// 按产品删除会员价格数据
        /// </summary>
        /// <param name="strProductID">分类ID,以,号分隔</param>
        public static void DeleteMemberPriceByProductID(string strProductID)
        {
            dal.DeleteMemberPriceByProductID(strProductID);
        }


        /// <summary>
        /// 按分类ID获得会员价格所有数据
        /// </summary>
        /// <param name="productID">分类ID</param>
        /// <returns>会员价格数据列表</returns>
        public static List<MemberPriceInfo> ReadMemberPriceByProduct(int productID)
        {
            return dal.ReadMemberPriceByProduct(productID);
        }

        /// <summary>
        /// 按分类ID获得会员价格所有数据
        /// </summary>
        /// <param name="strProductID">分类ID</param>
        /// <returns>会员价格数据列表</returns>
        public static List<MemberPriceInfo> ReadMemberPriceByProduct(string strProductID)
        {
            return dal.ReadMemberPriceByProduct(strProductID);
        }

        /// <summary>
        /// 按分类ID和会员等级获得会员价格所有数据
        /// </summary>
        /// <param name="strProductID">分类ID</param>
        /// <param name="gradeID">会员等级ID</param>
        /// <returns>会员价格数据列表</returns>
        public static List<MemberPriceInfo> ReadMemberPriceByProductGrade(string strProductID,int gradeID)
        {
            return dal.ReadMemberPriceByProductGrade(strProductID, gradeID);
        }
        /// <summary>
        /// 读取产品指定的会员价格
        /// </summary>
        /// <param name="MemberPriceList"></param>
        /// <param name="gradeID"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public static decimal ReadCurrentMemberPrice(List<MemberPriceInfo> MemberPriceList,int gradeID,ProductInfo product)
        {
            decimal result = product.MarketPrice * UserGradeBLL.ReadUserGradeCache(gradeID).Discount/100;
            foreach (MemberPriceInfo memberPrice in MemberPriceList)
            {
                if (memberPrice.GradeID == gradeID && memberPrice.ProductID == product.ID)
                {
                    result = memberPrice.Price;
                    break;
                }
            }
            return Math.Round(result,2);
        }
        /// <summary>
        /// 读取产品指定的会员价格
        /// </summary>
        /// <param name="userGrade"></param>
        /// <param name="memberPriceList"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public static decimal ReadMemberPrice(UserGradeInfo userGrade, List<MemberPriceInfo> memberPriceList, ProductInfo product)
        {
            decimal result = product.MarketPrice * userGrade.Discount / 100;
            foreach(MemberPriceInfo memberPrice in memberPriceList)
            {
                if (memberPrice.GradeID == userGrade.ID)
                {
                    result = memberPrice.Price;
                    break;
                }
            }
            return Math.Round(result, 2);
        }
	}
}

