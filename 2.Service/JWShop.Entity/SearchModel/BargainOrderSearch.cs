using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价订单搜索模型。
	/// </summary>
	public sealed  class BargainOrderSearch
	{	  
		private int id = int.MinValue;		 
		private int userId = int.MinValue;		 
		private int status = int.MinValue;		 
		private int bargainDetailsId = int.MinValue;		 
		private decimal bargainPrice =decimal.MinValue;
		private decimal sharePrice =decimal.MinValue;
		private int shareStatus = int.MinValue;		 

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
	}
}

