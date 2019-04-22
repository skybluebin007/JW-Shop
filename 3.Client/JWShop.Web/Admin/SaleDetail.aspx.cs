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
    public partial class SaleDetail : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsSale", PowerCheckType.Single);

                foreach (ProductClassInfo productClass in ProductClassBLL.ReadNamedList())
                {
                    ClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
                }
                ClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

                BrandID.DataSource = ProductBrandBLL.ReadList();
                BrandID.DataTextField = "Name";
                BrandID.DataValueField = "Id";
                BrandID.DataBind();
                BrandID.Items.Insert(0, new ListItem("所有品牌", string.Empty));

                ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                Name.Text = RequestHelper.GetQueryString<string>("Name");
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                UserName.Text = RequestHelper.GetQueryString<string>("UserName");
                OrderNumber.Text = RequestHelper.GetQueryString<string>("OrderNumber");

                ProductSearchInfo productSearch = new ProductSearchInfo();
                OrderSearchInfo orderSearch = new OrderSearchInfo();
                productSearch.IsSale = (int)BoolType.True;
                productSearch.Name = RequestHelper.GetQueryString<string>("Name");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                productSearch.InProductId = RequestHelper.GetQueryString<string>("ProductID");
                orderSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                orderSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                orderSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
                orderSearch.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
                BindControl(OrderDetailBLL.StatisticsSaleDetail(CurrentPage, PageSize, orderSearch, productSearch, ref Count), RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "SaleDetail.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "OrderNumber=" + OrderNumber.Text + "&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            productSearch.IsSale = (int)BoolType.True;
            productSearch.Name = ShopCommon.ConvertToT<string>(Name.Text);
            productSearch.ClassId = ShopCommon.ConvertToT<string>(ClassID.Text);
            productSearch.BrandId = ShopCommon.ConvertToT<int>(BrandID.Text);
            orderSearch.StartAddDate = ShopCommon.ConvertToT<DateTime>(StartAddDate.Text);
            orderSearch.EndAddDate = ShopCommon.SearchEndDate(ShopCommon.ConvertToT<DateTime>(EndAddDate.Text));
            orderSearch.UserName = ShopCommon.ConvertToT<string>(UserName.Text);
            orderSearch.OrderNumber = ShopCommon.ConvertToT<string>(OrderNumber.Text);
            var data = OrderDetailBLL.StatisticsSaleDetail(1, 1000, orderSearch, productSearch, ref Count);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 1, 0, 1);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("时间");
            row.CreateCell(1).SetCellValue("单号");
            row.CreateCell(2).SetCellValue("商品名称");
            row.CreateCell(3).SetCellValue("数量");
            row.CreateCell(4).SetCellValue("金额");
            row.CreateCell(5).SetCellValue("用户名");

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
                dataRow.CreateCell(0).SetCellValue(Convert.ToString(dr["AddDate"]));
                dataRow.CreateCell(1).SetCellValue(Convert.ToString(dr["OrderNumber"]));
                dataRow.CreateCell(2).SetCellValue(Convert.ToString(dr["Name"]));
                dataRow.CreateCell(3).SetCellValue(Convert.ToString(dr["BuyCount"]));
                dataRow.CreateCell(4).SetCellValue(Convert.ToString(dr["Money"]));
                dataRow.CreateCell(5).SetCellValue(Convert.ToString(dr["UserName"]));

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