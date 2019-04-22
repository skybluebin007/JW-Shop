using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using GeetestSDK;
using Newtonsoft.Json;
using System.Web.Security;
using System.Threading.Tasks;

namespace JWShop.Web.Admin
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 结果
        /// </summary>
        protected string result = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string checkCode = RequestHelper.GetQueryString<string>("CheckCode");
                if (checkCode != string.Empty)
                {
                    string decode = StringHelper.Decode(checkCode, ShopConfig.ReadConfigInfo().SecureKey);
                    if (decode.IndexOf('|') > 0)
                    {
                        int adminID = Convert.ToInt32(decode.Split('|')[0]);
                        string email = decode.Split('|')[1];
                        string loginName = decode.Split('|')[2];                    
                        string safeCode = decode.Split('|')[3];
                       AdminInfo admin =AdminBLL.Read(adminID);
                       if (admin.Id > 0 && admin.Name == loginName  && admin.Email == email &&  safeCode == admin.SafeCode)
                        {
                           //邮件链接15分钟失效
                            if (admin.FindDate.AddMinutes(15) < RequestHelper.DateNow)
                            {
                                errorMessage = "信息过时，请重新申请";
                            }
                        }
                        else
                        {
                            errorMessage = "错误的信息";
                        }
                    }
                    else
                    {
                        errorMessage = "错误的信息";
                    }
                }
                result = RequestHelper.GetQueryString<string>("Result");
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {           
            //如果账号不存在
            if (!string.Equals(NewPassword.Text, NewPassword2.Text, StringComparison.OrdinalIgnoreCase))
            {
                ScriptHelper.AlertFront("两次密码不一致");
            }
            else
            {
                #region 滑块验证码
                GeetestLib geetest = new GeetestLib("b46d1900d0a894591916ea94ea91bd2c", "36fc3fe98530eea08dfc6ce76e3d24c4");
                Byte gt_server_status_code = (Byte)Session[GeetestLib.gtServerStatusSessionKey];
                String userID = (String)Session["userID"];
                int result = 0;
                String challenge = Request.Form.Get(GeetestLib.fnGeetestChallenge);
                String validate = Request.Form.Get(GeetestLib.fnGeetestValidate);
                String seccode = Request.Form.Get(GeetestLib.fnGeetestSeccode);
                try
                {
                    if (gt_server_status_code != null && gt_server_status_code == 1) result = geetest.enhencedValidateRequest(challenge, validate, seccode, userID);
                    else result = geetest.failbackValidateRequest(challenge, validate, seccode);
                }
                catch (Exception ex)
                {
                    result = -1;//极验验证码出错，不进行验证
                }
                if (result == 1 || result == -1)
                {// 验证通过,重置密码
               
                    string checkCode = RequestHelper.GetForm<string>("CheckCode");
                    string decode = StringHelper.Decode(checkCode, ShopConfig.ReadConfigInfo().SecureKey);
                    int adminID = Convert.ToInt32(decode.Split('|')[0]);
                    string newPassword = StringHelper.Password(NewPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType); 
                    // 重置密码
                    AdminBLL.ChangePassword(adminID, newPassword);
                    Task.Run(() => {
                        //安全码
                        ShopConfigInfo config = ShopConfig.ReadConfigInfo();
                        config.SecureKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        ShopConfig.UpdateConfigInfo(config);
                    });
                    //清空safecode，finddate恢复
                    AdminBLL.ChangeAdminSafeCode(adminID, string.Empty, RequestHelper.DateNow);
                    //错误次数清零，解锁
                    AdminBLL.UpdateStatus(adminID);
                   string msg = "恭喜您，密码修改成功！" + "&nbsp;&nbsp;点击<a href=\"/admin/Login.aspx\" style=\"color: #1dd42b;font-size: larger;\">\"使用新密码登录\"</a>";
                    //清除原有的user Cookies
                    CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);
                    CookiesHelper.DeleteCookie("AdminSign");

                    ResponseHelper.Redirect("/admin/ResetPassword.aspx?Result=" + Server.UrlEncode(msg));

                }
                else
                {
                    //验证失败
                    ScriptHelper.AlertFront("图片验证失败，请拖动图片滑块重新验证。");
                }
                #endregion

            }



        }
    }
}