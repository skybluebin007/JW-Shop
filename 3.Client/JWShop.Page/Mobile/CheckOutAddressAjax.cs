using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Mobile
{
    public class CheckOutAddressAjax : AjaxBasePage
    {       
        /// <summary>
        /// 单一无限极分类
        /// </summary>
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        /// <summary>
        /// 用户地址
        /// </summary>
        protected UserAddressInfo userAddress = new UserAddressInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.FunctionName = "readShippingList()";
            int id = RequestHelper.GetQueryString<int>("ID");
            if (id > 0)
            {
                userAddress = UserAddressBLL.Read(id, base.UserId);
                singleUnlimitClass.ClassID = userAddress.RegionId;
            }
        }
    }
}
