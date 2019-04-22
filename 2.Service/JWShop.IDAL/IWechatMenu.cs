using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;


namespace JWShop.IDAL
{
    /// <summary>
    /// 微信菜单接口层说明。
    /// </summary>
    public interface IWechatMenu
    {
         int Add(WechatMenuInfo entity);
        
         void Update(WechatMenuInfo entity);
       

         void Delete(int id);
        

         WechatMenuInfo Read(int id);
        
         List<WechatMenuInfo> ReadList();

         List<WechatMenuInfo> ReadChildList(int fatherid);
         /// <summary>
         /// 上移图片
         /// </summary>
         /// <param name="id">要移动的id</param>
         void MoveUpWechatMenu(int id);
       

         /// <summary>
         /// 下移图片
         /// </summary>
         /// <param name="id">要移动的id</param>
         void MoveDownWechatMenu(int id);
        

    }
}
