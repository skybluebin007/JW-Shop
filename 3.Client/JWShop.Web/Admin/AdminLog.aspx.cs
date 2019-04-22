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
    public partial class AdminLog : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadAdminLog", PowerCheckType.Single);

                int count = 0;
                var adminList = AdminBLL.ReadList(1, 1000, ref count);
                AdminId.DataSource = adminList;
                AdminId.DataTextField = "Name";
                AdminId.DataValueField = "ID";
                AdminId.DataBind();
                AdminId.Items.Insert(0, new ListItem("所有管理员", string.Empty));

                int adminId = RequestHelper.GetQueryString<int>("AdminId");
                string logType = RequestHelper.GetQueryString<string>("LogType");
                DateTime startAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                DateTime endAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));

                AdminId.Text = adminId.ToString();
                LogType.Text = logType;
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");

                var data = AdminLogBLL.ReadList(CurrentPage, PageSize, ref Count, logType, startAddDate, endAddDate, adminId);

                BindControl(data, RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteAdminLog", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                AdminLogBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)), 0);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("AdminLog"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "AdminLog.aspx?Action=search&";
            URL += "AdminId=" + AdminId.Text + "&"; ;
            URL += "LogType=" + LogType.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}