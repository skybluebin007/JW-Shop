using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;

namespace JWShop.Page.Mobile
{
    public class UserAddressAdd : UserBasePage
    {
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected UserAddressInfo userAddress = new UserAddressInfo();

        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("id");
            userAddress = UserAddressBLL.Read(id, base.UserId);

            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.ClassID = userAddress.RegionId;
            
            Title = "收货地址";
        }

    }
}