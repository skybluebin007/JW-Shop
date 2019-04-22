using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
	/// <summary>
	/// 链接接口层说明。
	/// </summary>
	public interface ILink
	{
        /// <summary>
        /// 增加一条链接数据
        /// </summary>
        /// <param name="link">链接模型变量</param>
        int AddLink(LinkInfo link);

        /// <summary>
        /// 更新一条链接数据
        /// </summary>
        /// <param name="link">链接模型变量</param>
        void UpdateLink(LinkInfo link);

        /// <summary>
        /// 删除多条链接数据
        /// </summary>
        /// <param name="strID">链接的主键值,以,号分隔</param>
        void DeleteLink(string strID);


        /// <summary>
        /// 获得链接数据列表
        /// </summary>
        /// <returns>链接数据列表</returns>
        List<LinkInfo> ReadLinkAllList();


        /// <summary>
        /// 移动链接数据排序
        /// </summary>
        /// <param name="action">改变动作,向上:ChangeAction.Up;向下:ChangeActon.Down</param>
        /// <param name="id">当前记录的主键值</param>
        void ChangeLinkOrder(ChangeAction action, int id);


	}
}