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
	/// 砍价详情业务逻辑。
	/// </summary>
	public sealed class BargainDetailsBLL
	{
		private static readonly IBargainDetails dal = FactoryHelper.Instance<IBargainDetails>(Global.DataProvider,"BargainDetailsDAL");

		/// <summary>
		/// 增加一条砍价详情数据
		/// </summary>
		/// <param name="model">砍价详情模型变量</param>
		public static int AddBargainDetails(BargainDetailsInfo model)
		{
			model.Id = dal.AddBargainDetails(model);
			return model.Id;
		}

		/// <summary>
		/// 更新一条砍价详情数据
		/// </summary>
		/// <param name="model">砍价详情模型变量</param>
		public static void UpdateBargainDetails(BargainDetailsInfo model)
		{
			dal.UpdateBargainDetails(model);
		} 

		/// <summary>
		/// 删除多条砍价详情数据
		/// </summary>
		/// <param name="strId">砍价详情的主键值,以,号分隔</param>
		public static void DeleteBargainDetails(string strId)
		{
			dal.DeleteBargainDetails(strId);
		}


        public static List<BargainDetailsInfo> ReadList()
        {
            return dal.ReadList();
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(int id)
        {
            dal.Delete(id);
        }

		/// <summary>
		/// 读取一条砍价详情数据
		/// </summary>
		/// <param name="id">砍价详情的主键值</param>
		/// <returns>砍价详情数据模型</returns>
		public static BargainDetailsInfo ReadBargainDetails(int id)
		{
			return dal.ReadBargainDetails(id);
		}


        /// <summary>
        /// 读取活动下面的所有商品
        /// </summary>
        /// <param name="BargainId"></param>
        /// <returns></returns>
        public static List<BargainDetailsInfo> ReadByBargainId(int BargainId)
        {
            return dal.ReadByBargainId(BargainId);
        }
        /// <summary>
        /// 读取正在参加砍价的商品列表(Id,Name)
        /// </summary>
        /// <returns></returns>
        public static List<BargainDetailsInfo> ReadBargainProducts()
        {
            return dal.ReadBargainProducts();
        }
        /// <summary>
        /// 获取参与砍价的用户姓名 头像
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<BargainUserInfo> GetBargainUsers(int id)
        {
            return dal.GetBargainUsers(id);
        }
    }
}