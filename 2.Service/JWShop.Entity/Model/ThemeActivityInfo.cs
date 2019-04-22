using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 专题活动实体模型
    /// </summary>
    public sealed class ThemeActivityInfo
    {
        public const string TABLENAME = "ThemeActivityInfo";

        private int id;
        private string name = string.Empty;
        private string photo = string.Empty;
        private string description = string.Empty;
        private string css = string.Empty;
        private string cssMobile = string.Empty;
        private string productGroup = string.Empty;
        private string style = string.Empty;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        public string Css
        {
            set { this.css = value; }
            get { return this.css; }
        }
        public string CssMobile
        {
            set { this.cssMobile = value; }
            get { return this.cssMobile; }
        }
        public string ProductGroup
        {
            set { this.productGroup = value; }
            get { return this.productGroup; }
        }
        public string Style
        {
            set { this.style = value; }
            get { return this.style; }
        }
    }
}