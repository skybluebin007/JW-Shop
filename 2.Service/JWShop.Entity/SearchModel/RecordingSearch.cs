using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价记录表搜索模型。
	/// </summary>
	public sealed  class RecordingSearch
	{	  
		private int id = int.MinValue;		 
		private int bOrderId = int.MinValue;		 
		private decimal price =decimal.MinValue;
		private string name = string.Empty;
		private string photo = string.Empty;
		private DateTime addDate =DateTime.MinValue;
		private int userId = int.MinValue;		 
		private string userName = string.Empty;

		public int Id
		{
	   	set {this.id = value;} 
			get {return this.id;}  
		}
		public int BOrderId
        {
	   	set {this.bOrderId = value;} 
			get {return this.bOrderId; }  
		}
		public decimal Price
		{
	   	set {this.price = value;} 
			get {return this.price;}  
		}
		public string Name
		{
	   	set {this.name = value;} 
			get {return this.name;}  
		}
		public string Photo
		{
	   	set {this.photo = value;} 
			get {return this.photo;}  
		}
		public DateTime AddDate
		{
	   	set {this.addDate = value;} 
			get {return this.addDate;}  
		}
		public int UserId
		{
	   	set {this.userId = value;} 
			get {return this.userId;}  
		}
		public string UserName
		{
	   	set {this.userName = value;} 
			get {return this.userName;}  
		}
        public string InBOrderId { get; set; }
    }
}

