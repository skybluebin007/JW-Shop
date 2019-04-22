using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using System.Web.Security;
using Newtonsoft.Json;

namespace JWShop.Page.Admin
{
  public  class ShippingOrder:AdminBase
    {
        protected OrderInfo order = new OrderInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("id");
            order = OrderBLL.Read(id);

            topNav = 0;

            if (RequestHelper.GetForm<string>("action").ToLower() == "shipping") Ship();
        }
        /// <summary>
        /// 发货
        /// </summary>
        protected void Ship()
        {
            bool flag = true;
            string msg = string.Empty;
           
            try
            {
                int orderId = RequestHelper.GetForm<int>("orderid");
                DateTime date = RequestHelper.GetForm<DateTime>("date");
                string shippingnumber = RequestHelper.GetForm<string>("shippingnumber");
                if (orderId <= 0)
                {
                    flag = false;
                    msg = "请求参数错误";
                }
                if ((date - DateTime.Now).Days<0)
                {
                    flag = false;
                    msg = "配送日期不规范" ;
                    
                }
                if (string.IsNullOrEmpty(shippingnumber))
                {
                    flag = false;
                    msg = "配送单号不能为空" ;
                    
                }
                OrderInfo order = OrderBLL.Read(orderId);
                if (order.OrderStatus == (int)OrderStatus.Shipping)
                {
                    int startOrderStatus = order.OrderStatus;
                    order.OrderStatus = (int)OrderStatus.HasShipping;
                    order.ShippingNumber = shippingnumber;
                    order.ShippingDate = date;
                    //更新商品库存数量
                    ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);
                    OrderBLL.AdminUpdateOrderAddAction(order, "", (int)OrderOperate.Send, startOrderStatus);
                    flag = true;
                  
                }

            }
            catch (Exception ex)
            {
                flag = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = flag, msg = msg }));
                Response.End();
            }
        }
    }
}
