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
using System.Linq;
using Newtonsoft.Json;

namespace JWShop.Page.Mobile
{
   public class PointProduct:CommonBasePage
    {     
        protected List<PointProductInfo> pointProductList = new List<PointProductInfo>();
        protected UserInfo CurrentUser = new UserInfo();
       protected int pageSize = 1;
        protected override void PageLoad()
        {
            base.PageLoad();
            if (base.UserId > 0) CurrentUser = UserBLL.ReadUserMore(base.UserId);
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            pageSize = 10;
            int count = 0;

            pointProductList = PointProductBLL.SearchList(currentPage, pageSize, new PointProductSearchInfo { IsSale = (int)BoolType.True, ValidDate = DateTime.Now }, ref count);               

            Title = "积分兑换商品";
            if (RequestHelper.GetQueryString<string>("Action") == "LoadProducts") this.LoadProdcuts();
        }
        protected void LoadProdcuts() {
            int pageNum = RequestHelper.GetQueryString<int>("pageNum");
            int pageSize = RequestHelper.GetQueryString<int>("pageSize");
            int count = 0;
            List<PointProductInfo> _pointProductList = new List<PointProductInfo>();
            if (pageNum > 1) {
              var  resultlist = PointProductBLL.SearchList(pageNum, pageSize, new PointProductSearchInfo { IsSale = (int)BoolType.True, ValidDate = DateTime.Now }, ref count);
              foreach (PointProductInfo item in resultlist) {
                  item.Photo = item.Photo.Replace("Original","400-300");
                  _pointProductList.Add(item);
              }
            }
            Response.Clear();
            ResponseHelper.Write(JsonConvert.SerializeObject(new {count=_pointProductList.Count,dataList=_pointProductList }));
            ResponseHelper.End();
        }
    }
}
