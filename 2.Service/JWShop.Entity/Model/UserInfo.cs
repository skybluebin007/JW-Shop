using System;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户实体模型
    /// </summary>
    public sealed class UserInfo
    {
        public const string TABLENAME = "Usr";

        private int id;
        private string userName = string.Empty;
        private string userPassword = string.Empty;
        private string email = string.Empty;
        private int sex;
        private string introduce = string.Empty;
        private string photo = string.Empty;
        private string mSN = string.Empty;
        private string qQ = string.Empty;
        private string tel = string.Empty;
        private string mobile = string.Empty;
        private string regionId = string.Empty;
        private string address = string.Empty;
        private string birthday = string.Empty;
        private string registerIP = string.Empty;
        private DateTime registerDate = DateTime.Now;
        private string lastLoginIP = string.Empty;
        private DateTime lastLoginDate = DateTime.Now;
        private int loginTimes;
        private string safeCode = string.Empty;
        private DateTime findDate = DateTime.Now;
        private int status;
        private string openId = string.Empty;
        private int userType;
        private string cardNo = string.Empty;
        private string cardPwd = string.Empty;
        private string providerNo = string.Empty;
        private string providerName = string.Empty;
        private string providerAddress = string.Empty;
        private string providerBankNo = string.Empty;
        private string providerTaxRegistration = string.Empty;
        private string providerCorporate = string.Empty;
        private string providerLinkerTel = string.Empty;
        private string providerFax = string.Empty;
        private string providerLinker = string.Empty;
        private string providerOperateBrand = string.Empty;
        private string providerOperateClass = string.Empty;
        private string providerAccount = string.Empty;
        private string providerAccountCycle = string.Empty;
        private string providerShipping = string.Empty;
        private string providerService = string.Empty;
        private string providerEnsure = string.Empty;
        private string realName = string.Empty;
        private int hasRegisterCoupon;
        private int hasBirthdayCoupon;
        private DateTime getBirthdayCouponDate = DateTime.Now;
        /// <summary>
        /// 用户附加的统计信息
        /// </summary>
        private decimal moneyLeft = 0;
        private int pointLeft = 0;
        private decimal moneyUsed = 0;
        private int couPonLeft = 0;
        private int noReadMessage = 0;

        public int Id
        {
            set { this.id = value; }
            get { return this.id; }
        }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }
        public string UserPassword
        {
            set { this.userPassword = value; }
            get { return this.userPassword; }
        }
        public string Email
        {
            set { this.email = value; }
            get { return this.email; }
        }
        public int Sex
        {
            set { this.sex = value; }
            get { return this.sex; }
        }
        public string Introduce
        {
            set { this.introduce = value; }
            get { return this.introduce; }
        }
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string MSN
        {
            set { this.mSN = value; }
            get { return this.mSN; }
        }
        public string QQ
        {
            set { this.qQ = value; }
            get { return this.qQ; }
        }
        public string Tel
        {
            set { this.tel = value; }
            get { return this.tel; }
        }
        public string Mobile
        {
            set { this.mobile = value; }
            get { return this.mobile; }
        }
        public string RegionId
        {
            set { this.regionId = value; }
            get { return this.regionId; }
        }
        public string Address
        {
            set { this.address = value; }
            get { return this.address; }
        }
        public string Birthday
        {
            set { this.birthday = value; }
            get { return this.birthday; }
        }
        public string RegisterIP
        {
            set { this.registerIP = value; }
            get { return this.registerIP; }
        }
        public DateTime RegisterDate
        {
            set { this.registerDate = value; }
            get { return this.registerDate; }
        }
        public string LastLoginIP
        {
            set { this.lastLoginIP = value; }
            get { return this.lastLoginIP; }
        }
        public DateTime LastLoginDate
        {
            set { this.lastLoginDate = value; }
            get { return this.lastLoginDate; }
        }
        public int LoginTimes
        {
            set { this.loginTimes = value; }
            get { return this.loginTimes; }
        }
        public string SafeCode
        {
            set { this.safeCode = value; }
            get { return this.safeCode; }
        }
        public DateTime FindDate
        {
            set { this.findDate = value; }
            get { return this.findDate; }
        }
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        public string OpenId
        {
            set { this.openId = value; }
            get { return this.openId; }
        }
        /// <summary>
        /// 会员类型
        /// 0-普通会员  1-分销商&普通会员
        /// </summary>
        public int UserType
        {
            set { this.userType = value; }
            get { return this.userType; }
        }
        public string CardNo
        {
            set { this.cardNo = value; }
            get { return this.cardNo; }
        }
        public string CardPwd
        {
            set { this.cardPwd = value; }
            get { return this.cardPwd; }
        }
        //public bool IsTulouMember
        //{
        //    get { return !string.IsNullOrEmpty(this.CardNo) && !string.IsNullOrEmpty(this.CardPwd); }
        //}


        #region 供应商

        /// <summary>
        /// 供应商-编号
        /// </summary>
        public string ProviderNo
        {
            set { this.providerNo = value; }
            get { return this.providerNo; }
        }
        /// <summary>
        /// 供应商-公司名称
        /// </summary>
        public string ProviderName
        {
            set { this.providerName = value; }
            get { return this.providerName; }
        }
        /// <summary>
        /// 供应商-详细地址
        /// </summary>
        public string ProviderAddress
        {
            set { this.providerAddress = value; }
            get { return this.providerAddress; }
        }
        /// <summary>
        /// 供应商-对公银行开户行
        /// </summary>
        public string ProviderBankNo
        {
            set { this.providerBankNo = value; }
            get { return this.providerBankNo; }
        }
        /// <summary>
        /// 供应商-税务号
        /// </summary>
        public string ProviderTaxRegistration
        {
            set { this.providerTaxRegistration = value; }
            get { return this.providerTaxRegistration; }
        }
        /// <summary>
        /// 供应商-法人代表
        /// </summary>
        public string ProviderCorporate
        {
            set { this.providerCorporate = value; }
            get { return this.providerCorporate; }
        }
        /// <summary>
        /// 供应商-联系电话
        /// </summary>
        public string ProviderLinkerTel
        {
            set { this.providerLinkerTel = value; }
            get { return this.providerLinkerTel; }
        }
        /// <summary>
        /// 供应商-传真
        /// </summary>
        public string ProviderFax
        {
            set { this.providerFax = value; }
            get { return this.providerFax; }
        }
        /// <summary>
        /// 供应商-联系人
        /// </summary>
        public string ProviderLinker
        {
            set { this.providerLinker = value; }
            get { return this.providerLinker; }
        }
        /// <summary>
        /// 供应商-经营品牌
        /// </summary>
        public string ProviderOperateBrand
        {
            set { this.providerOperateBrand = value; }
            get { return this.providerOperateBrand; }
        }
        /// <summary>
        /// 供应商-经营品类
        /// </summary>
        public string ProviderOperateClass
        {
            set { this.providerOperateClass = value; }
            get { return this.providerOperateClass; }
        }
        /// <summary>
        /// 供应商-结算方式
        /// </summary>
        public string ProviderAccount
        {
            set { this.providerAccount = value; }
            get { return this.providerAccount; }
        }
        /// <summary>
        /// 供应商-结算周期
        /// </summary>
        public string ProviderAccountCycle
        {
            set { this.providerAccountCycle = value; }
            get { return this.providerAccountCycle; }
        }
        /// <summary>
        /// 供应商-物流配送
        /// </summary>
        public string ProviderShipping
        {
            set { this.providerShipping = value; }
            get { return this.providerShipping; }
        }
        /// <summary>
        /// 供应商-售后服务
        /// </summary>
        public string ProviderService
        {
            set { this.providerService = value; }
            get { return this.providerService; }
        }
        /// <summary>
        /// 供应商-退换货保障
        /// </summary>
        public string ProviderEnsure
        {
            set { this.providerEnsure = value; }
            get { return this.providerEnsure; }
        }
        #endregion

        /// <summary>
        /// 会员真实姓名
        /// </summary>
        public string RealName
        {
            get { return this.realName; }
            set { this.realName = value; }
        }
        /// <summary>
        /// 是否获取注册赠送优惠券 0-否，1-是
        /// </summary>
        public int HasRegisterCoupon
        {
            get { return this.hasRegisterCoupon; }
            set { this.hasRegisterCoupon = value; }
        }
        /// <summary>
        /// 是否获取生日优惠券
        /// </summary>
        public int HasBirthdayCoupon
        {
            get { return this.hasBirthdayCoupon; }
            set { this.hasBirthdayCoupon = value; }
        }
        /// <summary>
        /// 获取生日优惠券的日期，用来判断是否本年度获取(1年限制获取1次)
        /// </summary>
        public DateTime GetBirthdayCouponDate
        {
            get { return this.getBirthdayCouponDate; }
            set { this.getBirthdayCouponDate = value; }
        }
        /// <summary>
        /// 推荐人 
        /// </summary>
        public int Recommend_UserId { get; set; }

        /// <summary>
        /// 推荐人 
        /// </summary>
        /// [Computed]
        public string Recommend_UserName { get; set; }

        /// <summary>
        /// 分销商状态 -1-冻结  0--待审核  1--正常
        /// </summary>
        public int Distributor_Status { get; set; }
        /// <summary>
        /// 分销商  总佣金
        /// </summary>
        public decimal Total_Commission{get;set;}
        /// <summary>
        /// 分销商  总提现
        /// </summary>
        public decimal Total_Withdraw { get; set; }
        /// <summary>
        /// 总下级数量(1,2级)
        /// </summary>
        public int Total_Subordinate { get; set; }
        /// <summary>
        /// 分销商等级
        /// </summary>
        [Computed]
        public int Distributor_GradeId { get; set; }
        /// <summary>
        /// 分销商等级名称
        /// </summary>
        [Computed]
        public string Distributor_Grade_Title { get; set; }

        /***********************用户附加的统计信息************************/

        [Computed]
        public decimal MoneyLeft
        {
            set { this.moneyLeft = value; }
            get { return this.moneyLeft; }
        }
        [Computed]
        public int PointLeft
        {
            set { this.pointLeft = value; }
            get { return this.pointLeft; }
        }
        [Computed]
        public decimal MoneyUsed
        {
            set { this.moneyUsed = value; }
            get { return this.moneyUsed; }
        }
        /// <summary>
        /// 未使用未过期的优惠券 0-否，1-是
        /// </summary>
        [Computed]
        public int CouPonLeft {
            get { return this.couPonLeft; }
            set { this.couPonLeft = value; }
        }
        /// <summary>
        /// 未读消息
        /// </summary>
        [Computed]
        public int NoReadMessage {
            get { return this.noReadMessage; }
            set { this.noReadMessage = value; }
        }
        /// <summary>
        /// 会员等级
        /// </summary>
        [Computed]
        public string UserGrade { get; set; }
        /// <summary>
        /// 购物次数(确认收货状态统计)
        /// </summary>
        [Computed]
        public int ShoppingTimes { get; set; }
        /***********************用户附加的统计信息 End************************/
    }
}