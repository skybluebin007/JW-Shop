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
	/// 砍价订单业务逻辑。
	/// </summary>
	public sealed class BargainOrderBLL
	{
		private static readonly IBargainOrder dal = FactoryHelper.Instance<IBargainOrder>(Global.DataProvider,"BargainOrderDAL");

		/// <summary>
		/// 增加一条砍价订单数据
		/// </summary>
		/// <param name="mode">砍价订单模型变量</param>
		public static int AddBargainOrder(BargainOrderInfo mode)
		{
			mode.Id = dal.AddBargainOrder(mode);
			return mode.Id;
		}

		/// <summary>
		/// 更新一条砍价订单数据
		/// </summary>
		/// <param name="mode">砍价订单模型变量</param>
		public static bool  UpdateBargainOrder(BargainOrderInfo mode)
		{
		return	dal.UpdateBargainOrder(mode);
		} 

		/// <summary>
		/// 删除多条砍价订单数据
		/// </summary>
		/// <param name="strId">砍价订单的主键值,以,号分隔</param>
		public static void DeleteBargainOrder(string strId)
		{
			dal.DeleteBargainOrder(strId);
		}

        public static void Delete(int id)
        {
            dal.Delete(id);
        }


		/// <summary>
		/// 读取一条砍价订单数据
		/// </summary>
		/// <param name="id">砍价订单的主键值</param>
		/// <returns>砍价订单数据模型</returns>
		public static BargainOrderInfo ReadBargainOrder(int id)
		{
			return dal.ReadBargainOrder(id);
		}



		/// <summary>
		/// 搜索砍价订单数据列表
		/// </summary>
		/// <param name="mode">BargainOrderSearch模型变量</param>
		/// <returns>砍价订单数据列表</returns>
		public static List<BargainOrderInfo> SearchBargainOrderList(BargainOrderSearch mode)
		{
			return dal.SearchBargainOrderList(mode);
		}
		
		/// <summary>
		/// 搜索砍价订单数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="mode">BargainOrderSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价订单数据列表</returns>
		public static List<BargainOrderInfo> SearchBargainOrderList(int currentPage, int pageSize,BargainOrderSearch mode,ref int count)
		{
			return dal.SearchBargainOrderList(currentPage, pageSize,mode,ref count);
		}
        /// <summary>
        ///  事务操作：保存第一刀金额，分享金额，
        ///  保存帮砍记录金额,砍价参与人数加1
        ///  返回：BargainOrder.Id
        /// </summary>
        /// <returns></returns>
        public static int CreateBargainOrder(BargainOrderInfo entity, List<decimal> bargain_Moneys)
        {
            return dal.CreateBargainOrder(entity, bargain_Moneys);
        }
        /// <summary>
        /// 砍价订单在线付款成功(如果砍到0则不需要支付)
        /// bargaindetails.sales+1 销量+1
        /// bargain.salesvolume+1 销量+1
        /// bargainorder.status=5 支付成功
        /// </summary>
        /// <param name="id">砍价订单Id</param>
        public static void HandleBargainOrderPay(int id)
        {
            dal.HandleBargainOrderPay(id);
        }


    }
}