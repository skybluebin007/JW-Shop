using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWShop.Entity;

namespace JWShop.XcxApi
{
    public class ProductVirtualModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public string img { set; get; }
        public string imgbig { set; get; }
        public string imgorg { set; get; }
        public decimal price { set; get; }
        //市场价
        public decimal marketprice { get; set; }
        public int click { set; get; }
        public int like { set; get; }
        public int totalstore { set; get; }
        public int ordercount { set; get; }
        public int commentcount { set; get; }
        public double goodcompercent { set; get; }
        public int buycount { set; get; }
        /// <summary>
        /// 小程序推广码
        /// </summary>
        public string qrcode { get; set; }
        /// <summary>
        /// 拼团图片
        /// </summary>
       public string groupphoto { get; set; }
    }

    public class VirtualProductCommend
    {
        public int id { set; get; }
        public string name { set; get; }
        public string avator { set; get; }
        public int lv { set; get; }
        public DateTime date { set; get; }
        public string content { set; get; }
        public List<ProductPhotoInfo> imglist { set; get; }
        /// <summary>
        /// 管理员回复内容
        /// </summary>
        public string adminreply { get; set; }
        /// <summary>
        /// 管理员回复时间
        /// </summary>
        public DateTime replydate { get; set; }
    }

    public class VirtualAddress
    {
        public int id { set; get; }
        public string name { set; get; }
        public string address { set; get; }
        public string mobile { set; get; }
        public bool isdefault { set; get; }
        public int userid { set; get; }
        public string regionid { set; get; }
        public string regionnames { set; get; }
    }

    //public class VirtualUser
    //{
    //    public int id { set; get; }
    //    public string name { set; get; }
    //    public string photo { set; get; }
    //    public string openid { set; get; }
    //    public string avatar { set; get; }
    //}

    public class VirtualOrder
    {
        public int id { set; get; }
        public string ordernum { set; get; }
        public string orderstatus { set; get; }
        public decimal productmoney { set; get; }
        public decimal shippingmoney { set; get; }
        public decimal couponmoney { set; get; }
        public decimal favorablemoney { get; set; }
        public decimal pointmoney { get; set; }
        public decimal othermoney { get; set; }
        public string ordernote { get; set; }
        public decimal realpay { set; get; }
        public bool iscommended { set; get; }
        public List<ProductVirtualModel> proarr { set; get; }
        public string usermessage { set; get; }
        public DateTime orderdate { set; get; }
        public string address { set; get; }
        public VirtualAddress addinfo { set; get; }
        public bool canrefund { get; set; }
        public int isactivity { get; set; }
        public int favorableactivityid { get; set; }
        public int selfpick { get; set; }
    }

    public class VirtualCoupon
    {
        public int id { set; get; }
        public string name { set; get; }
        public decimal money { set; get; }
        public decimal minmoney { set; get; }
        public DateTime startdate { set; get; }
        public DateTime enddate { set; get; }
        public int isused { set; get; }

    }
}
