using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 砍价记录表接口层说明。
	/// </summary>
	public interface IRecording
	{
		/// <summary>
		/// 增加一条砍价记录表数据
		/// </summary>
		/// <param name="mode">砍价记录表模型变量</param>
		int AddRecording(RecordingInfo mode);

		/// <summary>
		/// 更新一条砍价记录表数据
		/// </summary>
		/// <param name="mode">砍价记录表模型变量</param>
		void UpdateRecording(RecordingInfo mode);	 

		/// <summary>
		/// 删除多条砍价记录表数据
		/// </summary>
		/// <param name="strId">砍价记录表的主键值,以,号分隔</param>
		void DeleteRecording(string strId);

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

		/// <summary>
		/// 读取一条砍价记录表数据
		/// </summary>
		/// <param name="id">砍价记录表的主键值</param>
		/// <returns>砍价记录表数据模型</returns>
		RecordingInfo ReadRecording(int id);


		/// <summary>
		/// 搜索砍价记录表数据列表
		/// </summary>
		/// <param name="mode">RecordingSearch模型变量</param>
		/// <returns>砍价记录表数据列表</returns>
		List<RecordingInfo> SearchRecordingList(RecordingSearch mode);

		/// <summary>
		/// 搜索砍价记录表数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="mode">RecordingSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价记录表数据列表</returns>
		List<RecordingInfo> SearchRecordingList(int currentPage, int pageSize,RecordingSearch mode,ref int count);


        /// <summary>
        /// 通过活动商品读取记录
        /// </summary>
        /// <param name="BDetailId"></param>
        /// <returns></returns>
        //List<RecordingInfo> ReadByBDetailId(int BDetailId);

        /// <summary>
        /// 获取用户当天砍价次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetRecordingCountByUser(int userId);
        /// <summary>
        /// 用户帮砍操作
        /// 帮砍成功之后更新BargainOrder
        /// 返回帮砍记录Id
        /// </summary>
        /// <param name="bargainOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        int HelpBargain(BargainOrderInfo bargainOrder, UserInfo user);
    }
}