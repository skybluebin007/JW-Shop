using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 商品回复实体模型
	/// </summary>
	public sealed  class ProductReplyInfo
	{	  
		private int id;		 
		private int productID;		 
		private int commentID;		 
		private string content = string.Empty;
		private string userIP = string.Empty;
		private DateTime postDate = DateTime.Now;
		private int userID;		 
		private string userName = string.Empty;

		public int ID
		{
	   	set {this.id = value;} 
			get {return this.id;}  
		}
		public int ProductID
		{
	   	set {this.productID = value;} 
			get {return this.productID;}  
		}
		public int CommentID
		{
	   	set {this.commentID = value;} 
			get {return this.commentID;}  
		}
		public string Content
		{
	   	set {this.content = value;} 
			get {return this.content;}  
		}
		public string UserIP
		{
	   	set {this.userIP = value;} 
			get {return this.userIP;}  
		}
		public DateTime PostDate
		{
	   	set {this.postDate = value;} 
			get {return this.postDate;}  
		}
		public int UserID
		{
	   	set {this.userID = value;} 
			get {return this.userID;}  
		}
		public string UserName
		{
	   	set {this.userName = value;} 
			get {return this.userName;}  
		}
	}
}