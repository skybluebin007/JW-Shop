using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 礼品搜索模型。
    /// </summary>
    public sealed class FavorableActivityGiftSearchInfo
    {
        private string name = string.Empty;
        private int[] inGiftIds = new int[] { };

        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public int[] InGiftIds
        {
            set { this.inGiftIds = value; }
            get { return this.inGiftIds; }
        }
    }
}