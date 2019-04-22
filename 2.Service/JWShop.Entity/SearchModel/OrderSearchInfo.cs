using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 订单搜索模型。
    /// </summary>
    public sealed class OrderSearchInfo
    {
        private int shopId = int.MinValue;
        private string orderNumber = string.Empty;
        private int orderStatus=int.MinValue;
        private string consignee = string.Empty;
        private string regionId = string.Empty;
        private int shippingId=int.MinValue;
        private DateTime startAddDate = DateTime.MinValue;
        private DateTime endAddDate = DateTime.MinValue;
        private DateTime orderDate = DateTime.MinValue;
        private int userId = int.MinValue;
        private string userName = string.Empty;
        private int isDelete = int.MinValue;
        private int isNoticed = int.MinValue;
        private int selfPick = int.MinValue;
        private DateTime startPayDate = DateTime.MinValue;
        private DateTime endPayDate = DateTime.MinValue;
        private int isActivity = int.MinValue;
        private int favorableActivityId = int.MinValue;
        private int notEqualStatus = int.MinValue;


        //新加属性
        private int saleid = int.MinValue;
        private int shuigongid = int.MinValue;
        private int shiyaid = int.MinValue;
        private string shuigong_name = string.Empty;
        private string shuigong_tel = string.Empty;
        private string shiya_name = string.Empty;
        private string shiya_tel = string.Empty;
        private int shuigong_stat = int.MinValue;
        private int shiya_stat = int.MinValue;

        /// <summary>
        /// 订单生命周期状态
        /// </summary>
        public int OrderPeriod { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 订单号或者手机号
        /// </summary>
        public string SearchKey { set; get; }
        /// <summary>
        /// 是否自提：1--自提，0--否
        /// </summary>
        public int SelfPick
        {
            get { return this.selfPick; }
            set { this.selfPick = value; }
        }
        public DateTime StartPayDate
        {
            get { return this.startPayDate; }
            set { this.startPayDate = value; }
        }
        public DateTime EndPayDate
        {
            get { return this.endPayDate; }
            set { this.endPayDate = value; }
        }
        public int IsActivity
        {
            get { return this.isActivity; }
            set { this.isActivity = value; }
        }
        public int FavorableActivityId
        {
            get { return this.favorableActivityId; }
            set { this.favorableActivityId = value; }
        }
        /// <summary>
        /// 订单状态不等于X
        /// </summary>
        public int NotEqualStatus
        {
            get { return this.notEqualStatus; }
            set { this.notEqualStatus = value; }
        }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate
        {
            get { return this.orderDate; }
            set { this.orderDate = value; }
        }

        public int IsNoticed
        {
            set { this.isNoticed=value; }
            get { return this.isNoticed; }
        }
        public int ShopId
        {
            set { this.shopId = value; }
            get { return this.shopId; }
        }
        public string OrderNumber
        {
            set { this.orderNumber = value; }
            get { return this.orderNumber; }
        }
        public int OrderStatus
        {
            set { this.orderStatus = value; }
            get { return this.orderStatus; }
        }
        public string Consignee
        {
            set { this.consignee = value; }
            get { return this.consignee; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public int ShippingId
        {
            set { this.shippingId = value; }
            get { return this.shippingId; }
        }
        public DateTime StartAddDate
        {
            set { this.startAddDate = value; }
            get { return this.startAddDate; }
        }
        public DateTime EndAddDate
        {
            set { this.endAddDate = value; }
            get { return this.endAddDate; }
        }
        public int UserId
        {
            set { this.userId = value; }
            get { return this.userId; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }
        /// <summary>
        /// 是否逻辑删除，1-删除，0-未删除
        /// </summary>
        public int IsDelete
        {
            set { this.isDelete = value; }
            get { return this.isDelete; }
        }

        #region 拼团订单搜索
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 团购商品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 团长Id
        /// </summary>
        public int Leader { get; set; }
        /// <summary>
        /// 排除团长Id
        /// </summary>
        public int NotLeader { get; set; }
        /// <summary>
        /// 拼团状态，对应enum--GroupBuyStatus
        /// </summary>
        public int Status { get; set; } = 2;
        #endregion


        /// <summary>
        /// 经销商ID
        /// </summary>
        public int Saleid
        {
            set { this.saleid = value; }
            get { return this.saleid; }
        }
        /// <summary>
        /// 水工ID
        /// </summary>
        public int Shuigongid
        {
            set { this.shuigongid = value; }
            get { return this.shuigongid; }
        }
        /// <summary>
        /// 试压ID
        /// </summary>
        public int Shiyaid
        {
            set { this.shiyaid = value; }
            get { return this.shiyaid; }
        }
        /// <summary>
        /// 水工姓名
        /// </summary>
        public string Shuigong_name
        {
            set { this.shuigong_name = value; }
            get { return this.shuigong_name; }
        }
        /// <summary>
        /// 水工电话
        /// </summary>
        public string Shuigong_tel
        {
            set { this.shuigong_tel = value; }
            get { return this.shuigong_tel; }
        }
        /// <summary>
        /// 试压姓名
        /// </summary>
        public string Shiya_name
        {
            set { this.shiya_name = value; }
            get { return this.shiya_name; }
        }
        /// <summary>
        /// 试压电话
        /// </summary>
        public string Shiya_tel
        {
            set { this.shiya_tel = value; }
            get { return this.shiya_tel; }
        }
        /// <summary>
        /// 安装状态(0 未安装  1 已安装)
        /// </summary>
        public int Shuigong_stat
        {
            set { this.shuigong_stat = value; }
            get { return this.shuigong_stat; }
        }
        /// <summary>
        /// 试压状态(0 未试压  1 已试压)
        /// </summary>
        public int Shiya_stat
        {
            set { this.shiya_stat = value; }
            get { return this.shiya_stat; }
        }
    }
}

