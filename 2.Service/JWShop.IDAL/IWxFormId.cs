using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 微信 fromid  接口层
    /// </summary>
  public  interface IWxFormId
    {
        int Add(WxFormIdInfo entity);
        /// <summary>
        /// 改变使用状态
        /// </summary>
        /// <param name="id"></param>
        void ChangeUsed(int id);
        /// <summary>
        /// 读取指定用户的有效formid 集合(7天内，未使用)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<WxFormIdInfo> ReadUnusedByUserId(int userId);
    }
}
