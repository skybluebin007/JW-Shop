using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    public sealed class ShopConfigInfo
    {
        private string title = string.Empty;
        private string copyright = string.Empty;
        private string author = string.Empty;
        private string keywords = string.Empty;
        private string description = string.Empty;
        private string secureKey = string.Empty;
        private int codeType;
        private int codeLength;
        private int codeDot;
        private int passwordType;
        private string powerKey;
        private string adminCookies = string.Empty;
        private string userCookies = string.Empty;
        private string templatePath = string.Empty;
        private int startYear = 0;
        private int endYear = 0;
        private string tel = string.Empty;
        private string recordCode = string.Empty;
        private string staticCode = string.Empty;
        private int voteID = 0;
        private int allowAnonymousVote;
        private int voteRestrictTime;
        private int saveAutoClosePop;
        private int popCloseRefresh;
        private string uploadFile = string.Empty;
        private string uploadImage = string.Empty;
        private string hotKeyword = string.Empty;
        private int productStorageType;
        private int allowAnonymousComment;
        private int commentDefaultStatus;
        private int commentRestrictTime;
        private int allowAnonymousCommentOperate;
        private int commentOperateRestrictTime;
        private int allowAnonymousReply;
        private int replyRestrictTime;
        private int allowAnonymousAddTags;
        private int addTagsRestrictTime;
        private string emailUserName = string.Empty;
        private string emailPassword = string.Empty;
        private string emailServer = string.Empty;
        private int emailServerPort;
        private string forbiddenName = string.Empty;
        private int passwordMinLength;
        private int passwordMaxLength;
        private int userNameMinLength;
        private int userNameMaxLength;
        private int registerCheck;
        private string agreement = string.Empty;
        private double findPasswordTimeRestrict;
        private int allowAnonymousAddCart;
        #region 订单状态改变是否发送通知（邮件、短信）
        private int payOrder;
        private int cancleOrder;
        private int checkOrder;
        private int sendOrder;
        private int receivedOrder;
        private int changeOrder;
        private int returnOrder;
        private int backOrder;
        private int refundOrder;
        #endregion
        #region 订单状态改变发送邮件
        public int PayOrderEmail { get; set; }
        public int CancleOrderEmail { get; set; }
        public int CheckOrderEmail { get; set; }
        public int SendOrderEmail { get; set; }
        public int ReceivedOrderEmail { get; set; }
        public int ChangeOrderEmail { get; set; }
        public int ReturnOrderEmail { get; set; }
        public int BackOrderEmail { get; set; }
        public int RefundOrderEmail { get; set; }
        #endregion
        #region 订单状态改变发送短信
        public int PayOrderMsg { get; set; }
        public int CancleOrderMsg { get; set; }
        public int CheckOrderMsg { get; set; }
        public int SendOrderMsg { get; set; }
        public int ReceivedOrderMsg { get; set; }
        public int ChangeOrderMsg { get; set; }
        public int ReturnOrderMsg { get; set; }
        public int BackOrderMsg { get; set; }
        public int RefundOrderMsg { get; set; }
        #endregion
        private string namePrefix = string.Empty;
        private string idPrefix = string.Empty;
        private int waterType;
        private int waterPossition;
        private string text;
        private string textFont;
        private int textSize;
        private string textColor;
        private string waterPhoto;
        private string appKey;
        private string appSecret;
        private string nickName;
        private int deleteProductClass;
        private string domain;
        private string frequency;
        private string priority;

        private string fax;
        private string postCode;
        private string smsAddress;

        private string address;
        private string siteName;
        private string icoAddress;
        private string logoAddress;
        private string mobileLogoAddress;

        private int allImageWidth;
        private int allImageIsNail;

        private string qq;
        private string weiXin;
        private string mobileImage;
        private string gTel;
        private string siteLink;

        private string cnzzId;
        #region 各种logo尺寸
        public int IcoWidth { get; set; }
        public int IcoHeight { get; set; }
        public int LogoWidth { get; set; }
        public int LogoHeight { get; set; }
        public int MobileLogoWidth { get; set; }
        public int MobileLogoHeight { get; set; }
        public int MobilePhotoWidth { get; set; }
        public int MobilePhotoHeight { get; set; }
        public int WeixinWidth { get; set; }
        public int WeixinHeight { get; set; }
        /// <summary>
        /// 会员登录背景
        /// </summary>
        public string UserLoginPic { get; set; }
        /// <summary>
        /// 会员登录背景宽度
        /// </summary>
        public int UserLoginWidth { get; set; }
        /// <summary>
        /// 会员登录背景高度
        /// </summary>
        public int UserLoginHeight { get; set; }
        /// <summary>
        /// 会员注册背景
        /// </summary>
        public string UserRegisterPic { get; set; }
        /// <summary>
        /// 会员注册背景宽度
        /// </summary>
        public int UserRegisterWidth { get; set; }
        /// <summary>
        /// 会员注册背景高度
        /// </summary>
        public int UserRegisterHeight { get; set; }
        #endregion
        //绑定邮箱时间限制（验证邮箱）
        public double BindEmailTime { get; set; }
        public string UPLogo { get; set; }
        public int UPLogoWidth { get; set; }
        public int UPLogoHeight { get; set; }
        //是否开启积分结算抵现
        public int EnablePointPay { get; set; }
        //结算时积分抵现百分比
        public double PointToMoney { get; set; }
        //订单完成后赠送积分百分比
        public double MoneyToPoint { get; set; }
        #region 微站配置参数
        public string AppID { get; set; }
        public string Appsecret { get; set; }
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        //微信登录URL
        public string WechatLoginURL { get; set; }
        //关注回复标题
        public string AttentionTitle { get; set; }
        //关注回复内容
        public string AttentionSummary { get; set; }
        //关注回复图片
        public string AttentionPicture { get; set; }
        //默认回复
        public string DefaultReply { get; set; }
        #endregion
        //注册时验证方式
        public int RegisterType { get; set; }
        //是否开发票 0：不开   1：开
        public int Invoicing { get; set; }
        /// <summary>
        /// 投票开始时间
        /// </summary>
        private DateTime voteStartDate = DateTime.Now;
        /// <summary>
        /// 投票结束时间
        /// </summary>
        private DateTime voteEndDate = DateTime.Now;
        /// <summary>
        /// 订单付款时限（分钟）
        /// </summary>
        public int OrderPayTime { get; set; }
        /// <summary>
        /// 订单自动收货天数（自发货日期开始计算）
        /// </summary>
        public int OrderRecieveShippingDays { get; set; }
        /// <summary>
        /// 打印机SN
        /// </summary>
        public string PrintSN { get; set; }
        /// <summary>
        /// 成为分销商是否需要审核
        /// 0-不需要审核  1-需要审核
        /// </summary>
        public int CheckToBeDistributor { get; set; }
        /// <summary>
        /// 1级分销返佣
        /// </summary>
        public decimal FirstLevelDistributorRebatePercent { get; set; }
        /// <summary>
        /// 2级分销返佣
        /// </summary>
        public decimal SecondLevelDistributorRebatePercent { get; set; }
        /// <summary>
        /// 微信支付订单模板消息Id
        /// </summary>
        public string OrderPayTemplateId { get; set; }
        /// <summary>
        /// 到店自提提醒 模板消息id
        /// </summary>
        public string SelfPickTemplateId { get; set; }
        /// <summary>
        /// 开团提醒 模板Id
        /// </summary>
        public string OpenGroupTemplateId { get; set; }
        /// <summary>
        /// 参团提醒 模板Id
        /// </summary>
        public string GroupSignTemplateId { get; set; }
        /// <summary>
        /// 砍价成功提醒 模板Id
        /// </summary>
        public string BarGainTemplateId { get; set; }

        /// <summary>
        /// 拼团成功 模板Id 暂未启用
        /// </summary>
        public string GroupSuccessTemplateId { get; set; }
        /// <summary>
        /// 拼团失败 模板Id 暂未启用
        /// </summary>
        public string GroupFailTemplateId { get; set; }
        #region 微信JSSDK
        private string ticket;
        private int expireTime;
        #endregion
        #region 订单满立减
        /// <summary>
        /// 订单满立减开关，1：开
        /// </summary>
        public int PayDiscount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderMoney { get; set; }
        /// <summary>
        /// 减金额
        /// </summary>
        public decimal OrderDisCount { get; set; }
        #endregion

        public string SmsAddress
        {
            get { return smsAddress; }
            set { smsAddress = value; }
        }
        private string smsUserName;

        public string SmsUserName
        {
            get { return smsUserName; }
            set { smsUserName = value; }
        }
        private string smsPassword;

        public string SmsPassword
        {
            get { return smsPassword; }
            set { smsPassword = value; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Copyright
        {
            get { return this.copyright; }
            set { this.copyright = value; }
        }
        public string Author
        {
            get { return this.author; }
            set { this.author = value; }
        }
        public string Keywords
        {
            get { return this.keywords; }
            set { this.keywords = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public string SecureKey
        {
            get { return this.secureKey; }
            set { this.secureKey = value; }
        }
        public int CodeType
        {
            get { return this.codeType; }
            set { this.codeType = value; }
        }
        public int CodeLength
        {
            get { return this.codeLength; }
            set { this.codeLength = value; }
        }
        public int CodeDot
        {
            get { return this.codeDot; }
            set { this.codeDot = value; }
        }
        public int PasswordType
        {
            get { return this.passwordType; }
            set { this.passwordType = value; }
        }
        public string PowerKey
        {
            get { return this.powerKey; }
            set { this.powerKey = value; }
        }
        public string AdminCookies
        {
            get { return this.adminCookies; }
            set { this.adminCookies = value; }
        }
        public string UserCookies
        {
            get { return this.userCookies; }
            set { this.userCookies = value; }
        }
        public string TemplatePath
        {
            get { return this.templatePath; }
            set { this.templatePath = value; }
        }
        public int StartYear
        {
            get { return this.startYear; }
            set { this.startYear = value; }
        }
        public int EndYear
        {
            get { return this.endYear; }
            set { this.endYear = value; }
        }
        public string Tel
        {
            get { return this.tel; }
            set { this.tel = value; }
        }
        public string RecordCode
        {
            get { return this.recordCode; }
            set { this.recordCode = value; }
        }
        public string StaticCode
        {
            get { return this.staticCode; }
            set { this.staticCode = value; }
        }
        public int VoteID
        {
            get { return this.voteID; }
            set { this.voteID = value; }
        }
        public int AllowAnonymousVote
        {
            get { return this.allowAnonymousVote; }
            set { this.allowAnonymousVote = value; }
        }
        public int VoteRestrictTime
        {
            get { return this.voteRestrictTime; }
            set { this.voteRestrictTime = value; }
        }
        public int SaveAutoClosePop
        {
            get { return this.saveAutoClosePop; }
            set { this.saveAutoClosePop = value; }
        }
        public int PopCloseRefresh
        {
            get { return this.popCloseRefresh; }
            set { this.popCloseRefresh = value; }
        }
        public string UploadFile
        {
            get { return this.uploadFile; }
            set { this.uploadFile = value; }
        }
        public string UploadImage
        {
            get { return this.uploadImage; }
            set { this.uploadImage = value; }
        }
        public string HotKeyword
        {
            get { return this.hotKeyword; }
            set { this.hotKeyword = value; }
        }
        public int ProductStorageType
        {
            get { return this.productStorageType; }
            set { this.productStorageType = value; }
        }
        public int AllowAnonymousComment
        {
            get { return this.allowAnonymousComment; }
            set { this.allowAnonymousComment = value; }
        }
        public int CommentDefaultStatus
        {
            get { return this.commentDefaultStatus; }
            set { this.commentDefaultStatus = value; }
        }
        public int CommentRestrictTime
        {
            get { return this.commentRestrictTime; }
            set { this.commentRestrictTime = value; }
        }
        public int AllowAnonymousCommentOperate
        {
            get { return this.allowAnonymousCommentOperate; }
            set { this.allowAnonymousCommentOperate = value; }
        }
        public int CommentOperateRestrictTime
        {
            get { return this.commentOperateRestrictTime; }
            set { this.commentOperateRestrictTime = value; }
        }
        public int AllowAnonymousReply
        {
            get { return this.allowAnonymousReply; }
            set { this.allowAnonymousReply = value; }
        }
        public int ReplyRestrictTime
        {
            get { return this.replyRestrictTime; }
            set { this.replyRestrictTime = value; }
        }
        public int AllowAnonymousAddTags
        {
            get { return this.allowAnonymousAddTags; }
            set { this.allowAnonymousAddTags = value; }
        }
        public int AddTagsRestrictTime
        {
            get { return this.addTagsRestrictTime; }
            set { this.addTagsRestrictTime = value; }
        }
        public string EmailUserName
        {
            get { return this.emailUserName; }
            set { this.emailUserName = value; }
        }
        public string EmailPassword
        {
            get { return this.emailPassword; }
            set { this.emailPassword = value; }
        }
        public string EmailServer
        {
            get { return this.emailServer; }
            set { this.emailServer = value; }
        }
        public int EmailServerPort
        {
            get { return this.emailServerPort; }
            set { this.emailServerPort = value; }
        }
        public string ForbiddenName
        {
            get { return this.forbiddenName; }
            set { this.forbiddenName = value; }
        }
        public int PasswordMinLength
        {
            get { return this.passwordMinLength; }
            set { this.passwordMinLength = value; }
        }
        public int PasswordMaxLength
        {
            get { return this.passwordMaxLength; }
            set { this.passwordMaxLength = value; }
        }
        public int UserNameMinLength
        {
            get { return this.userNameMinLength; }
            set { this.userNameMinLength = value; }
        }
        public int UserNameMaxLength
        {
            get { return this.userNameMaxLength; }
            set { this.userNameMaxLength = value; }
        }
        /// <summary>
        /// 1:无需审核 2:邮件验证 3:人工审核
        /// </summary>
        public int RegisterCheck
        {
            get { return this.registerCheck; }
            set { this.registerCheck = value; }
        }
        public string Agreement
        {
            get { return this.agreement; }
            set { this.agreement = value; }
        }
        public double FindPasswordTimeRestrict
        {
            get { return this.findPasswordTimeRestrict; }
            set { this.findPasswordTimeRestrict = value; }
        }
        public int AllowAnonymousAddCart
        {
            get { return this.allowAnonymousAddCart; }
            set { this.allowAnonymousAddCart = value; }
        }
        public int PayOrder
        {
            get { return this.payOrder; }
            set { this.payOrder = value; }
        }
        public int CancleOrder
        {
            get { return this.cancleOrder; }
            set { this.cancleOrder = value; }
        }
        public int CheckOrder
        {
            get { return this.checkOrder; }
            set { this.checkOrder = value; }
        }
        public int SendOrder
        {
            get { return this.sendOrder; }
            set { this.sendOrder = value; }
        }
        public int ReceivedOrder
        {
            get { return this.receivedOrder; }
            set { this.receivedOrder = value; }
        }
        public int ChangeOrder
        {
            get { return this.changeOrder; }
            set { this.changeOrder = value; }
        }
        public int ReturnOrder
        {
            get { return this.returnOrder; }
            set { this.returnOrder = value; }
        }
        public int BackOrder
        {
            get { return this.backOrder; }
            set { this.backOrder = value; }
        }
        public int RefundOrder
        {
            get { return this.refundOrder; }
            set { this.refundOrder = value; }
        }
        public string NamePrefix
        {
            get { return this.namePrefix; }
            set { this.namePrefix = value; }
        }
        public string IDPrefix
        {
            get { return this.idPrefix; }
            set { this.idPrefix = value; }
        }
        public int WaterType
        {
            get { return this.waterType; }
            set { this.waterType = value; }
        }
        public int WaterPossition
        {
            get { return this.waterPossition; }
            set { this.waterPossition = value; }
        }
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        public string TextFont
        {
            get { return this.textFont; }
            set { this.textFont = value; }
        }
        public int TextSize
        {
            get { return this.textSize; }
            set { this.textSize = value; }
        }
        public string TextColor
        {
            get { return this.textColor; }
            set { this.textColor = value; }
        }
        public string WaterPhoto
        {
            get { return this.waterPhoto; }
            set { this.waterPhoto = value; }
        }
        public string AppKey
        {
            get { return this.appKey; }
            set { this.appKey = value; }
        }
        public string AppSecret
        {
            get { return this.appSecret; }
            set { this.appSecret = value; }
        }
        public string NickName
        {
            get { return this.nickName; }
            set { this.nickName = value; }
        }
        public int DeleteProductClass
        {
            get { return this.deleteProductClass; }
            set { this.deleteProductClass = value; }
        }
        public string Domain
        {
            get { return this.domain; }
            set { this.domain = value; }
        }
        public string Frequency
        {
            get { return this.frequency; }
            set { this.frequency = value; }
        }
        public string Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }
        public string IcoAddress
        {
            get { return icoAddress; }
            set { icoAddress = value; }
        }
        public string LogoAddress
        {
            get { return logoAddress; }
            set { logoAddress = value; }
        }
        public string MobileLogoAddress
        {
            get { return mobileLogoAddress; }
            set { mobileLogoAddress = value; }
        }

        public int AllImageWidth
        {
            get { return this.allImageWidth; }
            set { this.allImageWidth = value; }
        }

        public int AllImageIsNail
        {
            get { return this.allImageIsNail; }
            set { this.allImageIsNail = value; }
        }

        public string QQ
        {
            get { return this.qq; }
            set { this.qq = value; }
        }

        public string WeiXin
        {
            get { return this.weiXin; }
            set { this.weiXin = value; }
        }

        public string MobileImage
        {
            get { return this.mobileImage; }
            set { this.mobileImage = value; }
        }

        public string GTel
        {
            get { return this.gTel; }
            set { this.gTel = value; }
        }
        public string SiteLink
        {
            get { return this.siteLink; }
            set { this.siteLink = value; }
        }
        public string CnzzId
        {
            get { return this.cnzzId; }
            set { this.cnzzId = value; }
        }
        public DateTime VoteStartDate
        {
            get { return this.voteStartDate; }
            set { this.voteStartDate = value; }
        }
        public DateTime VoteEndDate
        {
            get { return this.voteEndDate; }
            set { this.voteEndDate = value; }
        }
        public string Ticket
        {
            get { return this.ticket; }
            set { this.ticket = value; }
        }
        public int ExpireTime
        {
            get
            {
                return this.expireTime;
            }
            set
            {
                this.expireTime = value;
            }
        }
        /// <summary>
        /// 后台管理员Logo
        /// </summary>
        public string AdminLogo { get; set; }
        /// <summary>
        /// 小程序码
        /// </summary>
        public string LittlePrgCode { get; set; }
        /// <summary>
        /// 营业资质
        /// </summary>
        public string Qualification { get; set; }
        /// <summary>
        /// 店铺营业时间
        /// </summary>
        public string BusinessHours { get; set; }
        /// <summary>
        /// 团购天数限制
        /// </summary>
        public int GroupBuyDays { get; set; }
        /// <summary>
        /// 自提：1-开，0-关
        /// </summary>
        public int SelfPick { get; set; }
    }
}
