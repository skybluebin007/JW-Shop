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
    public partial class UserConsume : JWShop.Page.AdminBasePage
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
                DateTime startDate = RequestHelper.GetQueryString<DateTime>("StartDate");
                DateTime endDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndDate"));
                UserName.Text = userSearch.UserName;
                StartDate.Text = RequestHelper.GetQueryString<string>("StartDate");
                EndDate.Text = RequestHelper.GetQueryString<string>("EndDate");
                Sex.Text = RequestHelper.GetQueryString<int>("Sex").ToString();
                string userConsumeType = RequestHelper.GetQueryString<string>("UserConsumeType");
                userConsumeType = (userConsumeType == string.Empty) ? "OrderCount" : userConsumeType;
                UserConsumeType.Text = userConsumeType;
                BindControl(UserBLL.StatisticsUserConsume(CurrentPage, PageSize, userSearch, ref Count, userConsumeType, startDate, endDate), RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserConsume.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Sex=" + Sex.Text + "&";
            URL += "UserConsumeType=" + UserConsumeType.Text + "&";
            URL += "StartDate=" + StartDate.Text + "&";
            URL += "EndDate=" + EndDate.Text;
            ResponseHelper.Redirect(URL);
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.UserName = ShopCommon.ConvertToT<string>(UserName.Text);
            userSearch.Sex = ShopCommon.ConvertToT<int>(Sex.Text);
            DateTime startDate = ShopCommon.ConvertToT<DateTime>(StartDate.Text);
            DateTime endDate = ShopCommon.SearchEndDate(ShopCommon.ConvertToT<DateTime>(EndDate.Text));
            string userConsumeType = ShopCommon.ConvertToT<string>(UserConsumeType.Text);
            userConsumeType = (userConsumeType == string.Empty) ? "OrderCount" : userConsumeType;
            var data = UserBLL.StatisticsUserConsume(1, 1000, userSearch, ref Count, userConsumeType, startDate, endDate);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 1, 0, 1);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("用户");
            row.CreateCell(1).SetCellValue("性别");
            row.CreateCell(2).SetCellValue("订单次数");
            row.CreateCell(3).SetCellValue("订单金额");

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
                dataRow.CreateCell(2).SetCellValue(Convert.ToString(dr["OrderCount"]));
                dataRow.CreateCell(3).SetCellValue(Convert.ToString(dr["OrderMoney"]));

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