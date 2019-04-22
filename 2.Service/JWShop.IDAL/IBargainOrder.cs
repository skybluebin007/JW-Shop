using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 砍价订单接口层说明。
	/// </summary>
	public interface IBargainOrder
	{
		/// <summary>
		/// 增加一条砍价订单数据
		/// </summary>
		/// <param name="mode">砍价订单模型变量</param>
		int AddBargainOrder(BargainOrderInfo mode);

		/// <summary>
		/// 更新一条砍价订单数据
		/// </summary>
		/// <param name="mode">砍价订单模型变量</param>
		bool UpdateBargainOrder(BargainOrderInfo mode);	 

		/// <summary>
		/// 删除多条砍价订单数据
		/// </summary>
		/// <param name="strId">砍价订单的主键值,以,号分隔</param>
		void DeleteBargainOrder(string strId);


        void Delete(int id);

		/// <summary>
		/// 读取一条砍价订单数据
		/// </summary>
		/// <param name="id">砍价订单的主键值</param>
		/// <returns>砍价订单数据模型</returns>
		BargainOrderInfo ReadBargainOrder(int id);


		/// <summary>
		/// 搜索砍价订单数据列表
		/// </summary>
		/// <param name="mode">BargainOrderSearch模型变量</param>
		/// <returns>砍价订单数据列表</returns>
		List<BargainOrderInfo> SearchBargainOrderList(BargainOrderSearch mode);

		/// <summary>
		/// 搜索砍价订单数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="mode">BargainOrderSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价订单数据列表</returns>
		List<BargainOrderInfo> SearchBargainOrderList(int currentPage, int pageSize,BargainOrderSearch mode,ref int count);
        /// <summary>
        ///  事务操作：保存第一刀金额，分享金额，
        ///  保存帮砍记录金额,砍价参与人数加1
        ///  返回：BargainOrder.Id
        /// </summary>
        /// <returns></returns>
        int CreateBargainOrder(BargainOrderInfo entity, List<decimal> bargain_Moneys);
        /// <summary>
        /// 砍价订单在线付款成功(如果砍到0则不需要支付)
        /// bargaindetails.sales+1 销量+1
        /// bargain.salesvolume+1 销量+1
        /// bargainorder.status=5 支付成功
        /// </summary>
        /// <param name="id">砍价订单Id</param>
        void HandleBargainOrderPay(int id);




    }
}