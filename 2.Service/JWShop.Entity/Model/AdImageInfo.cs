using System;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 广告图片实体模型
    /// </summary>
    [Serializable]
    [Table("AdImage")]
    public sealed class AdImageInfo
    {
        public const string TABLENAME = "AdImage";

        private int id;
        private int adType;
        private string title = string.Empty;
        private string subTitle = string.Empty;
        private string linkUrl = string.Empty;
        private string imageUrl = string.Empty;
        private string mobileLinkUrl = string.Empty;
        private string mobileImageUrl = string.Empty;
        private int classId;
        private int orderId;
        private string remark = string.Empty;
        private DateTime tm;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int AdType
        {
            get { return adType; }
            set { adType = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }
        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }
        public string MobileLinkUrl
        {
            get { return mobileLinkUrl; }
            set { mobileLinkUrl = value; }
        }
        public string MobileImageUrl
        {
            get { return mobileImageUrl; }
            set { mobileImageUrl = value; }
        }
        public int ClassId
        {
            get { return classId; }
            set { classId = value; }
        }
        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public DateTime Tm
        {
            get { return tm; }
            set { tm = value; }
        }
    }
}