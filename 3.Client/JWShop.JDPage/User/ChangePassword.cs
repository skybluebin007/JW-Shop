﻿using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page
{
    public class ChangePassword : UserBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "CheckPassword":
                    CheckPassword();
                    break;
            }
        }

        protected void CheckPassword()
        {
            string result = string.Empty;
            string oldPassword = StringHelper.Password(RequestHelper.GetForm<string>("OldPassword"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword1"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword2 = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword2"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            if (oldPassword != CurrentUser.UserPassword) result = "旧密码错误";
            if (result == string.Empty)
            {
            if (string.IsNullOrEmpty(newPassword)) result = "请输入新密码";
            }
            if (result == string.Empty)
            {
                if (!string.Equals(newPassword,newPassword2)) result = "确认密码与新密码不一致";
            }
            result = string.Format(@"""{0}"": ""{1}""", string.IsNullOrEmpty(result) ? "ok" : "error", result);
            ResponseHelper.Write("{" + result + "}");
            ResponseHelper.End();
        }

        protected override void PostBack()
        {
            string oldPassword = StringHelper.Password(RequestHelper.GetForm<string>("OldPassword"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword1"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword2 = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword2"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            if (oldPassword != CurrentUser.UserPassword)
            {
                ScriptHelper.AlertFront("旧密码错误", RequestHelper.RawUrl);
            }
            else
            {
                if (string.IsNullOrEmpty(RequestHelper.GetForm<string>("UserPassword1")))
                { ScriptHelper.AlertFront("请输入新密码", RequestHelper.RawUrl); }
                else
                {
                    if (!string.Equals(RequestHelper.GetForm<string>("UserPassword1"), RequestHelper.GetForm<string>("UserPassword2"))) 
                    { ScriptHelper.AlertFront("确认密码与新密码不一致", RequestHelper.RawUrl); }
                    else
                    {

                        UserBLL.ChangePassword(base.UserId, oldPassword, newPassword);
                        CurrentUser.UserPassword = newPassword;
                        ScriptHelper.AlertFront("密码修改成功", RequestHelper.RawUrl);
                    }
                }
            }
        }
    }
}