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
	/// 砍价记录表业务逻辑。
	/// </summary>
	public sealed class RecordingBLL
	{
		private static readonly IRecording dal = FactoryHelper.Instance<IRecording>(Global.DataProvider,"RecordingDAL");

		/// <summary>
		/// 增加一条砍价记录表数据
		/// </summary>
		/// <param name="mode">砍价记录表模型变量</param>
		public static int AddRecording(RecordingInfo mode)
		{
			mode.Id = dal.AddRecording(mode);
			return mode.Id;
		}

		/// <summary>
		/// 更新一条砍价记录表数据
		/// </summary>
		/// <param name="mode">砍价记录表模型变量</param>
		public static void UpdateRecording(RecordingInfo mode)
		{
			dal.UpdateRecording(mode);
		} 

		/// <summary>
		/// 删除多条砍价记录表数据
		/// </summary>
		/// <param name="strId">砍价记录表的主键值,以,号分隔</param>
		public static void DeleteRecording(string strId)
		{
			dal.DeleteRecording(strId);
		}




		/// <summary>
		/// 读取一条砍价记录表数据
		/// </summary>
		/// <param name="id">砍价记录表的主键值</param>
		/// <returns>砍价记录表数据模型</returns>
		public static RecordingInfo ReadRecording(int id)
		{
			return dal.ReadRecording(id);
		}



		/// <summary>
		/// 搜索砍价记录表数据列表
		/// </summary>
		/// <param name="mode">RecordingSearch模型变量</param>
		/// <returns>砍价记录表数据列表</returns>
		public static List<RecordingInfo> SearchRecordingList(RecordingSearch mode)
		{
			return dal.SearchRecordingList(mode);
		}
		
		/// <summary>
		/// 搜索砍价记录表数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="mode">RecordingSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价记录表数据列表</returns>
		public static List<RecordingInfo> SearchRecordingList(int currentPage, int pageSize,RecordingSearch mode,ref int count)
		{
			return dal.SearchRecordingList(currentPage, pageSize,mode,ref count);
		}

        /// <summary>
        /// 获取用户当天砍价次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetRecordingCountByUser(int userId)
        {
            return dal.GetRecordingCountByUser(userId);
        }

        /// <summary>
        /// 用户帮砍操作
        /// 帮砍成功之后更新BargainOrder
        /// 返回帮砍记录Id
        /// </summary>
        /// <param name="bargainOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int HelpBargain(BargainOrderInfo bargainOrder, UserInfo user)
        {
            return dal.HelpBargain(bargainOrder, user);
        }

    }
}