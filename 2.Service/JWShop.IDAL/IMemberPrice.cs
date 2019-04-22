using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 会员价格接口层说明。
    /// </summary>
    public interface IMemberPrice
    {
        /// <summary>
        /// 增加一条会员价格数据
        /// </summary>
        /// <param name="memberPrice">会员价格模型变量</param>
        void AddMemberPrice(MemberPriceInfo memberPrice);


        /// <summary>
        /// 按会员等级删除会员价格数据
        /// </summary>
        /// <param name="strGradeID">分类ID,以,号分隔</param>
        void DeleteMemberPriceByGradeID(string strGradeID);



        /// <summary>
        /// 按产品删除会员价格数据
        /// </summary>
        /// <param name="strProductID">分类ID,以,号分隔</param>
        void DeleteMemberPriceByProductID(string strProductID);


        /// <summary>
        /// 按分类ID获得会员价格所有数据
        /// </summary>
        /// <param name="productID">分类ID</param>
        /// <returns>会员价格数据列表</returns>
        List<MemberPriceInfo> ReadMemberPriceByProduct(int productID);

        /// <summary>
        /// 按分类ID获得会员价格所有数据
        /// </summary>
        /// <param name="strProductID">分类ID</param>
        /// <returns>会员价格数据列表</returns>
       List<MemberPriceInfo> ReadMemberPriceByProduct(string strProductID);

       /// <summary>
       /// 按分类ID和会员等级获得会员价格所有数据
       /// </summary>
       /// <param name="strProductID">分类ID</param>
       /// <param name="gradeID">会员等级ID</param>
       /// <returns>会员价格数据列表</returns>
       List<MemberPriceInfo> ReadMemberPriceByProductGrade(string strProductID, int gradeID);
    }
}


