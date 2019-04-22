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
    public partial class UserDuihuanAdd : JWShop.Page.AdminBasePage
    {
        protected OtherDuihuanInfo OtherDuihuan = new OtherDuihuanInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                if (id != int.MinValue)
                {
                    //CheckAdminPower("ReadUser", PowerCheckType.Single);
                    OtherDuihuan = OtherDuihuanBLL.Read(id);
                    userid.Text = OtherDuihuan.userid.ToString();
                    name.Text = OtherDuihuan.truename;
                    mobile.Text = OtherDuihuan.mobile;
                    integral.Text = OtherDuihuan.integral;
                    note.Text = OtherDuihuan.note;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userid.Text))
            {
                ScriptHelper.Alert("会员ID不能为空");
                return;
            }

            OtherDuihuanInfo model = new OtherDuihuanInfo();
            model.id = RequestHelper.GetQueryString<int>("ID");
            if (model.id > 0)
            {
                //model = OtherDuihuanBLL.Read(model.id);
                //model.truename = name.Text;
                //model.mobile = mobile.Text;
                //model.note = note.Text;
                //model.integral = integral.Text;
                //model.adminid = Cookies.Admin.GetAdminID(true);
                //model.addtime = RequestHelper.DateNow;

                //OtherDuihuanBLL.Update(model);

                //AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), "修改兑换记录", model.id);
            }
            else
            {
                UserInfo usermodel = UserBLL.Read(Convert.ToInt32(userid.Text));
                if (usermodel.Id > 0)
                {
                    if (string.IsNullOrEmpty(integral.Text) || Convert.ToInt32(integral.Text) <= 0)
                    {
                        ScriptHelper.Alert("请填写兑换积分");
                        return;
                    }

                    if (usermodel.PointLeft < Convert.ToInt32(integral.Text))
                    {
                        ScriptHelper.Alert("会员可用积分不足");
                        return;
                    }

                    model.userid = usermodel.Id;
                    model.truename = name.Text;
                    model.mobile = mobile.Text;
                    model.note = note.Text;
                    model.integral = integral.Text;
                    model.adminid = Cookies.Admin.GetAdminID(true);
                    model.addtime = RequestHelper.DateNow;

                    //CheckAdminPower("UpdateUserMessage", PowerCheckType.Single);
                    int _id=OtherDuihuanBLL.Add(model);
                    if (_id > 0)
                    {
                        //会员积分扣除
                        usermodel.PointLeft -= Convert.ToInt32(integral.Text);
                        UserBLL.Update(usermodel);

                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), "添加兑换记录", _id);
                    }
                }
                else
                {
                    ScriptHelper.Alert("会员ID不存在或已删除");
                    return;
                }
            }

            ScriptHelper.Alert(ShopLanguage.ReadLanguage("OperateOK"), RequestHelper.RawUrl);
        }
    }
}