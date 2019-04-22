using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 砍价接口层说明。
	/// </summary>
	public interface IBargain
	{
		/// <summary>
		/// 增加一条砍价数据
		/// </summary>
		/// <param name="model">砍价模型变量</param>
		int AddBargain(BargainInfo model);

		/// <summary>
		/// 更新一条砍价数据
		/// </summary>
		/// <param name="model">砍价模型变量</param>
		void UpdateBargain(BargainInfo model);	 

		/// <summary>
		/// 删除多条砍价数据
		/// </summary>
		/// <param name="strId">砍价的主键值,以,号分隔</param>
		void DeleteBargain(string strId);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

		/// <summary>
		/// 读取一条砍价数据
		/// </summary>
		/// <param name="id">砍价的主键值</param>
		/// <returns>砍价数据模型</returns>
		BargainInfo ReadBargain(int id);




		/// <summary>
		/// 搜索砍价数据列表
		/// </summary>
		/// <param name="model">BargainSearch模型变量</param>
		/// <returns>砍价数据列表</returns>
		List<BargainInfo> SearchBargainList(BargainSearch model);

		/// <summary>
		/// 搜索砍价数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="model">BargainSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价数据列表</returns>
		List<BargainInfo> SearchBargainList(int currentPage, int pageSize,BargainSearch model,ref int count);


        /// <summary>
        /// 读取正在进行的砍价活动
        /// </summary>
        /// <returns></returns>
        List<BargainInfo> View_ReadBargain();

        /// <summary>
        /// 关闭活动事务：关闭活动，将未支付成功的砍价全部置为“砍价失败”，原“活动已取消，砍价失败”
        /// </summary>
        /// <param name="id"></param>
        void ChangeBargainStatus(int id, int status);

    }
}