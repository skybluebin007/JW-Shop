using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价详情实体模型
	/// </summary>
	public sealed  class BargainDetailsInfo
	{	  
		private int id;		 
		private int bargainId;		 
		private int productID;		 
		private int stock;		 
		private int sort;		 
		private decimal reservePrice;
        private int sales;
        private string productName=string.Empty;

        public int Id
		{
	   	set {this.id = value;} 
			get {return this.id;}  
		}
		public int BargainId
		{
	   	set {this.bargainId = value;} 
			get {return this.bargainId;}  
		}
		public int ProductID
		{
	   	set {this.productID = value;} 
			get {return this.productID;}  
		}
		public int Stock
		{
	   	set {this.stock = value;} 
			get {return this.stock;}  
		}
		public int Sort
		{
	   	set {this.sort = value;} 
			get {return this.sort;}  
		}
		public decimal ReservePrice
		{
	   	set {this.reservePrice = value;} 
			get {return this.reservePrice;}  
		}

        public int Sales
        {
            get
            {
                return sales;
            }

            set
            {
                sales = value;
            }
        }

        public string ProductName
        {
            get
            {
                return productName;
            }

            set
            {
                productName = value;
            }
        }
        /// <summary>
        /// 小程序砍价分享图片1
        /// </summary>
        public string ShareImage1 { get; set; }
        /// <summary>
        /// 小程序砍价分享图片2
        /// </summary>
        public string ShareImage2 { get; set; }
        /// <summary>
        /// 小程序砍价分享图片3
        /// </summary>
        public string ShareImage3 { get; set; }
        #region 虚拟属性
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }       
        /// <summary>
        /// 虚拟属性：本活动共砍次数
        /// </summary>
        [Computed]
        public int Bargain_Records_Total { get; set; }
        [Computed]
        public List<BargainUserInfo> BargainUserList { get; set; }
        /// <summary>
        /// 虚拟销量
        /// </summary>
        [Computed]
        public int Virtual_Sales { get; set; }
        #endregion
    }
    /// <summary>
    /// 参与砍价用户
    /// </summary>
    public sealed class BargainUserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
}