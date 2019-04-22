using System;using System.Data;using System.Collections;using System.Collections.Generic;using JWShop.Common;using JWShop.Entity;using JWShop.IDAL;using SkyCES.EntLib;namespace JWShop.Business{	/// <summary>	/// 砍价业务逻辑。	/// </summary>	public sealed class BargainBLL:BaseBLL	{		private static readonly IBargain dal = FactoryHelper.Instance<IBargain>(Global.DataProvider,"BargainDAL");		/// <summary>		/// 增加一条砍价数据		/// </summary>		/// <param name="model">砍价模型变量</param>		public static int AddBargain(BargainInfo model)		{			model.Id = dal.AddBargain(model);			return model.Id;		}		/// <summary>		/// 更新一条砍价数据		/// </summary>		/// <param name="model">砍价模型变量</param>		public static void UpdateBargain(BargainInfo model)		{			dal.UpdateBargain(model);		} 		/// <summary>		/// 删除多条砍价数据		/// </summary>		/// <param name="strId">砍价的主键值,以,号分隔</param>		public static void DeleteBargain(string strId)		{			dal.DeleteBargain(strId);		}        /// <summary>
        /// 删除一条数据
        /// </summary>        public static void Delete(int id)
        {
            dal.Delete(id);
        }		/// <summary>		/// 读取一条砍价数据		/// </summary>		/// <param name="id">砍价的主键值</param>		/// <returns>砍价数据模型</returns>		public static BargainInfo ReadBargain(int id)		{			return dal.ReadBargain(id);		}

        /// <summary>        /// 搜索砍价数据列表        /// </summary>        /// <param name="model">BargainSearch模型变量</param>        /// <returns>砍价数据列表</returns>        public static List<BargainInfo> SearchBargainList(BargainSearch model)		{			return dal.SearchBargainList(model);		}				/// <summary>		/// 搜索砍价数据列表		/// </summary>		/// <param name="currentPage">当前的页数</param>		/// <param name="pageSize">每页记录数</param>		/// <param name="model">BargainSearch模型变量</param>		/// <param name="count">总数量</param>		/// <returns>砍价数据列表</returns>		public static List<BargainInfo> SearchBargainList(int currentPage, int pageSize,BargainSearch model,ref int count)		{			return dal.SearchBargainList(currentPage, pageSize,model,ref count);		}        /// <summary>
        /// 读取正在进行且有效的活动
        /// </summary>
        /// <returns></returns>        public static List<BargainInfo> View_ReadBargain()
        {
            return dal.View_ReadBargain();
        }
        /// <summary>
        /// 关闭活动事务：关闭活动，将未支付成功的砍价全部置为“砍价失败”，原“活动已取消，砍价失败”
        /// </summary>
        /// <param name="id"></param>
        public static void ChangeBargainStatus(int id,int status)
        {
            if (status == (int)Bargain_Status.ShutDown || status == (int)Bargain_Status.End)
            {
                dal.ChangeBargainStatus(id, status);
            }
        }
    }}