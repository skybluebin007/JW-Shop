using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 文章分类实体模型
    /// </summary>
    [Serializable]
    public sealed class ArticleClassInfo
    {
        public const string TABLENAME = "ArticleClass";

        private int id;
        private int parentId;
        private int orderId;
        private string name = string.Empty;
        private string enName = string.Empty;
        private string description = string.Empty;
        private string photo = string.Empty;
        private int showType;
        private int showTerminal;
        private int isSystem;
        private int addCol1;
        private string addCol2 = string.Empty;

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public int ParentId
        {
            set { this.parentId = value; }
            get { return this.parentId; }
        }
        public int OrderId
        {
            set { this.orderId = value; }
            get { return this.orderId; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string EnName
        {
            set { this.enName = value; }
            get { return this.enName; }
        }
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public int ShowType
        {
            set { this.showType = value; }
            get { return this.showType; }
        }
        public int ShowTerminal
        {
            set { this.showTerminal = value; }
            get { return this.showTerminal; }
        }
        public int IsSystem
        {
            set { this.isSystem = value; }
            get { return this.isSystem; }
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
    }
}