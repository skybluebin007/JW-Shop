using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Lab
{
    public class UserAdd : UserBasePage
    {
        protected bool canChangeUserName = true;
        protected SingleUnlimitClass regionId = new SingleUnlimitClass();

        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "CheckUserName":
                    CheckUser("用户名");
                    break;
                case "CheckMobile":
                    CheckUser("手机号码");
                    break;
                case "Submit":
                    Submit();
                    break;
            }

            //如果用户已订购过商品，则用户名不能再被修改
            if (OrderBLL.ReadCount(base.UserId) > 0) canChangeUserName = false;
            regionId.DataSource = RegionBLL.ReadRegionUnlimitClass();
            regionId.ClassID = CurrentUser.RegionId;
        }

        protected void CheckUser(string field)
        {
            string result = string.Empty;
            string value = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("value"));

            if (!string.IsNullOrEmpty(value) && !UserBLL.UniqueUser(value, base.UserId)) result = field + "已被占用";

            result = string.Format(@"""{0}"": ""{1}""", string.IsNullOrEmpty(result) ? "ok" : "error", result);
            ResponseHelper.Write("{" + result + "}");
            ResponseHelper.End();
        }

        protected void Submit()
        {
            string userName = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName"));
            CurrentUser.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            CurrentUser.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
            string photo = StringHelper.AddSafe(RequestHelper.GetForm<string>("Photo"));

            if (!string.IsNullOrEmpty(userName))
            {
                int firstCharIsNumber = 0;
                int.TryParse(userName.Substring(0, 1), out firstCharIsNumber);
                if (firstCharIsNumber > 0)
                {
                    ResponseHelper.Write("error|用户名不能以数字开头");
                    ResponseHelper.End();
                }
                if (!UserBLL.UniqueUser(userName, base.UserId))
                {
                    ResponseHelper.Write("error|用户名已被占用");
                    ResponseHelper.End();
                }
                if (OrderBLL.ReadCount(base.UserId) > 0)
                {
                    ResponseHelper.Write("error|用户名更新失败");
                    ResponseHelper.End();
                }
                CurrentUser.UserName = userName;
            }

            if (!string.IsNullOrEmpty(photo))
            {
                CurrentUser.Photo = photo;
                CookiesHelper.AddCookie("UserPhoto", photo);
            }
            else CurrentUser.Photo = UserBLL.ReadUserPhoto();

            CookiesHelper.AddCookie("UserEmail", CurrentUser.Email);

            if (CurrentUser.UserType == (int)UserType.Provider)
            {
                CurrentUser.ProviderName = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderName"));
                CurrentUser.ProviderBankNo = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderBankNo"));
                CurrentUser.ProviderTaxRegistration = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderTaxRegistration"));
                CurrentUser.ProviderCorporate = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderCorporate"));
                CurrentUser.ProviderLinkerTel = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderLinkerTel"));
                CurrentUser.ProviderFax = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderFax"));
                CurrentUser.ProviderAddress = StringHelper.AddSafe(RequestHelper.GetForm<string>("ProviderAddress"));
                CurrentUser.RegionId = regionId.ClassID;
            }
            UserBLL.Update(CurrentUser);

            //修改用户名，更新cookie
            if (!string.IsNullOrEmpty(userName))
            {
                base.UserName = userName;
                UserBLL.AddUserCookie(CurrentUser);
            }

            ResponseHelper.Write("ok|更新成功");
            ResponseHelper.End();
        }
    }
}