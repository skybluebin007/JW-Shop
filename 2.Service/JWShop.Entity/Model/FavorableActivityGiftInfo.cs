using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 优惠活动礼品模型
    /// </summary>
    [Serializable]
    public sealed class FavorableActivityGiftInfo
    {
        private int id;
        private string name = string.Empty;
        private string photo = string.Empty;
        private string description = string.Empty;

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
    }
}