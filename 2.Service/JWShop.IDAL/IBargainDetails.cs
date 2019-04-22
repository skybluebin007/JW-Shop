using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 砍价详情接口层说明。
	/// </summary>
	public interface IBargainDetails
	{
		/// <summary>
		/// 增加一条砍价详情数据
		/// </summary>
		/// <param name="model">砍价详情模型变量</param>
		int AddBargainDetails(BargainDetailsInfo model);

		/// <summary>
		/// 更新一条砍价详情数据
		/// </summary>
		/// <param name="model">砍价详情模型变量</param>
		void UpdateBargainDetails(BargainDetailsInfo model);	 

		/// <summary>
		/// 删除多条砍价详情数据
		/// </summary>
		/// <param name="strId">砍价详情的主键值,以,号分隔</param>
		void DeleteBargainDetails(string strId);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        void Delete(int id);

		/// <summary>
		/// 读取一条砍价详情数据
		/// </summary>
		/// <param name="id">砍价详情的主键值</param>
		/// <returns>砍价详情数据模型</returns>
		BargainDetailsInfo ReadBargainDetails(int id);

        /// <summary>
        /// 读取分类下面的所有商品详情
        /// </summary>
        /// <param name="BargainId"></param>
        /// <returns></returns>
        List<BargainDetailsInfo> ReadByBargainId(int BargainId);


        /// <summary>
        /// 读取所有
        /// </summary>
        /// <returns></returns>
        List<BargainDetailsInfo> ReadList();

        /// <summary>
        /// 读取正在参加砍价的商品列表(Id,Name)
        /// </summary>
        /// <returns></returns>
        List<BargainDetailsInfo> ReadBargainProducts();
        /// <summary>
        /// 获取参与砍价的用户姓名 头像
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<BargainUserInfo> GetBargainUsers(int id);
            
      

    }
}