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
using Newtonsoft.Json;
using System.Linq;

namespace JWShop.Page.Mobile
{
   public class MapApi:CommonBasePage
    {
        #region 微信分享      
        protected string str = string.Empty;
        protected string timestamp = string.Empty;
        protected string nonce = string.Empty;
        protected string signature = string.Empty;
        protected string WeChatImg = string.Empty;
        protected string url = string.Empty;
        protected string title = string.Empty;
        protected string desc = string.Empty;
        #endregion
        protected override void PageLoad()
        {
            base.PageLoad();
            #region 微信分享
            Hashtable ht = new Hashtable();
            WechatCommon wxs = new WechatCommon();
            ht = wxs.getSignPackage();
            timestamp = ht["timestamp"].ToString();
            nonce = ht["nonceStr"].ToString();
            signature = ht["signature"].ToString();
            url = ht["url"].ToString();

            #endregion
            if (RequestHelper.GetQueryString<string>("action").Equals("GetHospitals")) GetHospitals();
        }
        /// <summary>
        /// 获取所有门店的地址信息
        /// </summary>
        protected void GetHospitals()
        {
            List<AddressInfo> addressList = new List<AddressInfo>();
            addressList.Add (new AddressInfo(){ Id=1,Title= "中房", Address= "咸嘉湖西路",Tel="12345678", Longitude= 112.8927010000, Latitude= 28.2084820000 });
            addressList.Add(new AddressInfo() { Id = 2, Title = "柏家塘", Address = "枫林三路", Tel = "9897654321", Longitude = 112.8962940000, Latitude = 28.2041850000 });
            addressList.Add(new AddressInfo() { Id = 3, Title = "梅溪湖", Address = "梅溪湖路", Tel = "888888888", Longitude = 112.8893240000, Latitude = 28.1910370000 });
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(from item in addressList select new { id = item.Id, name = item.Title, address = item.Address, tel = item.Tel, lng = item.Longitude, lat = item.Latitude }));
            Response.End();
        }
        protected class AddressInfo {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Address { get; set; }
            public string Tel { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
    }
}
