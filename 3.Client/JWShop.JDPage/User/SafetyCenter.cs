using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page
{
   public class SafetyCenter:UserBasePage
    {
        //订单状态对应个数
       protected int[] arrT = new int[6];
     
       protected override void PageLoad()
       {
           base.PageLoad();
           #region 订单对应状态个数
           OrderSearchInfo orderSearch = new OrderSearchInfo();
           orderSearch.UserId = base.UserId;
           orderSearch.IsDelete = 0;
           arrT[0] = OrderBLL.SearchList(new OrderSearchInfo { UserId = base.UserId, IsDelete = 0 }).Count;
           orderSearch.OrderStatus = (int)(OrderStatus.WaitPay);
           orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
           arrT[1] = OrderBLL.SearchList(orderSearch).Count;
           orderSearch.OrderStatus = (int)(OrderStatus.Shipping);
           orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
           arrT[2] = OrderBLL.SearchList(orderSearch).Count;
           orderSearch.OrderStatus = (int)(OrderStatus.HasShipping);
           orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
           arrT[3] = OrderBLL.SearchList(orderSearch).Count;
           orderSearch.OrderStatus = (int)(OrderStatus.WaitCheck);
           orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
           arrT[4] = OrderBLL.SearchList(orderSearch).Count;
           orderSearch.OrderStatus = (int)(OrderStatus.NoEffect);
           orderSearch.UserId = base.UserId; orderSearch.IsDelete = 0;
           arrT[5] = OrderBLL.SearchList(orderSearch).Count;
           #endregion
           
        
       }
       /// <summary>
       /// 输出星号遮挡的字符串
       /// </summary>
       /// <param name="originalStr">原始字符串</param>
       /// <returns></returns>
       protected string GetStarString(string originalStr) {
           string result = originalStr;
           if (originalStr.Length == 1)
           {
               result = "*";
           }
           else if (originalStr.Length == 2)
           {
               result = "**";
           }
           else
           { 
               string replaceStr="";
               int replaceLenth=(int)Math.Ceiling(Convert.ToDecimal(originalStr.Length) / 3);
              for(int i=1;i<=replaceLenth;i++){
              replaceStr+="*";
              }
               result = originalStr.Replace(originalStr.Substring((originalStr.Length / 3), replaceLenth), replaceStr);
           }
               return result;
       }
    }
}
