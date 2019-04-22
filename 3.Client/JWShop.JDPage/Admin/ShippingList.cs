using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Net;
using System.IO;
using System.Text;

namespace JWShop.Page.Admin
{
    public class ShippingList : AdminBase
    {
        protected string shippingStr = string.Empty;
        protected OrderInfo tempOrder = new OrderInfo();
        protected override void PageLoad()
        {
            base.PageLoad();

            int orderid = RequestHelper.GetQueryString<int>("orderid");

            if (orderid > 0)
            {
                tempOrder = OrderBLL.Read(orderid);
                ShippingInfo tempShipping = ShippingBLL.Read(tempOrder.ShippingId);

                string show = RequestHelper.GetQueryString<string>("show");

                //string apiurl = "http://api.kuaidi100.com/api?id=2815b2d431fdfd26&com=" + typeCom + "&nu=" + nu + "&show=" + show + "&muti=1&order=asc";
                string apiurl = "https://m.kuaidi100.com/index_all.html?type=" + tempShipping.ShippingCode + "&postid=" + tempOrder.ShippingNumber + "&callbackurl=" + "http://" + HttpContext.Current.Request.Url.Host + "/mobileadmin/OrderDetail.html?ID=" + orderid;       
                ////Response.Write (apiurl);
                //WebRequest request = WebRequest.Create(@apiurl);
                //WebResponse response = request.GetResponse();
                //Stream stream = response.GetResponseStream();
                //Encoding encode = Encoding.UTF8;
                //StreamReader reader = new StreamReader(stream, encode);

                //shippingStr = reader.ReadToEnd();
               // Response.Redirect(apiurl);
                Response.Write("<script>window.location.href='"+apiurl+"'</script>");
                Response.End();
            }
        }
    }
}
