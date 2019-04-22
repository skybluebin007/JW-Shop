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

namespace JWShop.Web.Admin
{
    public partial class FavorableAjax : JWShop.Page.AdminBasePage
    {
        protected int typeID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int favorID = RequestHelper.GetQueryString<int>("favorId");
             typeID = RequestHelper.GetQueryString<int>("typeId");
            int changeType = RequestHelper.GetQueryString<int>("changeType");
            if (typeID<= (int)FavorableType.AllOrders)
            {
            
                RegionID.DataSource = RegionBLL.ReadRegionUnlimitClass();
                RegionID.DataBind();
            }
            if (typeID == (int)FavorableType.ProductClass)
            {
              
                RegionID.DataSource = ProductClassBLL.ReadUnlimitClassList();
                RegionID.DataBind();
            }
            //如果是修改,而且没改变优惠类型
            if (favorID > 0 && changeType<=0)
            {
                RegionID.ClassIDList = FavorableActivityBLL.Read(favorID).RegionId;
            }
           
        }
    }
}