using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class UserStatus : JWShop.Page.AdminBasePage
    {
        protected string result = string.Empty;
        protected string statusArr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsUser", PowerCheckType.Single);
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");
                DataTable dt = UserBLL.StatisticsUserStatus(userSearch);
                
                string[] color = { "33FF66", "FF6600", "FFCC33", "CC3399" };
                int i = 0;
                bool find = false;
                result += "[";
                foreach (EnumInfo temp in EnumHelper.ReadEnumList<JWShop.Entity.UserStatus>())
                {
                    statusArr = statusArr + "'" + temp.ChineseName + "',";
                    find = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt16(dr["Status"]) == temp.Value)
                        {
                            result += " {value:" + dr["Count"] + ", name:'" + temp.ChineseName + "'},";
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        result += " {value:0, name:'" + temp.ChineseName + "'},";
                    }
                    i++;
                }
                statusArr = statusArr.Substring(0, statusArr.Length - 1);
                result = result.Substring(0, result.Length - 1);
                result += "]";
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserStatus.aspx?Action=search&";
            URL += "StartRegisterDate=" + StartRegisterDate.Text + "&";
            URL += "EndRegisterDate=" + EndRegisterDate.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}