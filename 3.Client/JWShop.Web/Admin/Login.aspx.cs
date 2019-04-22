using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using GeetestSDK;
using Newtonsoft.Json;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class Login : System.Web.UI.Page
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (RequestHelper.GetQueryString<string>("Action") == "GeetestValidate") GeetestValidate();
            if (!IsPostBack)
            {
                if (RequestHelper.UserAgent()) Response.Redirect("/mobileadmin/Login.html");
                //检查待付款订单是否超时失效，超时则更新为失效状态
                OrderBLL.CheckOrderPayTime();
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string loginName = StringHelper.SearchSafe(AdminName.Text);
            string loginPass = StringHelper.SearchSafe(Password.Text);
            var theLoginAdmin = AdminBLL.Read(loginName);
            //如果登录日期与上次登录日期不是同一天，更新登录记录，清空错误次数，解除锁定
            if (theLoginAdmin.Id > 0 && (DateTime.Now-theLoginAdmin.LastLoginDate).Days>0)
            {
                AdminBLL.UpdateLogin(theLoginAdmin.Id, RequestHelper.DateNow, ClientHelper.IP);
            }
            bool remember = Remember.Checked;
            loginPass = StringHelper.Password(loginPass, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            AdminInfo admin = AdminBLL.CheckLogin(loginName, loginPass);
            if (admin.Id > 0)
            {
                
                // 如果账户未锁定
                if (admin.Status == (int)BoolType.True)
                {
                    #region 滑块验证码
                    int result = 0;
                        try
                        { 
                        GeetestLib geetest = new GeetestLib("b46d1900d0a894591916ea94ea91bd2c", "36fc3fe98530eea08dfc6ce76e3d24c4");
                        Byte gt_server_status_code = (Byte)Session[GeetestLib.gtServerStatusSessionKey];
                        String userID = (String)Session["userID"];
                      
                        String challenge = Request.Form.Get(GeetestLib.fnGeetestChallenge);
                        String validate = Request.Form.Get(GeetestLib.fnGeetestValidate);
                        String seccode = Request.Form.Get(GeetestLib.fnGeetestSeccode);
                       
                            if (gt_server_status_code != null && gt_server_status_code == 1) result = geetest.enhencedValidateRequest(challenge, validate, seccode, userID);
                            else result = geetest.failbackValidateRequest(challenge, validate, seccode);
                        }
                        catch (Exception ex)
                        {
                            result = -1;//极验验证码出错，不进行验证
                        }
                        if (result == 1 || result == -1)
                        {
                    #endregion

                            string randomNumber = Guid.NewGuid().ToString();
                            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + randomNumber + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
                            string value = sign + "|" + admin.Id.ToString() + "|" + admin.Name + "|" + admin.GroupId.ToString() + "|" + randomNumber;
                            if (remember)
                            {
                                CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, value, 1, TimeType.Year);
                            }
                            else
                            {
                                CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, value);
                            }
                            string signvalue = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent + AdminBLL.Read(admin.Id).Password, "MD5");
                            CookiesHelper.AddCookie("AdminSign", signvalue);
                            AdminBLL.UpdateLogin(admin.Id, RequestHelper.DateNow, ClientHelper.IP);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("LoginSystem"));
                            ResponseHelper.Redirect("/Admin");
                        }
                        else
                        {
                            //验证失败
                            string errorMsg = " *图片验证失败，请拖动图片滑块重新验证。";
                            ResponseHelper.Redirect("/Admin/login.aspx?errorMsg=" + errorMsg);
                        }
                   
                }
                else
                {//如果账户已锁定
                    string errorMsg = " *温馨提示：您一天内登录错误达到3次，已被锁定，可联系网站客服解锁，也可次日重新登录。";
                    ResponseHelper.Redirect("/Admin/login.aspx?errorMsg="+errorMsg);
                }
            }          
            else
            {
                //登录失败，失败次数加1。如果失败超过3次，则锁定账户
                AdminBLL.UpdateLogin(loginName, RequestHelper.DateNow, ClientHelper.IP, 3);
                AdminLogBLL.Add("管理员："+loginName+"在"+RequestHelper.DateNow+"登陆网站后台失败，登陆IP："+ClientHelper.IP);
                if (theLoginAdmin.Id>0 && theLoginAdmin.LoginErrorTimes >= 3)
                {
                    string errorMsg = " *温馨提示：您一天内登录错误达到3次，已被锁定，可联系网站客服解锁，也可次日重新登录。";
                    ResponseHelper.Redirect("/Admin/login.aspx?errorMsg=" + errorMsg);
                }
                else
                {
                    //ScriptHelper.AlertFront("登录失败", RequestHelper.RawUrl);
                    string errorMsg = " *用户名或密码错误，登录失败。";
                    ResponseHelper.Redirect("/Admin/login.aspx?errorMsg=" + Server.UrlEncode(errorMsg));
                }
            }
        }

        /// <summary>
        /// 极验验证,1表示成功
        /// </summary>
        protected void GeetestValidate() {
            
            string loginName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("AdminName"));
            string loginPass = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Password"));
            #region 滑块验证码
           
            GeetestLib geetest = new GeetestLib("b46d1900d0a894591916ea94ea91bd2c", "36fc3fe98530eea08dfc6ce76e3d24c4");
            Byte gt_server_status_code = (Byte)Session[GeetestLib.gtServerStatusSessionKey];
            String userID = (String)Session["userID"];
            int result = 0;
            String challenge = Request.Form.Get(GeetestLib.fnGeetestChallenge);
            String validate = Request.Form.Get(GeetestLib.fnGeetestValidate);
            String seccode = Request.Form.Get(GeetestLib.fnGeetestSeccode);
            if (gt_server_status_code == 1) result = geetest.enhencedValidateRequest(challenge, validate, seccode, userID);
            else result = geetest.failbackValidateRequest(challenge, validate, seccode);
            if (result != 1)
            {
                string errorMsg = " *验证失败，请重新验证。";
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { flag = "no", msg = errorMsg }));
                Response.End();
            }
           
            #endregion

            var theLoginAdmin = AdminBLL.Read(loginName);
            //如果登录日期与上次登录日期不是同一天，更新登录记录，清空错误次数，解除锁定
            if (theLoginAdmin.Id > 0 && (DateTime.Now - theLoginAdmin.LastLoginDate).Days > 0)
            {
                AdminBLL.UpdateLogin(theLoginAdmin.Id, RequestHelper.DateNow, ClientHelper.IP);
            }
            bool remember = Remember.Checked;
            loginPass = StringHelper.Password(loginPass, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            AdminInfo admin = AdminBLL.CheckLogin(loginName, loginPass);
            if (admin.Id > 0)
            {

                // 如果账户未锁定
                if (admin.Status == (int)BoolType.True)
                {
                    string randomNumber = Guid.NewGuid().ToString();
                    string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + randomNumber + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
                    string value = sign + "|" + admin.Id.ToString() + "|" + admin.Name + "|" + admin.GroupId.ToString() + "|" + randomNumber;
                    if (remember)
                    {
                        CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, value, 1, TimeType.Year);
                    }
                    else
                    {
                        CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, value);
                    }
                    string signvalue = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent + AdminBLL.Read(admin.Id).Password, "MD5");
                    CookiesHelper.AddCookie("AdminSign", signvalue);
                    AdminBLL.UpdateLogin(admin.Id, RequestHelper.DateNow, ClientHelper.IP);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("LoginSystem"));
                    //ResponseHelper.Redirect("/Admin");
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { flag="ok",msg=""}));
                    Response.End();
                   
                }
                else
                {//如果账户已锁定
                    string errorMsg = " *温馨提示：您一天内登录错误达到5次，已被锁定，可联系网站客服解锁，也可次日重新登录。";
                    //ResponseHelper.Redirect("/Admin/login.aspx?errorMsg=" + errorMsg);
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { flag = "no", msg = errorMsg }));
                    Response.End();
                }
            }
            else
            {
                //登录失败，失败次数加1。如果失败超过5次，则锁定账户
                AdminBLL.UpdateLogin(loginName, RequestHelper.DateNow, ClientHelper.IP, 5);

                if (theLoginAdmin.Id > 0 && theLoginAdmin.LoginErrorTimes >= 5)
                {
                    string errorMsg = " *温馨提示：您一天内登录错误达到5次，已被锁定，可联系网站客服解锁，也可次日重新登录。";
                    //ResponseHelper.Redirect("/Admin/login.aspx?errorMsg=" + errorMsg);
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { flag = "no", msg = errorMsg }));
                    Response.End();
                }
                else
                {                  
                    string errorMsg = " *用户名或密码错误，登录失败。";
                    //ResponseHelper.Redirect("/Admin/login.aspx?errorMsg=" + errorMsg);
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { flag = "no", msg = errorMsg }));
                    Response.End();
                }
            }
        }
     
    }
}