using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;
using SkyCES.EntLib;
using Newtonsoft.Json;

namespace JWShop.Web.Admin
{
    public partial class WriteOffPickUpCode : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (RequestHelper.GetQueryString<string>("action").ToLower() == "searchorder") SearchOrder();
            }
        }
        /// <summary>
        /// 根据提货码查询订单
        /// </summary>
        private void SearchOrder()
        {
            string pickCode = RequestHelper.GetQueryString<string>("pickUpCode");
            int checkCode = 0;
            OrderInfo order= PickUpCodeBLL.ReadByPickCode(pickCode,ref checkCode);
            if (checkCode == 1)
            {
                Response.Write(JsonConvert.SerializeObject(new { ok=true,msg="",order=order}));
                Response.End();
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的提货码" }));
                Response.End();
            }
        }

    }
}