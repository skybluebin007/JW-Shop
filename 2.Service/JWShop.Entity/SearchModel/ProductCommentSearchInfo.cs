using System;

namespace JWShop.Entity
{
	/// <summary>
	/// 商品评论搜索模型。
	/// </summary>
	public sealed  class ProductCommentSearchInfo
	{
        private string title = string.Empty;
        private string content = string.Empty;
        private string userIP = string.Empty;
        private DateTime startPostDate = DateTime.MinValue;
        private DateTime endPostDate = DateTime.MinValue;
        private int status = int.MinValue;
        private int userId = int.MinValue;
        private int productId = int.MinValue;
        private string productName = string.Empty;
        private int orderID = int.MinValue;
        private string rank = string.Empty;
        public string Title
        {
            set { this.title = value; }
            get { return this.title; }
        }
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }
        public string UserIP
        {
            set { this.userIP = value; }
            get { return this.userIP; }
        }
        public DateTime StartPostDate
        {
            set { this.startPostDate = value; }
            get { return this.startPostDate; }
        }
        public DateTime EndPostDate
        {
            set { this.endPostDate = value; }
            get { return this.endPostDate; }
        }
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        public int UserId
        {
            set { this.userId = value; }
            get { return this.userId; }
        }
        public int ProductId
        {
            set { this.productId = value; }
            get { return this.productId; }
        }
        public string ProductName
        {
            set { this.productName = value; }
            get { return this.productName; }
        }
        public int OrderID
        {
            set { this.orderID = value; }
            get { return this.orderID; }
        }
        public string Rank
        {
            set { this.rank = value; }
            get { return this.rank; }
        }
	}
}

