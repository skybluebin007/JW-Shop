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


namespace JWShop.Web.Admin
{
    public partial class FindPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string loginName = StringHelper.SearchSafe(AdminName.Text);
            string loginEmial = StringHelper.SearchSafe(Email.Text);
            var admin = AdminBLL.Read(loginName);
            //如果账号不存在
            if (admin.Id <= 0)
            {
                ScriptHelper.AlertFront("账号不存在");
            }
            //如果账号不存在
            if (!string.Equals(admin.Email, loginEmial,StringComparison.OrdinalIgnoreCase))
            {
                ScriptHelper.AlertFront("账号、邮箱不匹配");
            }

            if (admin.Id > 0 && string.Equals(admin.Email, loginEmial, StringComparison.OrdinalIgnoreCase))
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
                {// 验证通过,发送邮件
                    string tempSafeCode = Guid.NewGuid().ToString();
                   AdminBLL.ChangeAdminSafeCode(admin.Id, tempSafeCode, RequestHelper.DateNow);
                    string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/Admin/ResetPassword.aspx?CheckCode=" + StringHelper.Encode(admin.Id + "|" + admin.Email + "|" + admin.Name + "|" + tempSafeCode, ShopConfig.ReadConfigInfo().SecureKey);
                    EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent("FindPassword");
                    EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                    emailSendRecord.Title = emailContent.EmailTitle;
                    emailSendRecord.Content = emailContent.EmailContent.Replace("$Url$", url);
                    emailSendRecord.IsSystem = (int)BoolType.True;
                    emailSendRecord.EmailList = admin.Email;
                    emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                    emailSendRecord.SendStatus = (int)SendStatus.No;
                    emailSendRecord.AddDate = RequestHelper.DateNow;
                    emailSendRecord.SendDate = RequestHelper.DateNow;
                    emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                    EmailSendRecordBLL.SendEmail(emailSendRecord);
                   string  emailResult = "您的申请已提交，请在15分钟内登录邮箱重设你的密码,！<a href=\"http://mail." + admin.Email.Substring(admin.Email.IndexOf("@") + 1) + "\"  target=\"_blank\">马上登录</a>";
                   ResponseHelper.Redirect("/admin/FindPassword.aspx?emailResult=" + Server.UrlEncode(emailResult));
                
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