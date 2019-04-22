using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 上传实体模型
	/// </summary>
	public sealed  class UploadInfo
	{	  
		private int id;		 
		private int tableID;		 
		private int classID;		 
		private int recordID;		 
		private string uploadName = string.Empty;
		private string otherFile = string.Empty;
		private int size;		 
		private string fileType = string.Empty;
		private string randomNumber = string.Empty;
		private DateTime date = DateTime.Now;
		private string iP = string.Empty;

		public int ID
		{
	   	set {this.id = value;} 
			get {return this.id;}  
		}
		public int TableID
		{
	   	set {this.tableID = value;} 
			get {return this.tableID;}  
		}
		public int ClassID
		{
	   	set {this.classID = value;} 
			get {return this.classID;}  
		}
		public int RecordID
		{
	   	set {this.recordID = value;} 
			get {return this.recordID;}  
		}
		public string UploadName
		{
	   	set {this.uploadName = value;} 
			get {return this.uploadName;}  
		}
		public string OtherFile
		{
	   	set {this.otherFile = value;} 
			get {return this.otherFile;}  
		}
		public int Size
		{
	   	set {this.size = value;} 
			get {return this.size;}  
		}
		public string FileType
		{
	   	set {this.fileType = value;} 
			get {return this.fileType;}  
		}
		public string RandomNumber
		{
	   	set {this.randomNumber = value;} 
			get {return this.randomNumber;}  
		}
		public DateTime Date
		{
	   	set {this.date = value;} 
			get {return this.date;}  
		}
		public string IP
		{
	   	set {this.iP = value;} 
			get {return this.iP;}  
		}
	}
}