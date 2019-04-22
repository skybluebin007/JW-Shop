using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 文章实体模型
    /// </summary>
    public sealed class ArticleInfo
    {
        public const string TABLENAME = "Article";

        private int id;
        private string title = string.Empty;
        private string classId = string.Empty;
        private int isTop;
        private string author = string.Empty;
        private string resource = string.Empty;
        private string keywords = string.Empty;
        private string url = string.Empty;
        private string photo = string.Empty;
        private string summary = string.Empty;
        private string content = string.Empty;
        private DateTime date = DateTime.Now;
        private int orderId;
        private int viewCount;
        private int loveCount;
        private DateTime realDate = DateTime.Now;
        private string filePath = string.Empty;
        private int parentId;
        private int addCol1;
        private string addCol2 = string.Empty;
        private long rowNumber;
        private string content1 = string.Empty;
        private string mobilecontent1 = string.Empty;
        private string content2 = string.Empty;
        private string mobilecontent2 = string.Empty;
        private string addCol3 = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
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
        public string Url
        {
            set { this.url = value; }
            get { return this.url; }
        }
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string Summary
        {
            set { this.summary = value; }
            get { return this.summary; }
        }
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }
        public DateTime Date
        {
            set { this.date = value; }
            get { return this.date; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public int ViewCount
        {
            set { this.viewCount = value; }
            get { return this.viewCount; }
        }
        public int LoveCount
        {
            set { this.loveCount = value; }
            get { return this.loveCount; }
        }
        public DateTime RealDate
        {
            set { this.realDate = value; }
            get { return this.realDate; }
        }
        public string FilePath
        {
            set { this.filePath = value; }
            get { return this.filePath; }
        }
        public int ParentId
        {
            set { this.parentId = value; }
            get { return this.parentId; }
        }
        public int AddCol1
        {
            set { this.addCol1 = value; }
            get { return this.addCol1; }
        }
        public string AddCol2
        {
            set { this.addCol2 = value; }
            get { return this.addCol2; }
        }
        public long RowNumber
        {
            set { this.rowNumber = value; }
            get { return this.rowNumber; }
        }
        public string Content1
        {
            set { this.content1 = value; }
            get { return this.content1; }
        }
        public string Mobilecontent1
        {
            set { this.mobilecontent1 = value; }
            get { return this.mobilecontent1; }
        }
        public string Content2
        {
            set { this.content2 = value; }
            get { return this.content2; }
        }
        public string Mobilecontent2
        {
            set { this.mobilecontent2 = value; }
            get { return this.mobilecontent1; }
        }
        public string AddCol3
        {
            set { this.addCol3 = value; }
            get { return this.addCol3; }
        }
    }
}