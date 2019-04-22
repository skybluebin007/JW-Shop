using Dapper.Contrib.Extensions;
using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价实体模型
	/// </summary>
	public sealed  class BargainInfo
	{	  
		private int id;		 
		private DateTime startDate = DateTime.Now;
		private DateTime endDate = DateTime.Now;
		private int limitCount;		 
		private int numberPeople;		 
		private string activityRules = string.Empty;
		private int successRate;
        private string name = string.Empty;
        private int salesVolume;
        private int number;
        /// <summary>
        /// 状态：-1:已结束  0-已关闭/取消  1-进行中（有效）
        /// </summary>
        public int Status { get; set; }
        public int Id
		{
	   	set {this.id = value;} 
			get {return this.id;}  
		}
		public DateTime StartDate
		{
	   	set {this.startDate = value;} 
			get {return this.startDate;}  
		}
		public DateTime EndDate
		{
	   	set {this.endDate = value;} 
			get {return this.endDate;}  
		}
		public int LimitCount
		{
	   	set {this.limitCount = value;} 
			get {return this.limitCount;}  
		}
		public int NumberPeople
		{
	   	set {this.numberPeople = value;} 
			get {return this.numberPeople;}  
		}
		public string ActivityRules
		{
	   	set {this.activityRules = value;} 
			get {return this.activityRules;}  
		}
		public int SuccessRate
		{
	   	set {this.successRate = value;} 
			get {return this.successRate;}  
		}

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int SalesVolume
        {
            get
            {
                return salesVolume;
            }

            set
            {
                salesVolume = value;
            }
        }

        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }
        /// <summary>
        /// 虚拟属性：日期状态 -1未开始 0-进行中 1-已结束
        /// </summary>
        [Computed]
        public int BargainDateStatus { get; set; }
        /// <summary>
        /// 虚拟属性：本活动共砍次数
        /// </summary>
        [Computed]
        public int Bargain_Records_Total { get; set; }
    }
}