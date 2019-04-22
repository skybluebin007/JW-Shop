using Dapper.Contrib.Extensions;
using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价订单实体模型
	/// </summary>
	public sealed  class BargainOrderInfo
	{	  
		private int id;		 
		private int userId;		 
		private int status;		 
		private int bargainDetailsId;		 
		private decimal bargainPrice;		 
		private decimal sharePrice;		 
		private int shareStatus;
        private int bargainId;
        //备注
        public string Remark { get; set; }
        public int Id
		{
	   	set {this.id = value;} 
			get {return this.id;}  
		}
		public int UserId
		{
	   	set {this.userId = value;} 
			get {return this.userId;}  
		}
		public int Status
		{
	   	set {this.status = value;} 
			get {return this.status;}  
		}
		public int BargainDetailsId
		{
	   	set {this.bargainDetailsId = value;} 
			get {return this.bargainDetailsId;}  
		}
		public decimal BargainPrice
		{
	   	set {this.bargainPrice = value;} 
			get {return this.bargainPrice;}  
		}
		public decimal SharePrice
		{
	   	set {this.sharePrice = value;} 
			get {return this.sharePrice;}  
		}
		public int ShareStatus
		{
	   	set {this.shareStatus = value;} 
			get {return this.shareStatus;}  
		}

        public int BargainId
        {
            get
            {
                return bargainId;
            }

            set
            {
                bargainId = value;
            }
        }
        #region 虚拟属性
        /// <summary>
        /// 发起砍价用户名
        /// </summary>
        [Computed]
        public string UserName { get; set; }
        /// <summary>
        /// 发起砍价用户头像
        /// </summary>
        [Computed]
        public string UserPhoto { get; set; }
        /// <summary>
        /// 限砍几次
        /// </summary>
        [Computed]
        public int Total_Num { get; set; }
        /// <summary>
        /// 已砍几刀
        /// </summary>
        [Computed]
        public int Has_Bargain_Num { get; set; }
        /// <summary>
        /// 总需砍金额
        /// </summary>
        [Computed]
        public decimal Total_Money { get; set; }
        /// <summary>
        /// 总已砍金额
        /// </summary>
        [Computed]
        public decimal Has_Bargain_Money { get; set; }   
        #endregion

    }
}