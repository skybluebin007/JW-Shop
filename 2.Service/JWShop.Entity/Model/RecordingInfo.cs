using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价记录表实体模型
	/// </summary>
	public sealed  class RecordingInfo
	{	  
		private int id;		 
		private int bOrderId;		 
		private decimal price;		 
		private string content = string.Empty;
		private string photo = string.Empty;
		private DateTime addDate = DateTime.Now;
		private int userId;		 
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
		public string Content
        {
	   	set {this.content = value;} 
			get {return this.content;}  
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
	}
}