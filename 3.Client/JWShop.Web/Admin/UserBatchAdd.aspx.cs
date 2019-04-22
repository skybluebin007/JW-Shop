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
using System.Linq;
using System.Data;
using System.Text;

namespace JWShop.Web.Admin
{
    public partial class UserBatchAdd : JWShop.Page.AdminBasePage
    {  /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestHelper.GetQueryString<string>("Action");
            string proNumbers = RequestHelper.GetQueryString<string>("productNumber");
            if (action != string.Empty)
            {
                switch (action)
                {
                    case "FileUpload":
                        BatchImport();
                        break; 
                    case "Submit":
                        Submit();
                        break;               
                    case "CheckLoginName":
                        CheckLoginName();
                        break;
                    case"CheckLoginNames":
                        CheckLoginNames();
                        break;
                    default:
                        break;
                }
            }

        }
        //提交
        protected void Submit()
        {
            string result = "ok";
            try
            {

                var realnames = RequestHelper.GetForm<string>("realname").Split(',');
                var names = RequestHelper.GetForm<string>("username").Split(',');
                var mobiles = RequestHelper.GetForm<string>("mobile").Split(',');
                var birthdays = RequestHelper.GetForm<string>("birthday").Split(',');
                //var emails = RequestHelper.GetForm<string>("email").Split(',');
                //var tels = RequestHelper.GetForm<string>("tel").Split(',');
                //var qqs = RequestHelper.GetForm<string>("qq").Split(',');


                List<object[]> entities = new List<object[]>();
                for (int i = 0; i < names.Length; i++)
                {
                    UserInfo user = new UserInfo();

                    user.RealName = realnames[i];
                    user.UserName = names[i];
                    user.Mobile = mobiles[i];
                    user.Birthday = birthdays[i];
                    //user.Email = emails[i];
                    //user.Tel = tels[i];
                    //user.QQ = qqs[i];
                    //初始密码123456
                    user.UserPassword = StringHelper.Password("123456", (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                    user.RegisterIP = ClientHelper.IP;
                    user.RegisterDate = RequestHelper.DateNow;
                    user.LastLoginIP = ClientHelper.IP;
                    user.LastLoginDate = RequestHelper.DateNow;
                    user.FindDate = RequestHelper.DateNow;
                    user.Sex = (int)SexType.Secret;
                    user.Status = (int)JWShop.Entity.UserStatus.Normal;
                    user.HasRegisterCoupon = 0;
                    user.HasBirthdayCoupon = 0;
                    //检查姓名是否存在
                    if (string.IsNullOrEmpty(user.RealName))
                    {
                        result = "姓名必填";
                    }

                    //检查用户名/微信昵称是否存在
                    if (string.IsNullOrEmpty(user.UserName))
                    {
                        result = "用户名/微信昵称必填";
                        //Response.Write(result);
                        //Response.End();
                    }
                    string forbiddinName = ShopConfig.ReadConfigInfo().ForbiddenName;
                    if (forbiddinName != string.Empty)
                    {
                        foreach (string TempName in forbiddinName.Split('|'))
                        {
                            if (user.UserName.IndexOf(TempName.Trim()) != -1)
                            {
                                result = "用户名/微信昵称:[" + user.UserName+"]含有非法字符";                             
                                //Response.Write(result);
                                //Response.End();
                            }
                        }
                    }
                    if (!UserBLL.UniqueUser(user.UserName))
                    {
                        result = "用户名/微信昵称:[" + user.UserName + "]已经注册";                    
                        //Response.Write(result);
                        //Response.End();
                    }
                    if (string.IsNullOrEmpty(user.Mobile))
                    {
                        result = "手机必填";
                        //Response.Write(result);
                        //Response.End();
                    }
                    if (!ShopCommon.CheckMobile(user.Mobile))
                    {
                        result = "手机号码:["+user.Mobile+"]错误";
                        //Response.Write(result);
                        //Response.End();
                    }
                    if (!UserBLL.CheckMobile(user.Mobile, 0))
                    {
                        result = "手机号码:[" + user.Mobile + "]已经注册";
                        //Response.Write(result);
                        //Response.End();
                    }
                    DateTime date = DateTime.Now;
                    if(!string.IsNullOrEmpty(user.Birthday) && !DateTime.TryParse(user.Birthday,out date))
                    {
                        result = "生日:["+user.Birthday+"]格式错误";
                    }
                    //if (string.IsNullOrEmpty(user.Email))
                    //{
                    //    result = "邮箱:"+user.Email+"必填";
                    //    //Response.Write(result);
                    //    //Response.End();
                    //}
                    //if (!UserBLL.CheckEmail(user.Email))
                    //{
                    //    result = "邮箱:[" + user.Email + "]已被注册";
                    //    //Response.Write(result);
                    //    //Response.End();
                    //}
                    else
                    {
                        entities.Add(new object[] { user });
                    }
                }
                if (result == "ok")
                {
                    UserBLL.AddBatch(entities);
                }
            }
            catch (Exception ex)
            {
                result = "error";              
            }
            finally
            {
                Response.Clear();
                Response.Write(result);
                Response.End();
            }

        }

        //用户导入
        public void BatchImport()
        {
            bool isSuccess; string msg;
            DataTable dt = ExcelToDataTable.Convert(StringHelper.AddSafe(RequestHelper.GetForm<string>("file")), out isSuccess, out msg);
            if (!isSuccess)
            //return Json(new { flag = false, msg = msg }, JsonRequestBehavior.AllowGet);
            {
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, msg = msg }));

            }
            else
            {
                List<object> dataList = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    string realname = dr[0] + "";
                    string username = dr[1] + "";
                    string mobile = dr[2] + "";
                    string birthday = dr[3] + "";
                    //string email = dr[2] + "";
                    //string tel = dr[3] + "";
                    //string qq = dr[4] + "";
                    if (string.IsNullOrEmpty(realname))
                    {
                        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, msg = "姓名不能为空" }));
                        Response.End();
                    }
                    if (string.IsNullOrEmpty(username))
                    {
                        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, msg = "用户名/微信昵称不能为空" }));
                        Response.End();
                    }
                    if (string.IsNullOrEmpty(mobile))
                    {
                        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, msg = "手机号不能为空" }));
                        Response.End();
                    }
                    DateTime date = DateTime.Now;
                    if(!DateTime.TryParse(birthday,out date))
                    {
                        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, msg = "生日必须为日期格式(如1999-01-01)" }));
                        Response.End();
                    }
                    //if (string.IsNullOrEmpty(email))
                    //{
                    //    Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, msg = "Email不能为空" }));
                    //    Response.End();
                    //}

                    //dataList.Add(new { row = dt.Rows.IndexOf(dr),RealName=realname, UserName = username, Mobile = mobile, Email = email, Tel = tel,QQ=qq });
                    dataList.Add(new { row = dt.Rows.IndexOf(dr), RealName = realname, UserName = username, Mobile = mobile,Birthday=birthday });
                }

                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = true, msg = "", list = dataList }));
            }
            Response.End();
        }

      
        /// <summary>
        /// 验证用户名、手机号、邮箱是否存在--单个验证
        /// </summary>
        protected void CheckLoginName()
        {
            object json = new { ok = "" };
            string _type = "用户名";
            string userName = RequestHelper.GetQueryString<string>("loginName");
            int loginType = RequestHelper.GetQueryString<int>("loginType");
            if (loginType == 2) { _type = "手机"; }
            if (loginType == 3) { _type = "邮箱"; }
            UserInfo user = UserBLL.Read(userName);
            bool isUnique = user.Id<=0;
            if (!isUnique) json = new { error = _type+"已注册" };           
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json));
            Response.End();
        }
        /// <summary>
        /// 验证用户名、手机号、邮箱是否存在--集合验证
        /// </summary>
        protected void CheckLoginNames()
        {
            bool flag = true;
            string msg = string.Empty;
            string userNames = RequestHelper.GetQueryString<string>("userNames");
            string[] _usernames = userNames.Split('|');
            string mobiles = RequestHelper.GetQueryString<string>("mobiles");
            string[] _mobiles = mobiles.Split('|');
            string emails = RequestHelper.GetQueryString<string>("emails");
            string[] _emails = emails.Split('|');
            if (flag) {
                flag = UserBLL.CheckUserNames(_usernames);
                if (!flag) msg = "列表中有已注册的用户名";
            }
            if (flag)
            {
                flag = UserBLL.CheckMobiles(_mobiles);
                if (!flag) msg = "列表中有已注册的手机号";
            }
            //if (flag)
            //{
            //    flag = UserBLL.CheckEmails(_emails);
            //    if (!flag) msg = "列表中有已注册的邮箱";
            //}
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = flag,msg=msg }));
            Response.End();
        }
    }
}