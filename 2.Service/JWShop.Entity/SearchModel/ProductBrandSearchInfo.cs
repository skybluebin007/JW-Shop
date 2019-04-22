using System;


namespace JWShop.Entity
{
    /// <summary>
    /// 品牌搜索
    /// </summary>
   public sealed class ProductBrandSearchInfo
    {
        private string name = string.Empty;
        private string spelling = string.Empty;
        private string key = string.Empty;
        private int isTop = int.MinValue;
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string Spelling
        {
            set { this.spelling = value; }
            get { return this.spelling; }
        }
        public string Key {
            get { return this.key; }
            set { this.key = value; }
        }
        public int IsTop {
            get { return this.isTop; }
            set { this.isTop = value; }
        }
    }
}
