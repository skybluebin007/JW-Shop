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
    public partial class AdminAdd : JWShop.Page.AdminBasePage
    {
        protected AdminInfo pageAdmin = new AdminInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!Page.IsPostBack)
            {
                GroupID.DataSource = AdminGroupBLL.ReadList();
                GroupID.DataTextField = "Name";
                GroupID.DataValueField = "ID";
                GroupID.DataBind();
                int tempAdminID = RequestHelper.GetQueryString<int>("ID");
                if (tempAdminID != int.MinValue)
                {
                    CheckAdminPower("ReadAdmin", PowerCheckType.Single);
                    pageAdmin = AdminBLL.Read(tempAdminID);
                    if (pageAdmin.Id == 1 || pageAdmin.Name.ToLower() == "admin")
                    {
                        Email.Enabled = false;
                        GroupID.Enabled = false;
                        Status.Enabled = false;
                    }
                    GroupID.Text = pageAdmin.GroupId.ToString();
                    Name.Text = pageAdmin.Name;
                    Name.Enabled = false;
                    Email.Text = pageAdmin.Email;
                    NoteBook.Text = pageAdmin.NoteBook;
                    Add.Visible = false;
                    AddStatus.Visible = true;
                    Status.Text = pageAdmin.Status.ToString();                   
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            AdminInfo admin = new AdminInfo();
            admin.Id = RequestHelper.GetQueryString<int>("ID");
            if (admin.Id > 0)
            {
                admin = AdminBLL.Read(admin.Id);
            }
            admin.Name = Name.Text;
            var _admin=AdminBLL.Read(admin.Name);
            if ((admin.Id > 0 && _admin.Id != admin.Id) || (admin.Id <= 0 && _admin.Id > 0))
            {
                ScriptHelper.Alert("管理员名已存在，请重新输入");
            }
            else
            {
                admin.Email = Email.Text;
                if (!AdminBLL.UniqueEmail(admin.Email, admin.Id))
                {
                    ScriptHelper.Alert("邮箱已存在，请重新输入");
                }
                else
                {
                    admin.GroupId = Convert.ToInt32(GroupID.Text);
                    admin.Password = StringHelper.Password(Password.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                    admin.LastLoginDate = RequestHelper.DateNow;
                    admin.LastLoginIP = ClientHelper.IP;
                    admin.IsCreate = 0;
                    admin.NoteBook = NoteBook.Text;

                    string alertMessage = ShopLanguage.ReadLanguage("AddOK");
                    if (admin.Id == int.MinValue)
                    {
                        CheckAdminPower("AddAdmin", PowerCheckType.Single);
                        admin.Status = (int)BoolType.True;
                        admin.FindDate = DateTime.Now;
                        int id = AdminBLL.Add(admin);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Admin"), id);
                    }
                    else
                    {
                        CheckAdminPower("UpdateAdmin", PowerCheckType.Single);
                        admin.Status = Convert.ToInt32(Status.Text);
                        AdminBLL.Update(admin);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Admin"), admin.Id);
                        alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                    }
                    ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
                }
            }
        }
    }
}