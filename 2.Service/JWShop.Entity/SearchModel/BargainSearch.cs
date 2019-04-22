using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 砍价搜索模型。
	/// </summary>
	public sealed  class BargainSearch
	{	  
		private int id = int.MinValue;		 
		private DateTime startDate =DateTime.MinValue;
		private DateTime endDate =DateTime.MinValue;
		private int limitCount = int.MinValue;		 
		private int numberPeople = int.MinValue;		 
		private string activityRules = string.Empty;
		private int successRate = int.MinValue;
        private string name = string.Empty;

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
    }
}

