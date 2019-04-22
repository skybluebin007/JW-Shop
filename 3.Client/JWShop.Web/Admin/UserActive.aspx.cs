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
    public partial class UserActive : JWShop.Page.AdminBasePage
    {
        protected string result = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsUser", PowerCheckType.Single);
                Sex.DataSource = EnumHelper.ReadEnumList<SexType>();
                Sex.DataValueField = "Value";
                Sex.DataTextField = "ChineseName";
                Sex.DataBind();
                Sex.Items.Insert(0, new ListItem("所有", string.Empty));

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
                userSearch.Sex = RequestHelper.GetQueryString<int>("Sex");
                userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                UserName.Text = userSearch.UserName;
                StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");
                Sex.Text = RequestHelper.GetQueryString<int>("Sex").ToString();
                string userOrderType = RequestHelper.GetQueryString<string>("UserOrderType");
                userOrderType = (userOrderType == string.Empty) ? "LoginTimes" : userOrderType;
                UserOrderType.Text = userOrderType;
                BindControl(UserBLL.StatisticsUserActive(CurrentPage, PageSize, userSearch, ref Count, userOrderType), RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserActive.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Sex=" + Sex.Text + "&";
            URL += "UserOrderType=" + UserOrderType.Text + "&";
            URL += "StartRegisterDate=" + StartRegisterDate.Text + "&";
            URL += "EndRegisterDate=" + EndRegisterDate.Text;
            ResponseHelper.Redirect(URL);
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.UserName = ShopCommon.ConvertToT<string>(UserName.Text);
            userSearch.Sex = ShopCommon.ConvertToT<int>(Sex.Text);
            userSearch.StartRegisterDate = ShopCommon.ConvertToT<DateTime>(StartRegisterDate.Text);
            userSearch.EndRegisterDate = ShopCommon.SearchEndDate(ShopCommon.ConvertToT<DateTime>(EndRegisterDate.Text));
            string userOrderType = ShopCommon.ConvertToT<string>(UserOrderType.Text);
            userOrderType = (userOrderType == string.Empty) ? "LoginTimes" : userOrderType;
            var data = UserBLL.StatisticsUserActive(1, 1000, userSearch, ref Count, userOrderType);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 1, 0, 1);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("用户");
            row.CreateCell(1).SetCellValue("性别");
            row.CreateCell(2).SetCellValue("注册时间");
            row.CreateCell(3).SetCellValue("登录次数");
            row.CreateCell(4).SetCellValue("评论次数");

            //设置表头格式
            var headFont = book.CreateFont();
            headFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            headFont.FontHeightInPoints = 10;
            var headStyle = book.CreateCellStyle();
            headStyle.SetFont(headFont);
            headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            foreach (var cell in row.Cells)
            {
                cell.CellStyle = headStyle;
            }

            foreach (DataRow dr in data.Rows)
            {
                NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(data.Rows.IndexOf(dr) + 1);
                dataRow.CreateCell(0).SetCellValue(Convert.ToString(dr["UserName"]));
                dataRow.CreateCell(1).SetCellValue(EnumHelper.ReadEnumChineseName<SexType>(Convert.ToInt32(dr["Sex"])));
                dataRow.CreateCell(2).SetCellValue(Convert.ToString(dr["RegisterDate"]));
                dataRow.CreateCell(3).SetCellValue(Convert.ToString(dr["LoginTimes"]));
                dataRow.CreateCell(4).SetCellValue(Convert.ToString(dr["CommentCount"]));

                var style = book.CreateCellStyle();
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                foreach (var cell in dataRow.Cells)
                {
                    cell.CellStyle = style;
                }
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
            Response.End();
        }

    }
}