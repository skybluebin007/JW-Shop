using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class ShopConfigAdd : JWShop.Page.AdminBasePage
    {
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadConfig", PowerCheckType.Single);
                VoteStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                VoteEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                Title.Text = ShopConfig.ReadConfigInfo().Title;
                Copyright.Text = ShopConfig.ReadConfigInfo().Copyright;
                Author.Text = ShopConfig.ReadConfigInfo().Author;
                Keywords.Text = ShopConfig.ReadConfigInfo().Keywords;
                Description.Text = ShopConfig.ReadConfigInfo().Description;
                CodeType.Text = ShopConfig.ReadConfigInfo().CodeType.ToString();
                CodeLength.Text = ShopConfig.ReadConfigInfo().CodeLength.ToString();
                CodeDot.Text = ShopConfig.ReadConfigInfo().CodeDot.ToString();
                StartYear.Text = ShopConfig.ReadConfigInfo().StartYear.ToString();
                EndYear.Text = ShopConfig.ReadConfigInfo().EndYear.ToString();
                Tel.Text = ShopConfig.ReadConfigInfo().Tel;
                RecordCode.Text = ShopConfig.ReadConfigInfo().RecordCode;
                StaticCode.Text = ShopConfig.ReadConfigInfo().StaticCode;
                UploadFile.Text = ShopConfig.ReadConfigInfo().UploadFile;

                Fax.Text = ShopConfig.ReadConfigInfo().Fax;
                PostCode.Text = ShopConfig.ReadConfigInfo().PostCode;
                UploadImage.Text = ShopConfig.ReadConfigInfo().UploadImage;
                Address.Text = ShopConfig.ReadConfigInfo().Address;

                SiteName.Text = ShopConfig.ReadConfigInfo().SiteName;
                ICOAddress.Text = ShopConfig.ReadConfigInfo().IcoAddress;
                
                LogoAddress.Text = ShopConfig.ReadConfigInfo().LogoAddress;
              
                MobileLogoAddress.Text = ShopConfig.ReadConfigInfo().MobileLogoAddress;
              
                Text.Text = ShopConfig.ReadConfigInfo().Text;
                TextFont.Text = ShopConfig.ReadConfigInfo().TextFont;
                TextSize.Text = ShopConfig.ReadConfigInfo().TextSize.ToString();
                TextColor.Value = ShopConfig.ReadConfigInfo().TextColor;
                WaterPhoto.Text = ShopConfig.ReadConfigInfo().WaterPhoto;


                UploadFile.Text = ShopConfig.ReadConfigInfo().UploadFile;
                AllowAnonymousComment.Text = ShopConfig.ReadConfigInfo().AllowAnonymousComment.ToString();
               
                CommentRestrictTime.Text = ShopConfig.ReadConfigInfo().CommentRestrictTime.ToString();
                EmailUserName.Text = ShopConfig.ReadConfigInfo().EmailUserName;
                EmailPassword.Text = ShopConfig.ReadConfigInfo().EmailPassword;
                EmailServer.Text = ShopConfig.ReadConfigInfo().EmailServer;
                EmailServerPort.Text = ShopConfig.ReadConfigInfo().EmailServerPort.ToString();
                ForbiddenName.Text = ShopConfig.ReadConfigInfo().ForbiddenName;
                PasswordMinLength.Text = ShopConfig.ReadConfigInfo().PasswordMinLength.ToString();
                PasswordMaxLength.Text = ShopConfig.ReadConfigInfo().PasswordMaxLength.ToString();
                UserNameMinLength.Text = ShopConfig.ReadConfigInfo().UserNameMinLength.ToString();
                UserNameMaxLength.Text = ShopConfig.ReadConfigInfo().UserNameMaxLength.ToString();
                RegisterCheck.Text = ShopConfig.ReadConfigInfo().RegisterCheck.ToString();
                RegisterType.Text = ShopConfig.ReadConfigInfo().RegisterType.ToString();
                Agreement.Text = ShopConfig.ReadConfigInfo().Agreement;
                FindPasswordTimeRestrict.Text = ShopConfig.ReadConfigInfo().FindPasswordTimeRestrict.ToString();
                BindEmailTime.Text = ShopConfig.ReadConfigInfo().BindEmailTime.ToString();
                QQ.Text = ShopConfig.ReadConfigInfo().QQ;
                WeiXin.Text = ShopConfig.ReadConfigInfo().WeiXin;
                MobileImage.Text = ShopConfig.ReadConfigInfo().MobileImage;
                GTel.Text = ShopConfig.ReadConfigInfo().GTel;

                SiteLink.Text = ShopConfig.ReadConfigInfo().SiteLink;
                AllImageWidth.Text = ShopConfig.ReadConfigInfo().AllImageWidth.ToString();

                CnzzId.Text = ShopConfig.ReadConfigInfo().CnzzId;
                HotKeyword.Text = ShopConfig.ReadConfigInfo().HotKeyword;
                #region 各种logo尺寸
                IcoWidth.Text = ShopConfig.ReadConfigInfo().IcoWidth.ToString();
                IcoHeight.Text = ShopConfig.ReadConfigInfo().IcoHeight.ToString();
                LogoWidth.Text = ShopConfig.ReadConfigInfo().LogoWidth.ToString();
                LogoHeight.Text = ShopConfig.ReadConfigInfo().LogoHeight.ToString();
                MobileLogoWidth.Text = ShopConfig.ReadConfigInfo().MobileLogoWidth.ToString();
                MobileLogoHeight.Text = ShopConfig.ReadConfigInfo().MobileLogoHeight.ToString();
                MobilePhotoWidth.Text = ShopConfig.ReadConfigInfo().MobilePhotoWidth.ToString();
                MobilePhotoHeight.Text = ShopConfig.ReadConfigInfo().MobilePhotoHeight.ToString();
                WeixinWidth.Text = ShopConfig.ReadConfigInfo().WeixinWidth.ToString();
                WeixinHeight.Text = ShopConfig.ReadConfigInfo().WeixinHeight.ToString();
                UPLogo.Text = ShopConfig.ReadConfigInfo().UPLogo;
            
                UPLogoWidth.Text = ShopConfig.ReadConfigInfo().UPLogoWidth.ToString();
                UPLogoHeight.Text = ShopConfig.ReadConfigInfo().UPLogoHeight.ToString();
                UserLoginPic.Text = ShopConfig.ReadConfigInfo().UserLoginPic;
                UserLoginWidth.Text = ShopConfig.ReadConfigInfo().UserLoginWidth.ToString();
                UserLoginHeight.Text = ShopConfig.ReadConfigInfo().UserLoginHeight.ToString();
                UserRegisterPic.Text = ShopConfig.ReadConfigInfo().UserRegisterPic;
                UserRegisterWidth.Text = ShopConfig.ReadConfigInfo().UserRegisterWidth.ToString();
                UserRegisterHeight.Text = ShopConfig.ReadConfigInfo().UserRegisterHeight.ToString();
                #endregion
                //积分抵现的比率
                PointToMoney.Text = ShopConfig.ReadConfigInfo().PointToMoney.ToString();
                //MoneyToPoint.Text = ShopConfig.ReadConfigInfo().MoneyToPoint.ToString();
                // 投票起止时间
                VoteStartDate.Text = ShopConfig.ReadConfigInfo().VoteStartDate.ToString();
                VoteEndDate.Text = ShopConfig.ReadConfigInfo().VoteEndDate.ToString();
                //订单付款时限
                OrderPayTime.Text = ShopConfig.ReadConfigInfo().OrderPayTime.ToString();
                //订单单自动收货天数
                OrderRecieveShippingDays.Text = ShopConfig.ReadConfigInfo().OrderRecieveShippingDays.ToString();
                ////满立减
                //OrderMoney.Text = ShopConfig.ReadConfigInfo().OrderMoney.ToString();
                //OrderDisCount.Text = ShopConfig.ReadConfigInfo().OrderDisCount.ToString();
                AdminLogo.Text = ShopConfig.ReadConfigInfo().AdminLogo;
                LittlePrgCode.Text = ShopConfig.ReadConfigInfo().LittlePrgCode;
                Qualification.Text = ShopConfig.ReadConfigInfo().Qualification;
                BusinessHours.Text = ShopConfig.ReadConfigInfo().BusinessHours;
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateConfig", PowerCheckType.Single);
            ShopConfigInfo config = ShopConfig.ReadConfigInfo();
            config.Title = Title.Text;
            config.Copyright = Copyright.Text;
            config.Author = Author.Text;
            config.Keywords = Keywords.Text;
            config.Description = Description.Text;
            config.CodeType = Convert.ToInt32(CodeType.Text);
            config.CodeLength = Convert.ToInt32(CodeLength.Text);
            config.CodeDot = Convert.ToInt32(CodeDot.Text);
            config.StartYear = Convert.ToInt32(StartYear.Text);
            config.EndYear = Convert.ToInt32(EndYear.Text);
            config.Tel = Tel.Text;
            config.RecordCode = RecordCode.Text;
            config.StaticCode = StaticCode.Text;
            config.UploadFile = UploadFile.Text;

            config.UploadImage = UploadImage.Text;
            config.Fax = Fax.Text;
            config.PostCode = PostCode.Text;
            config.Address = Address.Text;

            config.SiteName = SiteName.Text;
            config.IcoAddress = ICOAddress.Text;
            config.LogoAddress = LogoAddress.Text;
            config.MobileLogoAddress = MobileLogoAddress.Text;

            config.WaterType = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$WaterType");
            config.WaterPossition = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$WaterPossition");
            config.Text = Text.Text;
            config.TextFont = TextFont.Text;
            config.TextSize = Convert.ToInt32(TextSize.Text);
            config.TextColor = TextColor.Value;
            config.WaterPhoto = WaterPhoto.Text;

            config.UploadFile = UploadFile.Text;
            config.AllowAnonymousComment = Convert.ToInt32(AllowAnonymousComment.Text);
            config.CommentDefaultStatus = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$CommentDefaultStatus");
            config.CommentRestrictTime = Convert.ToInt32(CommentRestrictTime.Text);
            config.EmailUserName = EmailUserName.Text;
            config.EmailPassword = EmailPassword.Text;
            config.EmailServer = EmailServer.Text;
            config.EmailServerPort = Convert.ToInt32(EmailServerPort.Text);
            config.ForbiddenName = ForbiddenName.Text;
            config.PasswordMinLength = Convert.ToInt32(PasswordMinLength.Text);
            config.PasswordMaxLength = Convert.ToInt32(PasswordMaxLength.Text);
            config.UserNameMinLength = Convert.ToInt32(UserNameMinLength.Text);
            config.UserNameMaxLength = Convert.ToInt32(UserNameMaxLength.Text);
            config.RegisterCheck = Convert.ToInt32(RegisterCheck.Text);
            config.RegisterType = Convert.ToInt32(RegisterType.Text);
            config.Agreement = Agreement.Text;
            config.FindPasswordTimeRestrict = Convert.ToDouble(FindPasswordTimeRestrict.Text);
            config.BindEmailTime = Convert.ToDouble(BindEmailTime.Text);
            #region 邮箱验证时间限制必须>1
            if (config.BindEmailTime < 1) config.BindEmailTime = 1;
            #endregion
            config.AllImageWidth = Convert.ToInt32(AllImageWidth.Text)<0?0: Convert.ToInt32(AllImageWidth.Text);
            config.AllImageIsNail = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$AllImageIsNail");
            #region 启用整站压缩 图片宽度不得小于600px
            if (config.AllImageIsNail == 1 && config.AllImageWidth < 600) ScriptHelper.Alert("整站图片压缩宽度不得小于600PX");
            #endregion
            config.QQ = QQ.Text;
            config.WeiXin = WeiXin.Text;
            config.MobileImage = MobileImage.Text;
            config.GTel = GTel.Text;
            config.SiteLink = SiteLink.Text;

            config.CnzzId = CnzzId.Text;
            config.HotKeyword = HotKeyword.Text;
            #region 各种logo尺寸
            config.IcoWidth = Convert.ToInt32(IcoWidth.Text) < 0 ? 0 : Convert.ToInt32(IcoWidth.Text);
            config.IcoHeight = Convert.ToInt32(IcoHeight.Text) < 0 ? 0 : Convert.ToInt32(IcoHeight.Text);
            config.LogoWidth = Convert.ToInt32(LogoWidth.Text) < 0 ? 0 : Convert.ToInt32(LogoWidth.Text);
            config.LogoHeight = Convert.ToInt32(LogoHeight.Text) < 0 ? 0 : Convert.ToInt32(LogoHeight.Text);
            config.MobileLogoWidth = Convert.ToInt32(MobileLogoWidth.Text) < 0 ? 0 : Convert.ToInt32(MobileLogoWidth.Text);
            config.MobileLogoHeight = Convert.ToInt32(MobileLogoHeight.Text) < 0 ? 0 : Convert.ToInt32(MobileLogoHeight.Text);
            config.MobilePhotoWidth = Convert.ToInt32(MobilePhotoWidth.Text) < 0 ? 0 : Convert.ToInt32(MobilePhotoWidth.Text);
            config.MobilePhotoHeight = Convert.ToInt32(MobilePhotoHeight.Text) < 0 ? 0 : Convert.ToInt32(MobilePhotoHeight.Text);
            config.WeixinWidth = Convert.ToInt32(WeixinWidth.Text) < 0 ? 0 : Convert.ToInt32(WeixinWidth.Text);
            config.WeixinHeight = Convert.ToInt32(WeixinHeight.Text) < 0 ? 0 : Convert.ToInt32(WeixinHeight.Text);
            config.UPLogo = UPLogo.Text;
            config.UPLogoWidth = Convert.ToInt32(UPLogoWidth.Text) < 0 ? 0 : Convert.ToInt32(UPLogoWidth.Text);
            config.UPLogoHeight = Convert.ToInt32(UPLogoHeight.Text) < 0 ? 0 : Convert.ToInt32(UPLogoHeight.Text);

            config.UserLoginPic = UserLoginPic.Text;
            config.UserLoginWidth = Convert.ToInt32(UserLoginWidth.Text) < 0 ? 0 : Convert.ToInt32(UserLoginWidth.Text);
            config.UserLoginHeight = Convert.ToInt32(UserLoginHeight.Text) < 0 ? 0 : Convert.ToInt32(UserLoginHeight.Text);
            config.UserRegisterPic = UserRegisterPic.Text;
            config.UserRegisterWidth = Convert.ToInt32(UserRegisterWidth.Text) < 0 ? 0 : Convert.ToInt32(UserRegisterWidth.Text);
            config.UserRegisterHeight = Convert.ToInt32(UserRegisterHeight.Text) < 0 ? 0 : Convert.ToInt32(UserRegisterHeight.Text);
          
            #endregion
            //积分抵现功能是否开启
            config.EnablePointPay = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$EnablePointPay");
            config.PointToMoney = Convert.ToInt32(PointToMoney.Text) < 0 ? 0 : Convert.ToInt32(PointToMoney.Text);
            if (config.EnablePointPay==1 &&(config.PointToMoney <= 0 || config.PointToMoney > 100))
            {
                ScriptHelper.Alert("积分抵现百分比必须大于0小于100" );              
            }
            //config.MoneyToPoint = Convert.ToInt32(MoneyToPoint.Text) < 0 ? 0 : Convert.ToInt32(MoneyToPoint.Text);
            //是否提供发票
            config.Invoicing = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$Invoicing");
            #region 订单状态改变是否发送通知（邮件 短信 ）
            config.PayOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$PayOrder");
            config.PayOrderEmail = RequestHelper.GetForm<int>("PayOrderEmail");
            config.PayOrderMsg = RequestHelper.GetForm<int>("PayOrderMsg");
            config.CancleOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$CancleOrder");
            config.CancleOrderEmail = RequestHelper.GetForm<int>("CancleOrderEmail");
            config.CancleOrderMsg = RequestHelper.GetForm<int>("CancleOrderMsg");
            config.CheckOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$CheckOrder");
            config.CheckOrderEmail = RequestHelper.GetForm<int>("CheckOrderEmail");
            config.CheckOrderMsg = RequestHelper.GetForm<int>("CheckOrderMsg");
            config.SendOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$SendOrder");
            config.SendOrderEmail = RequestHelper.GetForm<int>("SendOrderEmail");
            config.SendOrderMsg = RequestHelper.GetForm<int>("SendOrderMsg");
            config.ReceivedOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$ReceivedOrder");
            config.ReceivedOrderEmail = RequestHelper.GetForm<int>("ReceivedOrderEmail");
            config.ReceivedOrderMsg = RequestHelper.GetForm<int>("ReceivedOrderMsg");
            config.ChangeOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$ChangeOrder");
            config.ChangeOrderEmail = RequestHelper.GetForm<int>("ChangeOrderEmail");
            config.ChangeOrderMsg = RequestHelper.GetForm<int>("ChangeOrderMsg");
            config.ReturnOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$ReturnOrder");
            config.ReturnOrderEmail = RequestHelper.GetForm<int>("ReturnOrderEmail");
            config.ReturnOrderMsg = RequestHelper.GetForm<int>("ReturnOrderMsg");
            config.BackOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$BackOrder");
            config.BackOrderEmail = RequestHelper.GetForm<int>("BackOrderEmail");
            config.BackOrderMsg = RequestHelper.GetForm<int>("BackOrderMsg");
            config.RefundOrder = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$RefundOrder");
            config.RefundOrderEmail = RequestHelper.GetForm<int>("RefundOrderEmail");
            config.RefundOrderMsg = RequestHelper.GetForm<int>("RefundOrderMsg");
            #endregion
            //投票起止时间
            config.VoteStartDate = Convert.ToDateTime(VoteStartDate.Text);
            config.VoteEndDate = Convert.ToDateTime(VoteEndDate.Text);
            if ((config.VoteStartDate - config.VoteEndDate).Days > 0) ScriptHelper.Alert("投票结束日期不得小于开始日期");
            //订单付款时限,不小于0
            config.OrderPayTime = Convert.ToInt32(OrderPayTime.Text)<0?0:Convert.ToInt32(OrderPayTime.Text);
            //订单单自动收货天数
            config.OrderRecieveShippingDays = Convert.ToInt32(OrderRecieveShippingDays.Text) < 0 ? 0 : Convert.ToInt32(OrderRecieveShippingDays.Text);
        
            config.AdminLogo = AdminLogo.Text;
            config.LittlePrgCode = LittlePrgCode.Text;
            config.Qualification = Qualification.Text;
            config.BusinessHours = BusinessHours.Text;

            //安全码
            config.SecureKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            ////满立减
            //decimal orderMoney = 0; decimal orderDiscount = 0;
            //config.PayDiscount = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$PayDiscount");
            //if (config.PayDiscount == 1)
            //{
            //    if (!decimal.TryParse(OrderMoney.Text, out orderMoney) || !decimal.TryParse(OrderDisCount.Text, out orderDiscount))
            //    {
            //        ScriptHelper.Alert("满立减金额填写错误");
            //    }
            //    if (orderMoney <= orderDiscount)
            //    {
            //        ScriptHelper.Alert("满立减金额必须小于订单金额");
            //    }
            //}
            //config.OrderMoney = orderMoney;
            //config.OrderDisCount = orderDiscount;

            ShopConfig.UpdateConfigInfo(config);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }

    }
}
