using System;
using System.Web;
using System.Text.RegularExpressions;
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
    public partial class UserAdd : JWShop.Page.AdminBasePage
    {

        protected int country = 0;
        protected int province = 0;
        protected int city = 0;
        protected int district = 0;
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //会员类型
                usertype.DataSource = EnumHelper.ReadEnumList<UserType>();
                usertype.DataTextField = "ChineseName";
                usertype.DataValueField = "Value";
                usertype.DataBind();

                int userID = RequestHelper.GetQueryString<int>("ID");
                UserRegion.DataSource = RegionBLL.ReadRegionUnlimitClass();
                if (userID != int.MinValue)
                {
                    CheckAdminPower("ReadUser", PowerCheckType.Single);
                    user = UserBLL.Read(userID);
                    user.UserName = System.Web.HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
                    UserName.Text = user.UserName;
                    UserPassword.Text = user.UserPassword;
                    Email.Text = user.Email;
                    Sex.Text = user.Sex.ToString();
                    Introduce.Text = user.Introduce;
                    Photo.Text = user.Photo;
                    MSN.Text = user.MSN;
                    QQ.Text = user.QQ;
                    Tel.Text = user.Tel;
                    Mobile.Text = user.Mobile;
                    UserRegion.ClassID = user.RegionId;
                    Address.Text = user.Address;
                    Birthday.Text = user.Birthday;
                    Status.Text = user.Status.ToString();
                    RealName.Text = user.RealName;
                    UserName.Enabled = false;
                    Add.Visible = false;
                    Sex.Enabled = false;

                    usertype.SelectedValue = user.UserType.ToString();
                    FromUserid.Text = user.Recommend_UserId.ToString();
                }
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string userName = UserName.Text;
            int userID = RequestHelper.GetQueryString<int>("ID");
            if (userID == 0)
            {
                Regex rg = new Regex("^([a-zA-Z0-9_\u4E00-\u9FA5])+$");
                if (!rg.IsMatch(userName))
                {
                    ScriptHelper.Alert("用户名只能包含字母、数字、下划线、中文", RequestHelper.RawUrl);
                }
            }
            UserInfo user = new UserInfo();
            user.Id = userID;
            user.UserName = userName;

            user.UserType = Convert.ToInt32(usertype.SelectedValue);
            if (string.IsNullOrEmpty(FromUserid.Text))
            {
                ScriptHelper.Alert("非经销商类型用户，需填写所属经销商ID");
                return;
            }

            UserInfo user1 = new UserInfo();
            user1 = UserBLL.Read(Convert.ToInt32(FromUserid.Text));

            if (user.UserType == 3)
            {
                if (string.IsNullOrEmpty(UserRegion.ClassID))
                {
                    ScriptHelper.Alert("经销商类型用户，必须选择所属地区");
                    return;
                }
                
            }
            else
            {
                if (user1.Id <= 0 || user1.UserType != 3)
                {
                    ScriptHelper.Alert("非经销商类型用户，需填写所属经销商ID");
                    return;
                }
            }
            

            if (user.Id <=0)
            {
                //新增
                user.UserPassword = StringHelper.Password(UserPassword.Text.Trim(), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            }
            else
            {//修改
                var _theUser = UserBLL.Read(userID);
                if(user.UserType==3)
                    user.Recommend_UserId = 0;
                else
                    user.Recommend_UserId = _theUser.Recommend_UserId;

                user.UserName = _theUser.UserName;
                user.UserPassword = _theUser.UserPassword;
                user.RegisterDate = _theUser.RegisterDate;
                user.LoginTimes = _theUser.LoginTimes;
                user.LastLoginDate = _theUser.LastLoginDate;
                user.FindDate = _theUser.FindDate;
                user.OpenId = _theUser.OpenId;
                user.HasRegisterCoupon = _theUser.HasRegisterCoupon;
                user.HasBirthdayCoupon = _theUser.HasBirthdayCoupon;
                user.GetBirthdayCouponDate = _theUser.GetBirthdayCouponDate;
                //分销商小程序推广码
                user.ProviderName = _theUser.ProviderName;
            }
            user.Email = Email.Text;
            user.Sex = Convert.ToInt32(Sex.Text);
            user.Introduce = Introduce.Text;
            user.Photo = Photo.Text;
            user.MSN = MSN.Text;
            user.QQ = QQ.Text;
            user.Tel = Tel.Text;
            user.Mobile = Mobile.Text;
            user.RegionId = UserRegion.ClassID;
            user.Address = Address.Text;
            user.Birthday = Birthday.Text;
            user.RealName = RealName.Text;            

            user.Status = Convert.ToInt32(Status.Text);
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (user.Id == int.MinValue)
            {
                CheckAdminPower("AddUser", PowerCheckType.Single);
                int id = UserBLL.Add(user);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("User"), id);

                if (user.UserType != 3)
                {
                    if (user1.Id > 0 && user1.UserType == 3)
                        UserBLL.ChangeUserToDistributor(id, user1.Id);
                }
            }
            else
            {
                CheckAdminPower("UpdateUser", PowerCheckType.Single);
                UserBLL.Update(user);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("User"), user.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");

                if (user.UserType != 3 && user.Recommend_UserId!= Convert.ToInt32(FromUserid.Text))
                {
                    if (user1.Id > 0 && user1.UserType == 3)
                        UserBLL.ChangeUserToDistributor(user.Id, user1.Id);
                }
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}