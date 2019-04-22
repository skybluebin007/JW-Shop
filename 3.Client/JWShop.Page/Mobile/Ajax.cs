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

namespace JWShop.Page.Mobile
{
    public class Ajax : AjaxBasePage
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "MessageSubmit":
                    //MessageSubmit(RequestHelper.GetQueryString<string>("type"));
                    break;
                case "GetVerifyCode":
                    GetVerifyCode();
                    break;
            }
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        private void GetVerifyCode()
        {
            string mobile = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("mobile"));
            bool isSuccess = false;
            string msg = "";

            if (string.IsNullOrEmpty(mobile))
            {
                ResponseHelper.Write("error|请输入手机号码");
                ResponseHelper.End();
            }

            isSuccess = true;
            //isSuccess = WebService.GetHttp.PostSms(mobile, out msg);
            if (isSuccess)
            {
                CookiesHelper.AddCookie("verify_send", DateTime.Now.ToString(), 1, TimeType.Minute);
                ResponseHelper.Write("ok|");
                ResponseHelper.End();
            }
            else
            {
                ResponseHelper.Write("error|" + msg);
                ResponseHelper.End();
            }
        }

    }
}