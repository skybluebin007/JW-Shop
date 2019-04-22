using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 文章搜索模型。
    /// </summary>
    public sealed class ArticleSearchInfo
    {
        private string title = string.Empty;
        private string classId = string.Empty;
        private int isTop = int.MinValue;
        private string author = string.Empty;
        private string resource = string.Empty;
        private string keywords = string.Empty;
        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MinValue;
        private int allowComment = int.MinValue;
        private string inArticleId = string.Empty;

        private int parentId = int.MinValue;
        public string InClassId { get; set; }
        public string Title
        {
            set { this.title = value; }
            get { return this.title; }
        }
        public string ClassId
        {
            set { this.classId = value; }
            get { return this.classId; }
        }
        public int IsTop
        {
            set { this.isTop = value; }
            get { return this.isTop; }
        }
        public string Author
        {
            set { this.author = value; }
            get { return this.author; }
        }
        public string Resource
        {
            set { this.resource = value; }
            get { return this.resource; }
        }
        public string Keywords
        {
            set { this.keywords = value; }
            get { return this.keywords; }
        }
        public DateTime StartDate
        {
            set { this.startDate = value; }
            get { return this.startDate; }
        }
        public DateTime EndDate
        {
            set { this.endDate = value; }
            get { return this.endDate; }
        }
        public int AllowComment
        {
            set { this.allowComment = value; }
            get { return this.allowComment; }
        }
        public string InArticleId
        {
            set { this.inArticleId = value; }
            get { return this.inArticleId; }
        }

        public int ParentId
        {
            set { this.parentId = value; }
            get { return this.parentId; }
        }
    }
}

